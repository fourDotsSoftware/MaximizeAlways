namespace MaximizeAlways
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiMinWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMinAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiMaxAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1W = new System.Windows.Forms.Timer(this.components);
            this.timer2W = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MaximizeAlways";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiMinWindows,
            this.cmiMinAll,
            this.cmiMaxAll,
            this.toolStripMenuItem1,
            this.cmiOptions,
            this.helpToolStripMenuItem,
            this.cmiExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(195, 142);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // cmiMinWindows
            // 
            this.cmiMinWindows.Name = "cmiMinWindows";
            this.cmiMinWindows.Size = new System.Drawing.Size(194, 22);
            this.cmiMinWindows.Text = "&Maximize Applications";
            this.cmiMinWindows.Click += new System.EventHandler(this.cmiMinWindows_Click);
            // 
            // cmiMinAll
            // 
            this.cmiMinAll.Name = "cmiMinAll";
            this.cmiMinAll.Size = new System.Drawing.Size(194, 22);
            this.cmiMinAll.Text = "M&inimize All";
            this.cmiMinAll.Click += new System.EventHandler(this.cmiMinAll_Click);
            // 
            // cmiMaxAll
            // 
            this.cmiMaxAll.Name = "cmiMaxAll";
            this.cmiMaxAll.Size = new System.Drawing.Size(194, 22);
            this.cmiMaxAll.Text = "M&aximize All";
            this.cmiMaxAll.Click += new System.EventHandler(this.cmiMaxAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // cmiOptions
            // 
            this.cmiOptions.Image = global::MaximizeAlways.Properties.Resources.preferences;
            this.cmiOptions.Name = "cmiOptions";
            this.cmiOptions.Size = new System.Drawing.Size(194, 22);
            this.cmiOptions.Text = "&Configure";
            this.cmiOptions.Click += new System.EventHandler(this.cmiOptions_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Image = global::MaximizeAlways.Properties.Resources.help2;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // cmiExit
            // 
            this.cmiExit.Image = global::MaximizeAlways.Properties.Resources.exit;
            this.cmiExit.Name = "cmiExit";
            this.cmiExit.Size = new System.Drawing.Size(194, 22);
            this.cmiExit.Text = "&Exit";
            this.cmiExit.Click += new System.EventHandler(this.cmiExit_Click);
            // 
            // timer1W
            // 
            this.timer1W.Interval = 3000;
            this.timer1W.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2W
            // 
            this.timer2W.Enabled = true;
            this.timer2W.Interval = 5000;
            this.timer2W.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "frmMain";
            this.Text = "Maximize Always";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmiExit;
        private System.Windows.Forms.Timer timer1W;
        private System.Windows.Forms.ToolStripMenuItem cmiMinWindows;
        private System.Windows.Forms.ToolStripMenuItem cmiMinAll;
        private System.Windows.Forms.ToolStripMenuItem cmiMaxAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cmiOptions;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Timer timer2W;
    }
}

