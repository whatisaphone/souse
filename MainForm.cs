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
			notifyIcon.Icon = App.audioMaster.Running ? Properties.Resources.StartedIcon : Properties.Resources.StoppedIcon;
			notifyIcon.ContextMenuStrip = mnuNotifyIcon;
			notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

			App.audioMaster.Started += audioMaster_Started;
			App.audioMaster.Stopped += audioMaster_Stopped;
		}

		void ShowMe()
		{
			notifyIcon.Visible = false;
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
		}

		void HideMe()
		{
			Hide();
			notifyIcon.Visible = true;
		}

		void audioMaster_Started(object sender, EventArgs e)
		{
			notifyIcon.Icon = Properties.Resources.StartedIcon;
		}

		void audioMaster_Stopped(object sender, EventArgs e)
		{
			notifyIcon.Icon = Properties.Resources.StoppedIcon;
		}

		void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			notifyIcon.Visible = false;
		}

		void MainForm_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
				HideMe();
		}

		void NotifyIcon_DoubleClick(object sender, EventArgs e)
		{
			ShowMe();
		}

		void mnuTrayStart_Click(object sender, EventArgs e)
		{
			App.audioMaster.Start();
		}

		void mnuTrayStop_Click(object sender, EventArgs e)
		{
			App.audioMaster.Stop();
		}

		void mnuTrayShow_Click(object sender, EventArgs e)
		{
			ShowMe();
		}

		void mnuTrayExit_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
