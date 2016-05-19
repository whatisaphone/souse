namespace Souse
{
	partial class MainForm
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
            this.mnuNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuTrayStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrayStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTraySep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuReloadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrayKeyCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrayShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTrayExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuNotifyIcon
            // 
            this.mnuNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTrayStart,
            this.mnuTrayStop,
            this.mnuTraySep1,
            this.mnuReloadConfig,
            this.mnuTrayKeyCheck,
            this.mnuTrayShow,
            this.mnuTrayExit});
            this.mnuNotifyIcon.Name = "mnuNotifyIcon";
            this.mnuNotifyIcon.Size = new System.Drawing.Size(153, 164);
            // 
            // mnuTrayStart
            // 
            this.mnuTrayStart.Name = "mnuTrayStart";
            this.mnuTrayStart.Size = new System.Drawing.Size(152, 22);
            this.mnuTrayStart.Text = "&Start";
            this.mnuTrayStart.Click += new System.EventHandler(this.mnuTrayStart_Click);
            // 
            // mnuTrayStop
            // 
            this.mnuTrayStop.Name = "mnuTrayStop";
            this.mnuTrayStop.Size = new System.Drawing.Size(152, 22);
            this.mnuTrayStop.Text = "S&top";
            this.mnuTrayStop.Click += new System.EventHandler(this.mnuTrayStop_Click);
            // 
            // mnuTraySep1
            // 
            this.mnuTraySep1.Name = "mnuTraySep1";
            this.mnuTraySep1.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuReloadConfig
            // 
            this.mnuReloadConfig.Name = "mnuReloadConfig";
            this.mnuReloadConfig.Size = new System.Drawing.Size(152, 22);
            this.mnuReloadConfig.Text = "&Reload config";
            this.mnuReloadConfig.Click += new System.EventHandler(this.mnuReloadConfig_Click);
            // 
            // mnuTrayKeyCheck
            // 
            this.mnuTrayKeyCheck.Name = "mnuTrayKeyCheck";
            this.mnuTrayKeyCheck.Size = new System.Drawing.Size(152, 22);
            this.mnuTrayKeyCheck.Text = "&Key check";
            this.mnuTrayKeyCheck.Click += new System.EventHandler(this.mnuTrayKeyCheck_Click);
            // 
            // mnuTrayShow
            // 
            this.mnuTrayShow.Name = "mnuTrayShow";
            this.mnuTrayShow.Size = new System.Drawing.Size(152, 22);
            this.mnuTrayShow.Text = "Sh&ow";
            this.mnuTrayShow.Click += new System.EventHandler(this.mnuTrayShow_Click);
            // 
            // mnuTrayExit
            // 
            this.mnuTrayExit.Name = "mnuTrayExit";
            this.mnuTrayExit.Size = new System.Drawing.Size(152, 22);
            this.mnuTrayExit.Text = "E&xit";
            this.mnuTrayExit.Click += new System.EventHandler(this.mnuTrayExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "MainForm";
            this.Text = "Souse";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.mnuNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip mnuNotifyIcon;
		private System.Windows.Forms.ToolStripMenuItem mnuTrayShow;
		private System.Windows.Forms.ToolStripMenuItem mnuTrayExit;
		private System.Windows.Forms.ToolStripMenuItem mnuTrayStart;
		private System.Windows.Forms.ToolStripMenuItem mnuTrayStop;
		private System.Windows.Forms.ToolStripSeparator mnuTraySep1;
		private System.Windows.Forms.ToolStripMenuItem mnuTrayKeyCheck;
        private System.Windows.Forms.ToolStripMenuItem mnuReloadConfig;
	}
}

