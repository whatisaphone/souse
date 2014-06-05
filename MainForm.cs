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
			notifyIcon.Icon = App.audioMaster.Enabled ? Properties.Resources.StartedIcon : Properties.Resources.StoppedIcon;
			notifyIcon.ContextMenuStrip = mnuNotifyIcon;
			notifyIcon.MouseClick += NotifyIcon_MouseClick;
			notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

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

		void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                if (App.audioMaster.Enabled)
                    App.audioMaster.Enabled = false;
                else
                    App.audioMaster.Enabled = true;
			}
		}

		void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ShowMe();
			}
		}

		void mnuTrayStart_Click(object sender, EventArgs e)
		{
            App.audioMaster.Enabled = true;
		}

		void mnuTrayStop_Click(object sender, EventArgs e)
		{
            App.audioMaster.Enabled = false;
		}

		void mnuTrayShow_Click(object sender, EventArgs e)
		{
			ShowMe();
		}

		void mnuTrayExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void mnuTrayKeyCheck_Click(object sender, EventArgs e)
		{
			MessageBox.Show(InputState.WhichNonModKeyIsDown().ToString());
		}
	}
}
