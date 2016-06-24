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
using System.Threading.Tasks;

namespace BuddhabrotCL
{
    public partial class MainForm : Form
    {
        const string DEFAULT_KERNEL_FILENAME = "/buddhabrot/cl_heuristic.c";
        const string DEFAULT_KERNEL_DIR = "kernel";
        const string APP_NAME = "BuddhabrotCL";
        string AppFullName = APP_NAME;

        BrotParams bp = new BrotParams();
        ComputePlatform cPlatform = null;
        Brush dimBrush = new SolidBrush(Color.FromArgb(100, Color.White));
        bool isRunning = false;
        Stopwatch hpet = new Stopwatch();
        bool autoUpdate = true;

        Buddhabrot bb;

        Bitmap backBitmap = null;
        Bitmap frontBitmap = null;
        bool __should_update = false;
        object __frontLocker = new object();
        Thread thGenerator;
        Thread thPainter;
        CancellationTokenSource cts;
        string kernelFilename = DEFAULT_KERNEL_DIR + DEFAULT_KERNEL_FILENAME;

        public MainForm()
        {
            InitializeComponent();

            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            AppFullName = $"{APP_NAME} {v.Major}.{v.Minor}.{v.Build}";
            Text = AppFullName;

            foreach (var p in ComputePlatform.Platforms)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)platformMenuItem.DropDownItems.Add(p.Name);
                item.CheckOnClick = false;
                item.Tag = p;
                item.Click += platformMenuSubItem_Click;
            }

            if (platformMenuItem.HasDropDownItems)
            {
                ((ToolStripMenuItem)platformMenuItem.DropDownItems[0]).Checked = true;
                Platform = ComputePlatform.Platforms[0];
            }

            DirectoryInfo di = new DirectoryInfo(DEFAULT_KERNEL_DIR);
            KernelDirs(di, kernelsMenuItem);

            startButton.Enabled = startMenuItem.Enabled = true;
            stopButton.Enabled = stopMenuItem.Enabled = false;

