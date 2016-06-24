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
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.platformStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.kernelStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.coordStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.kernelTimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.renderTimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openKernelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.platformMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kernelsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsImageButton = new System.Windows.Forms.ToolStripButton();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.fullViewButton = new System.Windows.Forms.ToolStripButton();
            this.autoupdateButton = new System.Windows.Forms.ToolStripButton();
            this.saveImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoupdateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.drawPanel = new BuddhabrotCL.CanvasPanel();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.platformStatusLabel,
            this.kernelStatusLabel,
            this.coordStatusLabel,
            this.kernelTimeStatusLabel,
            this.renderTimeStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 635);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(984, 25);
            this.mainStatusStrip.TabIndex = 4;
            // 
            // platformStatusLabel
            // 
            this.platformStatusLabel.Name = "platformStatusLabel";
            this.platformStatusLabel.Size = new System.Drawing.Size(61, 20);
            this.platformStatusLabel.Text = "OpenCL";
            // 
            // kernelStatusLabel
            // 
            this.kernelStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.kernelStatusLabel.Name = "kernelStatusLabel";
            this.kernelStatusLabel.Size = new System.Drawing.Size(125, 20);
            this.kernelStatusLabel.Text = "Kernel: cl_kernel.c";
            // 
            // coordStatusLabel
            // 
            this.coordStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.coordStatusLabel.Name = "coordStatusLabel";
            this.coordStatusLabel.Size = new System.Drawing.Size(471, 20);
            this.coordStatusLabel.Spring = true;
            this.coordStatusLabel.Text = "[Re; Im] = -; -";
            this.coordStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // kernelTimeStatusLabel
            // 
            this.kernelTimeStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.kernelTimeStatusLabel.Name = "kernelTimeStatusLabel";
            this.kernelTimeStatusLabel.Size = new System.Drawing.Size(117, 20);
            this.kernelTimeStatusLabel.Text = "Kernel time: -ms";
            this.kernelTimeStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // renderTimeStatusLabel
            // 
            this.renderTimeStatusLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.renderTimeStatusLabel.Name = "renderTimeStatusLabel";
            this.renderTimeStatusLabel.Size = new System.Drawing.Size(155, 20);
            this.renderTimeStatusLabel.Text = "Render time: -- --:--:--";
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
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 25);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(216, 582);
            this.propertyGrid.TabIndex = 17;
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
            this.splitContainer.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer.Size = new System.Drawing.Size(984, 607);
            this.splitContainer.SplitterDistance = 764;
            this.splitContainer.TabIndex = 19;
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
            this.autoupdateButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(216, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
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
            this.openKernelToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveImageAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openKernelToolStripMenuItem
            // 
            this.openKernelToolStripMenuItem.Name = "openKernelToolStripMenuItem";
            this.openKernelToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.openKernelToolStripMenuItem.Text = "&Open Kernel...";
            this.openKernelToolStripMenuItem.Click += new System.EventHandler(this.kernelButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
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
            this.defaultToolStripMenuItem,
            this.toolStripSeparator7,
            this.autoupdateMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "&View";
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
            this.contextToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contextToolStripMenuItem
            // 
            this.contextToolStripMenuItem.Enabled = false;
            this.contextToolStripMenuItem.Name = "contextToolStripMenuItem";
            this.contextToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.contextToolStripMenuItem.Text = "&Context";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
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
            this.stopButton.Image = global::BuddhabrotCL.Properties.Resources.control_stop_square;
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(23, 22);
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
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
            // autoupdateButton
            // 
            this.autoupdateButton.Checked = true;
            this.autoupdateButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoupdateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.autoupdateButton.Image = global::BuddhabrotCL.Properties.Resources.arrow_circle_double;
            this.autoupdateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoupdateButton.Name = "autoupdateButton";
            this.autoupdateButton.Size = new System.Drawing.Size(23, 22);
            this.autoupdateButton.Text = "Autoupdate";
            this.autoupdateButton.Click += new System.EventHandler(this.autoupdateMenuItem_Click);
            // 
            // saveImageAsToolStripMenuItem
            // 
            this.saveImageAsToolStripMenuItem.Image = global::BuddhabrotCL.Properties.Resources.disks;
            this.saveImageAsToolStripMenuItem.Name = "saveImageAsToolStripMenuItem";
            this.saveImageAsToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.saveImageAsToolStripMenuItem.Text = "Save &Image As...";
            this.saveImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Image = global::BuddhabrotCL.Properties.Resources.picture;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.defaultToolStripMenuItem.Text = "&Full View";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.fullViewButton_Click);
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
            this.stopMenuItem.Image = global::BuddhabrotCL.Properties.Resources.control_stop_square;
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Size = new System.Drawing.Size(135, 24);
            this.stopMenuItem.Text = "St&op";
            this.stopMenuItem.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // autoupdateMenuItem
            // 
            this.autoupdateMenuItem.Checked = true;
            this.autoupdateMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoupdateMenuItem.Image = global::BuddhabrotCL.Properties.Resources.arrow_circle_double;
            this.autoupdateMenuItem.Name = "autoupdateMenuItem";
            this.autoupdateMenuItem.Size = new System.Drawing.Size(157, 24);
            this.autoupdateMenuItem.Text = "&Autoupdate";
            this.autoupdateMenuItem.Click += new System.EventHandler(this.autoupdateMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(154, 6);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
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
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog;
        private CanvasPanel drawPanel;
        private System.Windows.Forms.ToolStripStatusLabel coordStatusLabel;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripButton saveAsImageButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripButton fullViewButton;
        private System.Windows.Forms.ToolStripStatusLabel kernelTimeStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel renderTimeStatusLabel;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kernelsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem platformMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel platformStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel kernelStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem openKernelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton autoupdateButton;
        private System.Windows.Forms.ToolStripMenuItem autoupdateMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    }
}

