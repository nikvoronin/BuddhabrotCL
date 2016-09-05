using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using Cloo;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using BuddhabrotCL.Properties;

namespace BuddhabrotCL
{
    public partial class MainForm : Form, IDisposable
    {
        const string DEFAULT_KERNEL_FILENAME = "/buddhabrot/cl_heuristic.c";
        const string DEFAULT_KERNEL_DIR = "kernel";
        const string APP_NAME = "BuddhabrotCL";
        const int CURSOR_DIVW = 10;
        const int CURSOR_DIVH = 10;
        const int THREAD_PAINT_SLEEP_INTERVAL = 500;

        string AppFullName = APP_NAME;

        RenderParams rp = new RenderParams();
        FilterParams fp = new FilterParams();
        ComputePlatform cPlatform = null;
        Brush dimBrush = new SolidBrush(Color.FromArgb(100, Color.White));
        bool isRunning = false;
        Stopwatch hpet = new Stopwatch();
        bool autoRefresh = true;

        Render render;

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

            propertyGrid.SelectedObject = rp;
            filterGrid.SelectedObject = fp;
            KernelFilename = kernelFilename;
        }

        private int KernelDirs(DirectoryInfo parentDirInfo, ToolStripMenuItem parentMenuItem)
        {
            int childs = 0;

            DirectoryInfo[] dirs = parentDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo di in dirs)
            {
                ToolStripMenuItem dItem = new ToolStripMenuItem(di.Name);
                dItem.Image = Resources.folder_horizontal;
                int grandch = KernelDirs(di, dItem);
                if (grandch > 0)
                {
                    childs += grandch;
                    parentMenuItem.DropDownItems.Add(dItem);
                }
            } // foreach

            FileInfo[] files = parentDirInfo.GetFiles("cl_*.c", SearchOption.TopDirectoryOnly);
            childs += files.Length;
            foreach(FileInfo fi in files)
            {
                ToolStripMenuItem fileItem = (ToolStripMenuItem)parentMenuItem.DropDownItems.Add(fi.Name);
                fileItem.Tag = fi.FullName;
                fileItem.Click += kernelsMenuSubItem_Click;
            }

