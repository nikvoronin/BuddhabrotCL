using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using Cloo;
using System.Reflection;
using System.Diagnostics;

namespace BuddhabrotCL
{
    public partial class MainForm : Form
    {
        const string DEFAULT_KERNEL_FILENAME_BBROT = "cl_heuristic.c";

        BrotParams bp = new BrotParams();
        ComputePlatform cPlatform = null;
        Brush dimBrush = new SolidBrush(Color.FromArgb(100, Color.White));
        bool isRunning = false;
        Stopwatch hpet = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"BuddhabrotCL {v.Major}.{v.Minor}.{v.Build}";

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

            startButton.Enabled = true;
            stopButton.Enabled = false;

            propertyGrid.SelectedObject = bp;
        }


        Buddhabrot bbrot;

        Bitmap bitmap = null;
        bool __should_update = false;
        bool __lock_backbuffer = true;
        Thread workThread;
        Thread drawThread;
        CancellationTokenSource cts;
        string bbrotKernelPath = "kernel/" + DEFAULT_KERNEL_FILENAME_BBROT;

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

            bp.workers = bp.Workers;

            bp.RecalculateColors();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            TransferBBrotParameters();
            startButton.Enabled = kernelButton.Enabled = platformDropDownButton.Enabled = false;
            stopButton.Enabled = true;

            string kernelSource = File.ReadAllText(bbrotKernelPath);

            bbrot = new Buddhabrot(cPlatform, kernelSource, bp);
            
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

            SynchronizationContext ui = SynchronizationContext.Current;

            workThread = new Thread(() => Generate(ui, cts.Token));
            workThread.IsBackground = false;

            drawThread = new Thread(() => DrawResultBuffer(ui, cts.Token));
            drawThread.IsBackground = false;

            isRunning = true;
            hpet.Restart();
            workThread.Start();
            drawThread.Start();
        }

        private void DrawResultBuffer(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (__should_update && ui != null)
                {
                    __should_update = false;
                    if (bp.UpdateCyclic)
                        UpdateBackBufferBitmap();

                    if (token.IsCancellationRequested)
                        return;

                    ui.Send(
                        (object o) =>
                        {
                            if (hpet_ktime > 0)
                            {
                                long ktime = hpet_ktime / TimeSpan.TicksPerMillisecond;
                                kernelTimeStatusLabel.Text = $"{ktime}ms";
                            }

                            if (bp.UpdateCyclic)
                                drawPanel.Invalidate();
                        }, null);
                } // if

                Thread.Sleep(250);
            } // while
        }

        long hpet_ktime = 0;
        long hpet_start = 0;
        private void Generate(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                hpet_start = hpet.ElapsedTicks;

                bbrot.ExecuteKernel_Buddhabrot();
                if (token.IsCancellationRequested) break;

                bbrot.ReadResult();
                if (token.IsCancellationRequested) break;

                hpet_ktime = hpet.ElapsedTicks - hpet_start;
                __should_update = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            cts?.Cancel();

            while (__lock_backbuffer) { Thread.Sleep(100); }

            UpdateBackBufferBitmap();

            isRunning = false;
            hpet.Stop();
            startButton.Enabled = kernelButton.Enabled = platformDropDownButton.Enabled = true;
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

        private void UpdateBackBufferBitmap()
        {
            UpdateBackBufferBitmap(new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }

        private void UpdateBackBufferBitmap(Rectangle region)
        {
            if (bitmap == null || region.Width == 0 || region.Height == 0)
                return;

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
                region,
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            byte[] bitmapBuffer = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, bitmapBuffer, 0, bitmapBuffer.Length);

            float scaleR = 255f * bp.Overexposure / Fx(maxR, bp.Factor);
            float scaleG = bp.isGrayscale ? 0f : 255f * bp.Overexposure / Fx(maxG, bp.Factor);
            float scaleB = bp.isGrayscale ? 0f : 255f * bp.Overexposure / Fx(maxB, bp.Factor);

            for (int y = 0; y < region.Height; y++)
            {
                for (int x = 0; x < region.Width; x++)
                {
                    int i = (x + region.X) + (y + region.Y) * bp.width;
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

                    switch(bp.Tint)
                    {
                        case Tint.Red:
                            bitmapBuffer[j + 0] = b;
                            bitmapBuffer[j + 1] = g;
                            bitmapBuffer[j + 2] = r;
                            break;
                        case Tint.Blue:
                            bitmapBuffer[j + 0] = r;
                            bitmapBuffer[j + 1] = g;
                            bitmapBuffer[j + 2] = b;
                            break;
                        case Tint.Green:
                            bitmapBuffer[j + 0] = b;
                            bitmapBuffer[j + 1] = r;
                            bitmapBuffer[j + 2] = g;
                            break;
                    }

                    if (bitLen == 4)
                        bitmapBuffer[j + 3] = 255;
                } // for x
            } // for y

            Marshal.Copy(bitmapBuffer, 0, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height);
            bitmap.UnlockBits(bitmapData);

            if (isDrag && isRunning)
                DrawCursor();

            __lock_backbuffer = false;
        }

        Rectangle GetCursorRect(Point cur)
        {
            float xl = Math.Abs(dragStart.X - cur.X);
            float yl = Math.Abs(dragStart.Y - cur.Y);
            float maxl = Math.Max(xl, yl);

            return
                new Rectangle(
                    (int)(dragStart.X - maxl),
                    (int)(dragStart.Y - maxl),
                    (int)(maxl * 2),
                    (int)(maxl * 2)
                    );
        }

        private void DrawCursor()
        {
            //float k = (float)bp.width / bp.height;
            Graphics g = Graphics.FromImage(bitmap);
            Rectangle rect = GetCursorRect(dragCur);

            g.FillRectangle(
                dimBrush,
                rect
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
            {
                __lock_backbuffer = true;
                e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
                __lock_backbuffer = false;
            }
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
            {
                if (!isRunning)
                {
                    Rectangle r = GetCursorRect(dragCur);
                    if ((r.Width > 0) && (r.Height > 0))
                        UpdateBackBufferBitmap(r);
                }

                dragCur = e.Location;

                if (!isRunning)
                {
                    DrawCursor();
                    drawPanel.Invalidate();
                }
            }
        }

        private void kernelButton_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bbrotKernelPath = openFileDialog.FileName;
                kernelButton.Text = new FileInfo(bbrotKernelPath).Name;
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
            if (bitmap == null)
                return;

            if (!isRunning)
            {
                Rectangle r = GetCursorRect(dragStop);
                UpdateBackBufferBitmap(r);
            }

            dragStart = e.Location;
            dragStop = dragCur = dragStart;
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

                propertyGrid.Refresh();
            }
            isDrag = false;
        }

        private void fullViewButton_Click(object sender, EventArgs e)
        {
            bp.ReMin = -2f;
            bp.ReMax = 2f;
            bp.ImMin = -2f;
            bp.ImMax = 2f;
            propertyGrid.Refresh();

            if (!isRunning)
            {
                UpdateBackBufferBitmap();
                drawPanel.Invalidate();
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (!isRunning)
                switch(e.ChangedItem.Label)
                {
                    case "Filter":
                    case "Factor":
                    case "Overexposure":
                        UpdateBackBufferBitmap();
                        drawPanel.Invalidate();
                        break;
                }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if(!isRunning)
            {
                UpdateBackBufferBitmap();
                drawPanel.Invalidate();
            }
        }
    }
}
