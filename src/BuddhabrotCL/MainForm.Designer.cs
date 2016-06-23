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
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
            System.Windows.Forms.ToolStripLabel toolStripLabel1;
            System.Windows.Forms.ToolStripLabel toolStripLabel3;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.kernelTimeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.coordStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.drawPanel = new BuddhabrotCL.CanvasPanel();
            this.saveAsImageButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.kernelButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.platformDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.fullViewButton = new System.Windows.Forms.ToolStripButton();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(70, 20);
            toolStripStatusLabel2.Text = "(re; im) =";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(54, 24);
            toolStripLabel1.Text = "Kernel:";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new System.Drawing.Size(69, 24);
            toolStripLabel3.Text = "Platform:";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(88, 20);
            toolStripStatusLabel1.Text = "Kernel time:";
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel1,
            this.kernelTimeStatusLabel,
            toolStripStatusLabel2,
            this.coordStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 635);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(984, 25);
            this.mainStatusStrip.TabIndex = 4;
            // 
            // kernelTimeStatusLabel
            // 
            this.kernelTimeStatusLabel.Name = "kernelTimeStatusLabel";
            this.kernelTimeStatusLabel.Size = new System.Drawing.Size(55, 20);
            this.kernelTimeStatusLabel.Text = "0.00ms";
            // 
            // coordStatusLabel
            // 
            this.coordStatusLabel.Name = "coordStatusLabel";
            this.coordStatusLabel.Size = new System.Drawing.Size(32, 20);
            this.coordStatusLabel.Text = "0; 0";
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
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(216, 608);
            this.propertyGrid.TabIndex = 17;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 27);
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
            this.splitContainer.Size = new System.Drawing.Size(984, 608);
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
            // saveAsImageButton
            // 
            this.saveAsImageButton.BackColor = System.Drawing.SystemColors.Control;
            this.saveAsImageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveAsImageButton.Image = ((System.Drawing.Image)(resources.GetObject("saveAsImageButton.Image")));
            this.saveAsImageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsImageButton.Name = "saveAsImageButton";
            this.saveAsImageButton.Size = new System.Drawing.Size(119, 24);
            this.saveAsImageButton.Text = "Save Image As...";
            this.saveAsImageButton.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(44, 24);
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.SystemColors.Control;
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(44, 24);
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // kernelButton
            // 
            this.kernelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kernelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.kernelButton.Image = ((System.Drawing.Image)(resources.GetObject("kernelButton.Image")));
            this.kernelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.kernelButton.Name = "kernelButton";
            this.kernelButton.Size = new System.Drawing.Size(23, 24);
            this.kernelButton.Text = "...";
            this.kernelButton.Click += new System.EventHandler(this.kernelButton_Click);
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
            this.toolStripSeparator2,
            toolStripLabel1,
            this.kernelButton,
            this.toolStripSeparator3,
            toolStripLabel3,
            this.platformDropDownButton,
            this.fullViewButton,
            this.refreshButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(984, 27);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // platformDropDownButton
            // 
            this.platformDropDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.platformDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.platformDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("platformDropDownButton.Image")));
            this.platformDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.platformDropDownButton.Name = "platformDropDownButton";
            this.platformDropDownButton.Size = new System.Drawing.Size(31, 24);
            this.platformDropDownButton.Text = "...";
            this.platformDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.platformDropDownButton_DropDownItemClicked);
            // 
            // fullViewButton
            // 
            this.fullViewButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.fullViewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.fullViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fullViewButton.Image = ((System.Drawing.Image)(resources.GetObject("fullViewButton.Image")));
            this.fullViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fullViewButton.Name = "fullViewButton";
            this.fullViewButton.Size = new System.Drawing.Size(70, 24);
            this.fullViewButton.Text = "Full view";
            this.fullViewButton.Click += new System.EventHandler(this.fullViewButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(62, 24);
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.c";
            this.openFileDialog.Filter = "OpenCL C-files (*.c)|*.c|All files (*.*)|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 660);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MainForm";
            this.Text = "BuddhabrotCL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton kernelButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton platformDropDownButton;
        private System.Windows.Forms.ToolStripButton fullViewButton;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripStatusLabel kernelTimeStatusLabel;
    }
}