            return childs;
        }

        private AppStatus Status
        {
            set
            {
                switch(value)
                {
                    case AppStatus.Polishing:
                        statusLabel.Text = "Polishing...";
                        statusLabel.BackColor = Color.Yellow;
                        break;
                    case AppStatus.Loading:
                        statusLabel.Text = "Loading...";
                        statusLabel.BackColor = SystemColors.Control;
                        break;
                    case AppStatus.Ready:
                        statusLabel.Text = "Ready";
                        statusLabel.BackColor = SystemColors.Control;
                        break;
                    case AppStatus.Rendering:
                        statusLabel.Text = "Rendering";
                        statusLabel.BackColor = Color.LightGreen;
                        break;
                }
            } // set
        }

        private string KernelFilename
        {
            set
            {
                kernelFilename = value;
                string filename = new FileInfo(kernelFilename).Name;
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
            rp.isGrayscale = rp.IsGrayscale;
            rp.width = (int)rp.Width;
            rp.height = (int)rp.Height;
            rp.iterMax = rp.IterationsMax;
            rp.iterMin = rp.IterationsMin;
            rp.escapeOrbit = rp.EscapeOrbit;

            rp.reMin = rp.ReMin;
            rp.reMax = rp.ReMax;
            rp.imMin = rp.ImMin;
            rp.imMax = rp.ImMax;

            rp.workers = rp.Workers;

            rp.RecalculateColors();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            TransferBBrotParameters();
            propertyGrid.Refresh();

            try
            {
                string kernelSource = File.ReadAllText(kernelFilename);

                render = new Render(cPlatform, kernelSource, rp);

                render.BuildKernels();
                render.AllocateBuffers();
                render.ConfigureKernel();
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

            propertyGrid.Enabled = openKernelMenuItem.Enabled = fullViewMenuItem.Enabled = fullViewButton.Enabled = kernelsMenuItem.Enabled = platformMenuItem.Enabled = startButton.Enabled = startMenuItem.Enabled = false;
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
            backBitmap = new Bitmap(rp.width, rp.height, g);
            drawPanel.Width = rp.width;
            drawPanel.Height = rp.height;
            frontBitmap = new Bitmap(backBitmap);

            cts = new CancellationTokenSource();

            SynchronizationContext ui = SynchronizationContext.Current;

            thGenerator = new Thread(() => Thread_Generator(ui, cts.Token));
            thGenerator.IsBackground = false;

            thPainter = new Thread(() => Thread_Painter(ui, cts.Token));
            thPainter.IsBackground = true;

            isRunning = true;
            hpet.Restart();
            thGenerator.Start();
            thPainter.Start();

            Status = AppStatus.Rendering;
        }

        private void Thread_Painter(SynchronizationContext ui, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                long hpet_ubbb_st = hpet.ElapsedMilliseconds;
                if (__should_update && ui != null)
                {
                    if (autoRefresh)
                        UpdateBackBuffer();

                    if (token.IsCancellationRequested)
                        return;

                    ui.Send(
                        (object o) => {
                            if (hpet_ktime > 0)
                            {
                                long ktime = hpet_ktime / TimeSpan.TicksPerMillisecond;
                                if (ktime >= 1000)
                                    kernelTimeStatusLabel.Text = $"Core: {(ktime / 1000f).ToString("0.00")}s {hpet_count}*";
                                else
                                    kernelTimeStatusLabel.Text = $"Core: {ktime}ms {hpet_count}*";
                            }

                            hpet_count = 0;

                            memoryStatusLabel.Text = $"Memory: {(Process.GetCurrentProcess().PrivateMemorySize64 / 1024.0 / 1024.0).ToString("0.00")}Mb";

                            TimeSpan ts = TimeSpan.FromMilliseconds(hpet.ElapsedMilliseconds);
                            renderTimeStatusLabel.Text = $"Rendering: {ts.ToString(@"dd\ hh\:mm\:ss")}";

                            if (autoRefresh)
                                drawPanel.Invalidate();
                        }, null); // ui.send

                    if (hpet.ElapsedMilliseconds - hpet_ubbb_st < THREAD_PAINT_SLEEP_INTERVAL)
                        Thread.Sleep(THREAD_PAINT_SLEEP_INTERVAL);

                    __should_update = false;

                    if (token.IsCancellationRequested)
                        return;
                } // if
            } // while
        }

        long hpet_ktime = 0;
        long hpet_start = 0;
        long hpet_count = 0;
        private void Thread_Generator(SynchronizationContext ui, CancellationToken token)
        {
            hpet_ktime = 0;
            hpet_count = 0;
            while (!token.IsCancellationRequested)
            {
                hpet_ktime = Math.Max(hpet_ktime, hpet.ElapsedTicks - hpet_start);
                hpet_start = hpet.ElapsedTicks;

                render.ExecuteKernel();
                if (token.IsCancellationRequested) break;

                if (__should_update)
                    render.FinishKernel();
                else
                {
                    render.ReadResult();
                    if (token.IsCancellationRequested) break;
                    __should_update = true;
                }

                hpet_count++;
            }
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            Status = AppStatus.Polishing;
            filterGrid.Enabled = stopButton.Enabled = stopMenuItem.Enabled = false;

            await Task.Run(() => { while (thPainter.IsAlive) Thread.Sleep(0); });

            UpdateBackBuffer();

            isRunning = false;
            hpet.Stop();
            filterGrid.Enabled = propertyGrid.Enabled = openKernelMenuItem.Enabled = fullViewMenuItem.Enabled = fullViewButton.Enabled = kernelsMenuItem.Enabled = platformMenuItem.Enabled = startButton.Enabled = startMenuItem.Enabled = true;
            Status = AppStatus.Ready;
        }

        private float Fx(float x, float factor = 1.0f)
        {
            switch (fp.Type)
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
            if (backBitmap != null)
                UpdateBackBuffer(new Rectangle(0, 0, backBitmap.Width, backBitmap.Height));
        }

        private void UpdateBackBuffer(Rectangle region)
        {
            if (backBitmap == null || region.Width == 0 || region.Height == 0)
                return;

            float maxR = float.MinValue;
            float maxG = float.MinValue;
            float maxB = float.MinValue;

            Vector4[] h_resBuf = render.h_resultBuf;
            int l = h_resBuf.Length;

            if (rp.isGrayscale)
                for (int i = 0; i < l; i++)
                    maxR = Math.Max(h_resBuf[i].x, maxR);
            else
                for (int i = 0; i < l; i++)
                {
                    maxR = Math.Max(h_resBuf[i].x, maxR);
                    maxG = Math.Max(h_resBuf[i].y, maxG);
                    maxB = Math.Max(h_resBuf[i].z, maxB);
                }

            BitmapData bitmapData = backBitmap.LockBits(
                region,
                ImageLockMode.WriteOnly,
                backBitmap.PixelFormat);

            float scaleR = 255f * fp.Exposure / Fx(maxR, fp.Factor);
            float scaleG = rp.isGrayscale ? 0f : 255f * fp.Exposure / Fx(maxG, fp.Factor);
            float scaleB = rp.isGrayscale ? 0f : 255f * fp.Exposure / Fx(maxB, fp.Factor);

            int stride = bitmapData.Stride;
            IntPtr Scan0 = bitmapData.Scan0;
            unsafe
            {
                uint* p = (uint*)(void*)Scan0;
                uint argb = 0;

                for (int y = 0; y < region.Height; y++)
                {
                    int ryw = (y + region.Y) * rp.width;
                    for (int x = 0; x < region.Width; x++)
                    {
                        int i = x + region.X + ryw;

                        if (rp.isGrayscale)
                        {
                            argb = ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor));
                            argb |= argb << 8;
                            argb |= argb << 8;
                            argb |= 0xFF000000;
                        }
                        else
                        {
                            switch (fp.Tint)
                            {
                                case Tint.RGB:
                                    argb =
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor)) << 16) |
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor)) << 8) |
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor))) |
                                        0xFF000000;
                                    break;
                                case Tint.BGR:
                                    argb =
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor)) << 16) |
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor)) << 8) |
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor))) |
                                        0xFF000000;
                                    break;
                                case Tint.GRB:
                                    argb =
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor)) << 16) |
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor)) << 8) |
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor))) |
                                        0xFF000000;
                                    break;
                                case Tint.GBR:
                                    argb =
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor)) << 16) |
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor)) << 8) |
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor))) |
                                        0xFF000000;
                                    break;
                                case Tint.BRG:
                                    argb =
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor)) << 16) |
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor)) << 8) |
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor))) |
                                        0xFF000000;
                                    break;
                                case Tint.RBG:
                                    argb =
                                        (ByteClamp(scaleR * Fx(h_resBuf[i].x, fp.Factor)) << 16) |
                                        (ByteClamp(scaleB * Fx(h_resBuf[i].z, fp.Factor)) << 8) |
                                        (ByteClamp(scaleG * Fx(h_resBuf[i].y, fp.Factor))) |
                                        0xFF000000;
                                    break;
                            }
                        }

                        p[i] = argb;
                    } // for x
                } // for y
            }

            backBitmap.UnlockBits(bitmapData);

            lock (__frontLocker)
            {
                Graphics g = Graphics.FromImage(frontBitmap);
                g.DrawImageUnscaled(backBitmap, 0, 0);
            }
        }

        private uint ByteClamp(float value)
        {
            if (value > 255f)
                value = 255f;
            else
                if (value < 0f)
                    value = 0f;

            return (uint)value;
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
            Rectangle rect = GetCursorRect(dragCurrent);

            g.FillRectangle(
                dimBrush,
                rect
                );

            int cw = rect.Width / CURSOR_DIVW;
            int ch = rect.Height / CURSOR_DIVH;

            g.DrawLine(Pens.White,
                dragStart.X - cw,
                dragStart.Y,
                dragStart.X + cw,
                dragStart.Y
                );

            g.DrawLine(Pens.White,
                dragStart.X,
                dragStart.Y - ch,
                dragStart.X,
                dragStart.Y + ch
                );
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts?.Cancel();
        }

        private void saveAsImageButton_Click(object sender, EventArgs e)
        {
            bool uc = autoRefresh;
            autoRefresh = false;

            if (!isRunning)
            {
                UpdateBackBuffer();
                drawPanel.Invalidate();
            }

            if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                backBitmap.Save(saveImageFileDialog.FileName);

            autoRefresh = uc;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            lock (__frontLocker)
                if (frontBitmap != null)
                    e.Graphics.DrawImageUnscaled(frontBitmap, 0, 0);
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (render == null)
                return;

            string xstr = ToRe(e.X).ToString("0.00000000").Replace(",", ".");
            string ystr = ToIm(e.Y).ToString("0.00000000").Replace(",", ".");
            coordStatusLabel.Text = $"[Re; Im]= {xstr}; {ystr}";

            if (isDrag && !isRunning)
            {
                Rectangle r = GetCursorRect(dragCurrent);
                if ((r.Width > 0) && (r.Height > 0))
                {
                    lock (__frontLocker)
                        Graphics.FromImage(frontBitmap).DrawImage(backBitmap, r, r, GraphicsUnit.Pixel);
                }

                dragCurrent = e.Location;

                DrawCursor();
                drawPanel.Invalidate();
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
        Point dragCurrent;
        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (backBitmap == null)
                return;

            if (!isRunning)
            {
                Rectangle r = GetCursorRect(dragStop);
                lock (__frontLocker)
                    Graphics.FromImage(frontBitmap).DrawImage(backBitmap, r, r, GraphicsUnit.Pixel);

                dragStart = e.Location;
                dragStop = dragCurrent = dragStart;
                isDrag = true;
            }
        }

        private float ToRe(int x)
        {
            float dx = x / (float)rp.width;
            return rp.reMin + (Math.Abs(rp.reMax - rp.reMin)) * dx;
        }

        private float ToIm(int y)
        {
            float dy = y / (float)rp.height;
            return rp.imMax - (Math.Abs(rp.imMax - rp.imMin)) * dy;
        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag && !isRunning)
            {
                dragStop = e.Location;

                int xl = Math.Abs(dragStart.X - dragStop.X);
                int yl = Math.Abs(dragStart.Y - dragStop.Y);
                int maxl = Math.Max(xl, yl);

                if (maxl > 0)
                {
                    rp.ReMin = ToRe(dragStart.X - maxl);
                    rp.ReMax = ToRe(dragStart.X + maxl);
                    rp.ImMin = ToIm(dragStart.Y + maxl);
                    rp.ImMax = ToIm(dragStart.Y - maxl);

                    propertyGrid.Refresh();

                    drawPanel.Invalidate();
                }
            }

            isDrag = false;
        }

        private void fullViewButton_Click(object sender, EventArgs e)
        {
            rp.ReMin = -2f;
            rp.ReMax = 2f;
            rp.ImMin = -2f;
            rp.ImMax = 2f;
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
            propertyGrid.Refresh();
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
            SetLabelColumnWidth(filterGrid, propertyGrid.Width / 2);
            autoRefreshMenuItem.Checked = autoRefreshButton.Checked = autoRefresh;
            Status = AppStatus.Ready;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoupdateMenuItem_Click(object sender, EventArgs e)
        {
            autoRefresh = !autoRefresh;
            autoRefreshMenuItem.Checked = autoRefreshButton.Checked = autoRefresh;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, $"{AppFullName}\n\n(c) Nikolai Voronin 2011-2016\nUnder the MIT License (MIT)\n\nhttps://github.com/nikvoronin/BuddhabrotCL", "About");
        }

        private void goGithubMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/nikvoronin/BuddhabrotCL");
        }

        private async void filterGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (!isRunning)
                switch (e.ChangedItem.Label)
                {
                    case "Type":
                    case "Factor":
                    case "Exposure":
                    case "Tint":
                        Status = AppStatus.Polishing;
                        filterGrid.Enabled = propertyGrid.Enabled = startMenuItem.Enabled = startButton.Enabled = false;
                        await Task.Run(() => { UpdateBackBuffer(); });
                        drawPanel.Invalidate();
                        filterGrid.Enabled = propertyGrid.Enabled = startMenuItem.Enabled = startButton.Enabled = true;
                        Status = AppStatus.Ready;
                        break;
                }
        }

        public new void Dispose()
        {
            Dispose(true);
            Dispose2(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose2(bool disposing)
        {
            if (disposing)
            {
                dimBrush.Dispose();
                if (render != null)
                    render.Dispose();
            }
        }
    }
}