            propertyGrid.SelectedObject = bp;
            KernelFilename = kernelFilename;
        }

        private void KernelDirs(DirectoryInfo parentDirInfo, ToolStripMenuItem parentMenuItem)
        {
            DirectoryInfo[] dirs = parentDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo di in dirs)
            {
                ToolStripMenuItem dItem = (ToolStripMenuItem)parentMenuItem.DropDownItems.Add(di.Name);
                KernelDirs(di, dItem);
            }

            FileInfo[] files = parentDirInfo.GetFiles("cl_*.c", SearchOption.TopDirectoryOnly);
            foreach(FileInfo fi in files)
            {
                ToolStripMenuItem fileItem = (ToolStripMenuItem)parentMenuItem.DropDownItems.Add(fi.Name);
                fileItem.Tag = fi.FullName;
                fileItem.Click += kernelsMenuSubItem_Click;
            }
        }

        private string KernelFilename
        {
            set
            {
                kernelFilename = value;
                string filename = new FileInfo(kernelFilename).Name;
                kernelStatusLabel.Text = $"Kernel: {filename}";
                Text = $"{filename} - {AppFullName}";
            }
        }

        private void kernelsMenuSubItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem fileItem = (ToolStripMenuItem)sender;
            KernelFilename = (string)fileItem.Tag;
        }

        private ComputePlatform Platform
        {
            set
            {
                cPlatform = value;
                platformStatusLabel.Text = $"{cPlatform.Vendor.Replace(" Corporation","")} {cPlatform.Version}";
            }
        }

        private void platformMenuSubItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (!item.Checked)
            {
                foreach (ToolStripMenuItem it in platformMenuItem.DropDownItems)
                    it.Checked = it == item;

                Platform = item.Tag as ComputePlatform;
            }
        }

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

            try
            {
                string kernelSource = File.ReadAllText(kernelFilename);

                bb = new Buddhabrot(cPlatform, kernelSource, bp);

                bb.BuildKernels();
                bb.AllocateBuffers();
                bb.ConfigureKernel();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Sorry, can't build kernel!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            startButton.Enabled = startMenuItem.Enabled = false;
            stopButton.Enabled = stopMenuItem.Enabled = true;                        

            if (backBitmap != null)
            {
                backBitmap.Dispose();
                backBitmap = null;
            }

            if (frontBitmap != null)
            {
                frontBitmap.Dispose();
                frontBitmap = null;
            }

            Graphics g = Graphics.FromHwnd(Handle);
            backBitmap = new Bitmap(bp.width, bp.height, g);
            drawPanel.Width = bp.width;
            drawPanel.Height = bp.height;
            frontBitmap = new Bitmap(backBitmap);

            cts = new CancellationTokenSource();

            SynchronizationContext ui = SynchronizationContext.Current;

            thGenerator = new Thread(() => Generate(ui, cts.Token));
            thGenerator.IsBackground = false;

            thPainter = new Thread(() => DrawResultBuffer(ui, cts.Token));
            thPainter.IsBackground = true;

            isRunning = true;
            hpet.Restart();
            thGenerator.Start();
            thPainter.Start();
        }

        private void DrawResultBuffer(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                long hpet_ubbb_st = hpet.ElapsedTicks;
                if (__should_update && ui != null)
                {
                    __should_update = false;
                    if (autoUpdate)
                        UpdateBackBuffer();

                    if (token.IsCancellationRequested)
                        return;

                    ui.Send(
                        (object o) =>
                        {
                            if (hpet_ktime > 0)
                            {
                                long ktime = hpet_ktime / TimeSpan.TicksPerMillisecond;
                                if (ktime >= 1000)
                                    kernelTimeStatusLabel.Text = $"Core: {(ktime/1000f).ToString("0.00")}s";
                                else
                                    kernelTimeStatusLabel.Text = $"Core: {ktime}ms";
                            }

                            TimeSpan ts = TimeSpan.FromMilliseconds(hpet.ElapsedMilliseconds);
                            renderTimeStatusLabel.Text = $"Rendering: {ts.ToString(@"dd\ hh\:mm\:ss")}";

                            if (autoUpdate)
                                drawPanel.Invalidate();
                        }, null);
                } // if

                if(hpet.ElapsedTicks - hpet_ubbb_st > 2500000000)
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

                bb.ExecuteKernel_Buddhabrot();
                if (token.IsCancellationRequested) break;

                bb.ReadResult();
                if (token.IsCancellationRequested) break;

                hpet_ktime = hpet.ElapsedTicks - hpet_start;
                __should_update = true;
            }
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            await Task.Run(() => { while (thPainter.IsAlive) Thread.Sleep(0); });

            UpdateBackBuffer();

            isRunning = false;
            hpet.Stop();
            startButton.Enabled = startMenuItem.Enabled = true;
            stopButton.Enabled = stopMenuItem.Enabled = false;
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
            UpdateBackBuffer(new Rectangle(0, 0, backBitmap.Width, backBitmap.Height));
        }

        private void UpdateBackBuffer(Rectangle region)
        {
            if (backBitmap == null || region.Width == 0 || region.Height == 0)
                return;

            float maxR = float.MinValue;
            float maxG = float.MinValue;
            float maxB = float.MinValue;

            int l = bb.h_resultBuf.Length;
            if (bp.isGrayscale)
                for (int i = 0; i < l; i++)
                    maxR = Math.Max(bb.h_resultBuf[i].r, maxR);
            else
                for (int i = 0; i < l; i++)
                {
                    maxR = Math.Max(bb.h_resultBuf[i].r, maxR);
                    maxG = Math.Max(bb.h_resultBuf[i].g, maxG);
                    maxB = Math.Max(bb.h_resultBuf[i].b, maxB);
                }

            int bitLen = backBitmap.PixelFormat == PixelFormat.Format24bppRgb ? 3 : 4;

            BitmapData bitmapData = backBitmap.LockBits(
                region,
                ImageLockMode.WriteOnly,
                backBitmap.PixelFormat);
            byte[] bitmapBuf = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, bitmapBuf, 0, bitmapBuf.Length);

            float scaleR = 255f * bp.Exposure / Fx(maxR, bp.Factor);
            float scaleG = bp.isGrayscale ? 0f : 255f * bp.Exposure / Fx(maxG, bp.Factor);
            float scaleB = bp.isGrayscale ? 0f : 255f * bp.Exposure / Fx(maxB, bp.Factor);

            for (int y = 0; y < region.Height; y++)
            {
                for (int x = 0; x < region.Width; x++)
                {
                    int i = (x + region.X) + (y + region.Y) * bp.width;
                    int j = bitmapData.Stride * y + x * bitLen;

                    byte r, g, b;
                    if (bp.isGrayscale)
                        r = g = b = ByteClamp(scaleR * Fx(bb.h_resultBuf[i].r, bp.Factor));
                    else
                    {
                        r = ByteClamp(scaleR * Fx(bb.h_resultBuf[i].r, bp.Factor));
                        g = ByteClamp(scaleG * Fx(bb.h_resultBuf[i].g, bp.Factor));
                        b = ByteClamp(scaleB * Fx(bb.h_resultBuf[i].b, bp.Factor));
                    }

                    switch(bp.Tint)
                    {
                        case Tint.BGR:
                            bitmapBuf[j + 0] = b;
                            bitmapBuf[j + 1] = g;
                            bitmapBuf[j + 2] = r;
                            break;
                        case Tint.RGB:
                            bitmapBuf[j + 0] = r;
                            bitmapBuf[j + 1] = g;
                            bitmapBuf[j + 2] = b;
                            break;
                        case Tint.BRG:
                            bitmapBuf[j + 0] = b;
                            bitmapBuf[j + 1] = r;
                            bitmapBuf[j + 2] = g;
                            break;
                        case Tint.RBG:
                            bitmapBuf[j + 0] = r;
                            bitmapBuf[j + 1] = b;
                            bitmapBuf[j + 2] = g;
                            break;
                        case Tint.GRB:
                            bitmapBuf[j + 0] = g;
                            bitmapBuf[j + 1] = r;
                            bitmapBuf[j + 2] = b;
                            break;
                        case Tint.GBR:
                            bitmapBuf[j + 0] = g;
                            bitmapBuf[j + 1] = b;
                            bitmapBuf[j + 2] = r;
                            break;
                    }

                    if (bitLen == 4)
                        bitmapBuf[j + 3] = 255;
                } // for x
            } // for y

            Marshal.Copy(bitmapBuf, 0, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height);
            backBitmap.UnlockBits(bitmapData);

            lock (__frontLocker)
            {
                Graphics g = Graphics.FromImage(frontBitmap);
                g.DrawImageUnscaled(backBitmap, 0, 0);
            }
        }

        private byte ByteClamp(float value)
        {
            if (value > 255f)
                value = 255f;
            else
                if (value < 0f)
                    value = 0f;

            return (byte)value;
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
            Graphics g = Graphics.FromImage(frontBitmap);
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
            bool uc = autoUpdate;
            autoUpdate = false;

            if (!isRunning)
            {
                UpdateBackBuffer();
                drawPanel.Invalidate();
            }

            if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                backBitmap.Save(saveImageFileDialog.FileName);

            autoUpdate = uc;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            lock (__frontLocker)
                if (frontBitmap != null)
                    e.Graphics.DrawImageUnscaled(frontBitmap, 0, 0);
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (bb == null)
                return;

            int hNX = bp.width >> 1;
            int hNY = bp.height >> 1;

            string xstr = ToRe(e.X).ToString("0.00000000").Replace(",", ".");
            string ystr = ToIm(e.Y).ToString("0.00000000").Replace(",", ".");
            coordStatusLabel.Text = $"[Re; Im]= {xstr}; {ystr}";

            if (isDrag)
            {
                if (!isRunning)
                {
                    Rectangle r = GetCursorRect(dragCur);
                    if ((r.Width > 0) && (r.Height > 0))
                    {
                        lock (__frontLocker)
                            Graphics.FromImage(frontBitmap).DrawImage(backBitmap, r, r, GraphicsUnit.Pixel);
                    }
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
                KernelFilename = openFileDialog.FileName;
        }

        bool isDrag = false;
        Point dragStart;
        Point dragStop;
        Point dragCur;
        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (backBitmap == null)
                return;

            if (!isRunning)
            {
                Rectangle r = GetCursorRect(dragStop);
                lock (__frontLocker)
                    Graphics.FromImage(frontBitmap).DrawImage(backBitmap, r, r, GraphicsUnit.Pixel);
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

                if (maxl > 0)
                {
                    bp.ReMin = ToRe(dragStart.X - maxl);
                    bp.ReMax = ToRe(dragStart.X + maxl);
                    bp.ImMin = ToIm(dragStart.Y + maxl);
                    bp.ImMax = ToIm(dragStart.Y - maxl);

                    propertyGrid.Refresh();

                    if (!isRunning)
                        drawPanel.Invalidate();
                }
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
                lock (__frontLocker)
                    Graphics.FromImage(frontBitmap).DrawImageUnscaled(backBitmap, 0, 0);
                drawPanel.Invalidate();
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (!isRunning)
                switch(e.ChangedItem.Label)
                {
                    case "Type":
                    case "Factor":
                    case "Exposure":
                    case "Tint":
                        UpdateBackBuffer();
                        drawPanel.Invalidate();
                        break;
                }
        }

        public static void SetLabelColumnWidth(PropertyGrid grid, int width)
        {
            if (grid == null)
                return;

            FieldInfo fi = grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                return;

            Control view = fi.GetValue(grid) as Control;
            if (view == null)
                return;

            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi == null)
                return;
            mi.Invoke(view, new object[] { width });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetLabelColumnWidth(propertyGrid, propertyGrid.Width / 2);
            autoupdateMenuItem.Checked = autoupdateButton.Checked = autoUpdate;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoupdateMenuItem_Click(object sender, EventArgs e)
        {
            autoUpdate = !autoUpdate;
            autoupdateMenuItem.Checked = autoupdateButton.Checked = autoUpdate;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, $"{AppFullName}\n(c) Nikolai Voronin 2011-2016\n\nhttps://github.com/nikvoronin/BuddhabrotCL", $"About {AppFullName}");
        }
    }
}
