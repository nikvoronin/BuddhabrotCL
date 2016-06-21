using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using Cloo;

namespace BuddhabrotCL
{
    public partial class MainForm : Form
    {
        const string DEFAULT_KERNEL_FILENAME_BBROT = "cl_heuristic.c";
        const string DEFAULT_KERNEL_FILENAME_RNG = "cl_xorshift.c";

        BrotParams bp = new BrotParams();
        ComputePlatform cPlatform = null;

        public MainForm()
        {
            InitializeComponent();

            foreach (var p in ComputePlatform.Platforms)
            {
                ToolStripItem item = platformDropDownButton.DropDownItems.Add(p.Name);
                item.Tag = p;
            }

            if (platformDropDownButton.HasDropDownItems)
            {
                cPlatform = ComputePlatform.Platforms[0];
                platformDropDownButton.Text = cPlatform.Name;
            }

            kernelButton.Text = new FileInfo(bbrotKernelPath).Name;
            rngButton.Text = new FileInfo(rngKernelPath).Name;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            propertyGrid.SelectedObject = bp;
        }


        Buddhabrot bbrot;

        Bitmap bitmap = null;
        bool __should_update = false;
        bool __lock_backbuffer = true;
        Thread workThread;
        Thread paintThread;
        Stopwatch hpet = new Stopwatch();
        float cps = 0f;
        long lastCps = 0;
        CancellationTokenSource cts;
        string bbrotKernelPath = "kernel/" + DEFAULT_KERNEL_FILENAME_BBROT;
        string rngKernelPath = "rng/" + DEFAULT_KERNEL_FILENAME_RNG;

        private void TransferBBrotParameters()
        {
            bp.isGrayscale = bp.IsGrayscale;
            bp.width = (int)bp.Width;
            bp.height = (int)bp.Height;
            bp.iterMax = bp.IterationsMax;
            bp.iterMin = bp.IterationsMin;
            bp.escapeOrbit = bp.EscapeOrbit;

            bp.reMin = bp.ReMin;
            bp.reMax = bp.ReMax;
            bp.imMin = bp.ImMin;
            bp.imMax = bp.ImMax;


            bp.RecalculateColors();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            TransferBBrotParameters();
            startButton.Enabled = kernelButton.Enabled = rngButton.Enabled = platformDropDownButton.Enabled = false;
            stopButton.Enabled = true;

            string kernelSource = File.ReadAllText(bbrotKernelPath);
            string rngSource = File.ReadAllText(rngKernelPath);

            bbrot = new Buddhabrot(cPlatform, kernelSource, rngSource, bp);
            
            bbrot.BuildKernels();
            bbrot.AllocateBuffers();
            bbrot.ConfigureKernel();
            
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            Graphics g = Graphics.FromHwnd(this.Handle);
            bitmap = new Bitmap(bp.width, bp.height, g);
            drawPanel.Width = bp.width;
            drawPanel.Height = bp.height;

            cts = new CancellationTokenSource();

            cps = 0f;
            hpet.Restart();
            SynchronizationContext ui = SynchronizationContext.Current;

            workThread = new Thread(() => Generate(ui, cts.Token));
            workThread.IsBackground = false;

            paintThread = new Thread(() => DrawComputeBuffer(ui, cts.Token));
            paintThread.IsBackground = false;

            workThread.Start();
            paintThread.Start();
        }

        private void DrawComputeBuffer(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (__should_update && ui != null)
                {
                    UpdateBackBuffer();

                    if (token.IsCancellationRequested)
                        return;

                    ui.Send(
                        (object o) =>
                        {
                            if (cps < float.MaxValue - 1000)
                            {
                                cpsStatusLabel.Text = cps.ToString("0.00");
                                cps = float.MaxValue;
                            }

                            drawPanel.Invalidate();
                        }, null);
                } // if

                Thread.Sleep(100);
            } // while
        }

