namespace BuddhabrotCL
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.drawPanel = new BuddhabrotCL.CanvasPanel();
            this.filterGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveAsImageButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.fullViewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.autoRefreshButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openKernelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.autoRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.platformMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kernelsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goGithubMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.platformStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.coordStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.memoryStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.kernelTimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.renderTimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveImageFileDialog
            // 
            this.saveImageFileDialog.DefaultExt = "png";
            this.saveImageFileDialog.Filter = "PNG (*.png)|*.png";
            this.saveImageFileDialog.RestoreDirectory = true;
            this.saveImageFileDialog.SupportMultiDottedExtensions = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(0, 127);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(216, 480);
            this.propertyGrid.TabIndex = 17;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.ViewBorderColor = System.Drawing.SystemColors.Window;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 28);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.Controls.Add(this.drawPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.filterGrid);
            this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer.Size = new System.Drawing.Size(984, 607);
            this.splitContainer.SplitterDistance = 764;
            this.splitContainer.TabIndex = 19;
            // 
            // drawPanel
            // 
            this.drawPanel.Location = new System.Drawing.Point(0, 0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(200, 100);
            this.drawPanel.TabIndex = 0;
            this.drawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawPanel_Paint);
            this.drawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawPanel_MouseDown);
            this.drawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawPanel_MouseMove);
            this.drawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawPanel_MouseUp);
            // 
            // filterGrid
            // 
            this.filterGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterGrid.HelpVisible = false;
            this.filterGrid.Location = new System.Drawing.Point(0, 28);
            this.filterGrid.Name = "filterGrid";
            this.filterGrid.Size = new System.Drawing.Size(216, 100);
            this.filterGrid.TabIndex = 19;
            this.filterGrid.ToolbarVisible = false;
            this.filterGrid.ViewBorderColor = System.Drawing.SystemColors.Window;
            this.filterGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.filterGrid_PropertyValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsImageButton,
            this.toolStripSeparator1,
            this.startButton,
            this.stopButton,
            this.toolStripSeparator3,
            this.fullViewButton,
            this.toolStripSeparator8,
            this.autoRefreshButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(216, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveAsImageButton
            // 
            this.saveAsImageButton.BackColor = System.Drawing.SystemColors.Control;
            this.saveAsImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveAsImageButton.Image = global::BuddhabrotCL.Properties.Resources.disks;
            this.saveAsImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsImageButton.Name = "saveAsImageButton";
            this.saveAsImageButton.Size = new System.Drawing.Size(23, 22);
            this.saveAsImageButton.Text = "Save Image As...";
            this.saveAsImageButton.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.Control;
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.startButton.Image = global::BuddhabrotCL.Properties.Resources.control;
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(23, 22);
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.SystemColors.Control;
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(23, 22);
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // fullViewButton
            // 
            this.fullViewButton.BackColor = System.Drawing.SystemColors.Control;
            this.fullViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fullViewButton.Image = global::BuddhabrotCL.Properties.Resources.picture;
            this.fullViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fullViewButton.Name = "fullViewButton";
            this.fullViewButton.Size = new System.Drawing.Size(23, 22);
            this.fullViewButton.Text = "Full view";
            this.fullViewButton.Click += new System.EventHandler(this.fullViewButton_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // autoRefreshButton
            // 
            this.autoRefreshButton.Checked = true;
            this.autoRefreshButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.autoRefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("autoRefreshButton.Image")));
            this.autoRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoRefreshButton.Name = "autoRefreshButton";
            this.autoRefreshButton.Size = new System.Drawing.Size(23, 22);
            this.autoRefreshButton.Text = "Auto Refresh";
            this.autoRefreshButton.Click += new System.EventHandler(this.autoupdateMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.c";
            this.openFileDialog.Filter = "OpenCL C-files (*.c)|*.c|All files (*.*)|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.renderToolStripMenuItem,
            this.kernelsMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainMenu.Size = new System.Drawing.Size(984, 28);
            this.mainMenu.TabIndex = 20;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openKernelMenuItem,
            this.toolStripSeparator2,
            this.saveImageAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openKernelMenuItem
            // 
            this.openKernelMenuItem.Name = "openKernelMenuItem";
            this.openKernelMenuItem.Size = new System.Drawing.Size(184, 24);
            this.openKernelMenuItem.Text = "&Open Kernel...";
            this.openKernelMenuItem.Click += new System.EventHandler(this.kernelButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // saveImageAsToolStripMenuItem
            // 
            this.saveImageAsToolStripMenuItem.Image = global::BuddhabrotCL.Properties.Resources.disks;
            this.saveImageAsToolStripMenuItem.Name = "saveImageAsToolStripMenuItem";
            this.saveImageAsToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.saveImageAsToolStripMenuItem.Text = "Save &Image As...";
            this.saveImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(181, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullViewMenuItem,
            this.toolStripSeparator7,
            this.autoRefreshMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // fullViewMenuItem
            // 
            this.fullViewMenuItem.Image = global::BuddhabrotCL.Properties.Resources.picture;
            this.fullViewMenuItem.Name = "fullViewMenuItem";
            this.fullViewMenuItem.Size = new System.Drawing.Size(163, 24);
            this.fullViewMenuItem.Text = "&Full View";
            this.fullViewMenuItem.Click += new System.EventHandler(this.fullViewButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(160, 6);
            // 
            // autoRefreshMenuItem
            // 
            this.autoRefreshMenuItem.Checked = true;
            this.autoRefreshMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoRefreshMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("autoRefreshMenuItem.Image")));
            this.autoRefreshMenuItem.Name = "autoRefreshMenuItem";
            this.autoRefreshMenuItem.Size = new System.Drawing.Size(163, 24);
            this.autoRefreshMenuItem.Text = "&Auto Refresh";
            this.autoRefreshMenuItem.Click += new System.EventHandler(this.autoupdateMenuItem_Click);
            // 
            // renderToolStripMenuItem
            // 
            this.renderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMenuItem,
            this.stopMenuItem,
            this.toolStripSeparator6,
            this.platformMenuItem});
            this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
            this.renderToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.renderToolStripMenuItem.Text = "&Render";
            // 
            // startMenuItem
            // 
            this.startMenuItem.Image = global::BuddhabrotCL.Properties.Resources.control;
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.Size = new System.Drawing.Size(135, 24);
            this.startMenuItem.Text = "&Start";
            this.startMenuItem.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("stopMenuItem.Image")));
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Size = new System.Drawing.Size(135, 24);
            this.stopMenuItem.Text = "St&op";
            this.stopMenuItem.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(132, 6);
            // 
            // platformMenuItem
            // 
            this.platformMenuItem.Name = "platformMenuItem";
            this.platformMenuItem.Size = new System.Drawing.Size(135, 24);
            this.platformMenuItem.Text = "&Platform";
            // 
            // kernelsMenuItem
            // 
            this.kernelsMenuItem.Name = "kernelsMenuItem";
            this.kernelsMenuItem.Size = new System.Drawing.Size(63, 24);
            this.kernelsMenuItem.Text = "&Kernel";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goGithubMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // goGithubMenuItem
            // 
            this.goGithubMenuItem.Name = "goGithubMenuItem";
            this.goGithubMenuItem.Size = new System.Drawing.Size(204, 24);
            this.goGithubMenuItem.Text = "&GitHub Project Site";
            this.goGithubMenuItem.Click += new System.EventHandler(this.goGithubMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(201, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(72, 20);
            this.statusLabel.Text = "Loading...";
            // 
            // platformStatusLabel
            // 
            this.platformStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.platformStatusLabel.Name = "platformStatusLabel";
            this.platformStatusLabel.Size = new System.Drawing.Size(61, 20);
            this.platformStatusLabel.Text = "OpenCL";
            // 
            // coordStatusLabel
            // 
            this.coordStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.coordStatusLabel.Name = "coordStatusLabel";
            this.coordStatusLabel.Size = new System.Drawing.Size(457, 20);
            this.coordStatusLabel.Spring = true;
            this.coordStatusLabel.Text = "[Re; Im] = -; -";
            this.coordStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // memoryStatusLabel
            // 
            this.memoryStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.memoryStatusLabel.Name = "memoryStatusLabel";
            this.memoryStatusLabel.Size = new System.Drawing.Size(99, 20);
            this.memoryStatusLabel.Text = "Memory: -Mb";
            // 
            // kernelTimeStatusLabel
            // 
            this.kernelTimeStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.kernelTimeStatusLabel.Name = "kernelTimeStatusLabel";
            this.kernelTimeStatusLabel.Size = new System.Drawing.Size(88, 20);
            this.kernelTimeStatusLabel.Text = "Core: -ms -*";
            this.kernelTimeStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // renderTimeStatusLabel
            // 
            this.renderTimeStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.renderTimeStatusLabel.Name = "renderTimeStatusLabel";
            this.renderTimeStatusLabel.Size = new System.Drawing.Size(142, 20);
            this.renderTimeStatusLabel.Text = "Rendering: -- --:--:--";
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.platformStatusLabel,
            this.coordStatusLabel,
            this.memoryStatusLabel,
            this.kernelTimeStatusLabel,
            this.renderTimeStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 635);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(984, 25);
            this.mainStatusStrip.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 660);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "BuddhabrotCL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog;
        private CanvasPanel drawPanel;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripButton saveAsImageButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripButton fullViewButton;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kernelsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goGithubMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem platformMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openKernelMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton autoRefreshButton;
        private System.Windows.Forms.ToolStripMenuItem autoRefreshMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.PropertyGrid filterGrid;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel platformStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel coordStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel memoryStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel kernelTimeStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel renderTimeStatusLabel;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
    }
}

