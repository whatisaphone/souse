using System;
using System.Windows.Forms;

namespace MouseAhead
{
	public partial class MainForm : Form
	{
		NotifyIcon notifyIcon;

		public MainForm()
		{
			InitializeComponent();
			notifyIcon = new NotifyIcon();
			notifyIcon.Text = "MouseAhead";
			notifyIcon.Icon = this.Icon;
			notifyIcon.ContextMenuStrip = mnuNotifyIcon;
			notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
		}

		private void mnuShow_Click(object sender, EventArgs e)
		{
			ShowMe();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
				HideMe();
		}

		private void NotifyIcon_DoubleClick(object sender, EventArgs e)
		{
			ShowMe();
		}

		private void ShowMe()
		{
			notifyIcon.Visible = false;
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
		}

		private void HideMe()
		{
			Hide();
			notifyIcon.Visible = true;
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			notifyIcon.Visible = false;
		}
	}
}