        private void Generate(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                bbrot.ExecuteKernel_Rng();
                if (token.IsCancellationRequested) break;

                bbrot.ExecuteKernel_Buddhabrot();
                if (token.IsCancellationRequested) break;

                bbrot.ReadResult();
                if (token.IsCancellationRequested) break;

                float delta = hpet.ElapsedTicks - lastCps + 1;
                cps = Math.Min(cps, TimeSpan.TicksPerSecond / delta);
                lastCps = hpet.ElapsedTicks;

                __should_update = true;
                Thread.Sleep(1);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            hpet.Stop();
            cts?.Cancel();

            while (__lock_backbuffer) { Thread.Sleep(100); }

            UpdateBackBuffer();

            startButton.Enabled = kernelButton.Enabled = rngButton.Enabled = platformDropDownButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private float Fx(float x, float factor = 1.0f)
        {
            switch (bp.Filter)
            {                
                case FxFilter.Sqrt:
                    return (float)Math.Sqrt(factor * x);                
                case FxFilter.Log:
                    return (float)Math.Log(factor * x + 1);
                case FxFilter.Log10:
                    return (float)Math.Log10(factor * x + 1);
                case FxFilter.Exp:
                    return 1.0f - (float)Math.Exp(-factor * x);
                case FxFilter.Linear:
                default:
                    return x * factor;
            }
        }

        private void UpdateBackBuffer()
        {
            __lock_backbuffer = true;
            float maxR = float.MinValue;
            float maxG = float.MinValue;
            float maxB = float.MinValue;

            int l = bbrot.h_resultBuffer.Length;
            if (bp.isGrayscale)
                for (int i = 0; i < l; i++)
                    maxR = Math.Max(bbrot.h_resultBuffer[i].r, maxR);
            else
                for (int i = 0; i < l; i++)
                {
                    maxR = Math.Max(bbrot.h_resultBuffer[i].r, maxR);
                    maxG = Math.Max(bbrot.h_resultBuffer[i].g, maxG);
                    maxB = Math.Max(bbrot.h_resultBuffer[i].b, maxB);
                }

            int bitLen = bitmap.PixelFormat == PixelFormat.Format24bppRgb ? 3 : 4;

            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            byte[] bitmapBuffer = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, bitmapBuffer, 0, bitmapBuffer.Length);

            float scaleR = 255f * bp.Overexposure / Fx(maxR, bp.Factor);
            float scaleG = bp.isGrayscale ? 0f : 255f * bp.Overexposure / Fx(maxG, bp.Factor);
            float scaleB = bp.isGrayscale ? 0f : 255f * bp.Overexposure / Fx(maxB, bp.Factor);

            for (int y = 0; y < bp.height; y++)
            {
                for (int x = 0; x < bp.width; x++)
                {
                    int i = x + y * bp.width;
                    int j = bitmapData.Stride * y + x * bitLen;

                    byte r, g, b;
                    if (bp.isGrayscale)
                        r = g = b = (byte)(scaleR * Fx(bbrot.h_resultBuffer[i].r, bp.Factor));
                    else
                    {
                        r = (byte)(scaleR * Fx(bbrot.h_resultBuffer[i].r, bp.Factor));
                        g = (byte)(scaleG * Fx(bbrot.h_resultBuffer[i].g, bp.Factor));
                        b = (byte)(scaleB * Fx(bbrot.h_resultBuffer[i].b, bp.Factor));
                    }

                    bitmapBuffer[j + 0] = b;
                    bitmapBuffer[j + 1] = g;
                    bitmapBuffer[j + 2] = r;

                    if (bitLen == 4)
                        bitmapBuffer[j + 3] = 255;
                } // for x
            } // for y

            Marshal.Copy(bitmapBuffer, 0, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height);
            bitmap.UnlockBits(bitmapData);

            if (isDrag)
            {
                //float k = (float)bp.width / bp.height;
                Graphics g = Graphics.FromImage(bitmap);
                Brush b = new SolidBrush(Color.FromArgb(100, Color.White));
                float xl = Math.Abs(dragStart.X - dragCur.X);
                float yl = Math.Abs(dragStart.Y - dragCur.Y);
                float maxl = Math.Max(xl, yl);

                g.FillRectangle(
                //g.FillEllipse(
                    b,
                    dragStart.X - maxl,
                    dragStart.Y - maxl,
                    maxl * 2,
                    maxl * 2
                    );

                g.DrawLine(Pens.White,
                    dragStart.X - 10,
                    dragStart.Y,
                    dragStart.X + 10,
                    dragStart.Y
                    );

                g.DrawLine(Pens.White,
                    dragStart.X,
                    dragStart.Y - 10,
                    dragStart.X,
                    dragStart.Y + 10
                    );
            }

            __lock_backbuffer = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts?.Cancel();
        }

        private void saveAsImageButton_Click(object sender, EventArgs e)
        {
            if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                bitmap.Save(saveImageFileDialog.FileName);
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            if (bitmap != null && !__lock_backbuffer)
                e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (bbrot == null)
                return;

            int hNX = bp.width >> 1;
            int hNY = bp.height >> 1;

            string xstr = ToRe(e.X).ToString().Replace(",", ".");
            string ystr = ToIm(e.Y).ToString().Replace(",", ".");
            coordStatusLabel.Text = xstr + "; " + ystr;

            if (isDrag)
                dragCur = e.Location;
        }

        private void kernelButton_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bbrotKernelPath = openFileDialog.FileName;
                kernelButton.Text = new FileInfo(bbrotKernelPath).Name;
            }
        }

        private void randomButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rngKernelPath = openFileDialog.FileName;
                rngButton.Text = new FileInfo(rngKernelPath).Name;
            }
        }

        private void platformDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            cPlatform = e.ClickedItem.Tag as ComputePlatform;
            platformDropDownButton.Text = cPlatform.Name;
        }

        bool isDrag = false;
        Point dragStart;
        Point dragStop;
        Point dragCur;
        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            dragStop = dragStart;
            dragStart = e.Location;
            isDrag = true;
        }

        private float ToRe(int x)
        {
            return (Math.Abs(bp.reMax - bp.reMin)) / bp.width * x + bp.reMin;
        }

        private float ToIm(int y)
        {
            return -((Math.Abs(bp.imMax - bp.imMin)) / bp.height * y + bp.imMin);
        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                dragStop = e.Location;

                int xl = Math.Abs(dragStart.X - dragStop.X);
                int yl = Math.Abs(dragStart.Y - dragStop.Y);
                int maxl = Math.Max(xl, yl);

                bp.ReMin = ToRe(dragStart.X - maxl);
                bp.ReMax = ToRe(dragStart.X + maxl);
                bp.ImMin = ToIm(dragStart.Y + maxl);
                bp.ImMax = ToIm(dragStart.Y - maxl);
            }
            isDrag = false;
        }

        private void fullViewButton_Click(object sender, EventArgs e)
        {
            bp.ReMin = -2f;
            bp.ReMax = 2f;
            bp.ImMin = -2f;
            bp.ImMax = 2f;
        }
    }
}
