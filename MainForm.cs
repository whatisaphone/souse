using Souse.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Souse {
    public partial class MainForm : Form {
        private readonly NotifyIcon notifyIcon;

        public MainForm() {
            InitializeComponent();

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "Souse";
            notifyIcon.Icon = GetNotifyIconIcon();
            notifyIcon.ContextMenuStrip = mnuNotifyIcon;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

            App.audioMaster.EnabledChanged += audioMaster_EnabledChanged;
        }

        private void ShowMe() {
            notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void HideMe() {
            Hide();
            notifyIcon.Visible = true;
        }

        private void audioMaster_EnabledChanged(object sender, EventArgs e) {
            notifyIcon.Icon = GetNotifyIconIcon();
        }

        private Icon GetNotifyIconIcon() {
            return App.audioMaster.Enabled ? Resources.StartedIcon : Resources.StoppedIcon;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            notifyIcon.Visible = false;
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            if (WindowState == FormWindowState.Minimized)
                HideMe();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (App.audioMaster.Enabled)
                    App.audioMaster.Enabled = false;
                else
                    App.audioMaster.Enabled = true;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ShowMe();
            }
        }

        private void mnuTrayStart_Click(object sender, EventArgs e) {
            App.audioMaster.Enabled = true;
        }

        private void mnuTrayStop_Click(object sender, EventArgs e) {
            App.audioMaster.Enabled = false;
        }

        private void mnuTrayShow_Click(object sender, EventArgs e) {
            ShowMe();
        }

        private void mnuTrayExit_Click(object sender, EventArgs e) {
            Close();
        }

        private void mnuTrayKeyCheck_Click(object sender, EventArgs e) {
            MessageBox.Show(InputState.WhichNonModKeyIsDown().ToString());
        }

        private void mnuReloadConfig_Click(object sender, EventArgs e) {
            App.config.Reload();
        }
    }
}
