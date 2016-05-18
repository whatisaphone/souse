using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Souse
{
    internal static class App
	{
        public static Config config;
		public static AudioMaster audioMaster;

		private static MouseButtons lastButton;
		private static Tuple<Keys, Keys> lastKeys;
        private static RPCServer rpcServer;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        private static void Main() {
		    App.config = Config.Read();

			// in general, input devices should try to stay super responsive
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			audioMaster = new AudioMaster();
			audioMaster.ConsonantChanged += ConsonantChanged;
			audioMaster.Start();

		    if (App.config.RPCBindPrefix != null) {
		        rpcServer = new RPCServer(App.config.RPCBindPrefix);
		        rpcServer.Start();
		    }

		    try
			{
				Application.Run(new MainForm());
			}
			finally
			{
                SetKillTimeout();
                if (rpcServer != null)
                    rpcServer.Stop();
				audioMaster.Dispose();
			}
		}

        private static void SetKillTimeout() {
            // The audio code which I found somewhere on the internet has a threading
            // bug that I don't feel like debugging which causes it to deadlock on
            // dispose on newer windows versions. Here's the workaround
            var thread = new Thread(() => {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Process.GetCurrentProcess().Kill();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private static void ConsonantChanged(object sender, ConsonantChangedEventArgs e)
		{
			Debug.Assert(e.OldConsonant == Consonant.None || e.NewConsonant == Consonant.None);

			var curKeyDown = InputState.WhichNonModKeyIsDown();

			// mouse

			if (e.NewConsonant == Consonant.None && lastButton != MouseButtons.None)
				InputInjector.MouseEvent(lastButton, false);

			lastButton = curKeyDown == 0 ? ConsonantToButton(e.NewConsonant) : MouseButtons.None;
			if (lastButton != MouseButtons.None)
				InputInjector.MouseEvent(lastButton, true);

			// keyboard

			if (e.NewConsonant == Consonant.None && lastKeys._2 != Keys.None)
			{
				InputInjector.KeyEvent(lastKeys._2, false);
				InputInjector.KeyEvent(lastKeys._1, false);
			}

			lastKeys = curKeyDown == 0 ? ConsonantToKeystroke(e.NewConsonant) : Tuple.Make(Keys.None, Keys.None);
			if (lastKeys._2 != Keys.None)
			{
				InputInjector.KeyEvent(lastKeys._1, true);
				InputInjector.KeyEvent(lastKeys._2, true);
			}
		}

        private static MouseButtons ConsonantToButton(Consonant consonant)
		{
			switch (consonant)
			{
				case Consonant.T:
					return MouseButtons.Left;
				case Consonant.K:
					return MouseButtons.Middle;
				case Consonant.S:
					return MouseButtons.Right;
				default:
					return MouseButtons.None;
			}
		}

		private static Tuple<Keys, Keys> ConsonantToKeystroke(Consonant consonant)
		{
			switch (consonant)
			{
				case Consonant.C:
					return Tuple.Make(Keys.None, Keys.PageDown);
				case Consonant.Cs:
					return Tuple.Make(Keys.Control | Keys.Shift, Keys.Tab);
				case Consonant.D:
					return Tuple.Make(Keys.None, Keys.PageUp);
				case Consonant.Ds:
					return Tuple.Make(Keys.Control, Keys.Tab);
				case Consonant.E:
					return Tuple.Make(Keys.Control, Keys.Subtract);
				case Consonant.Fs:
					return Tuple.Make(Keys.Control, Keys.Add);
				case Consonant.G:
					return Tuple.Make(Keys.Control, Keys.F4);
				case Consonant.HighA:
					return Tuple.Make(Keys.Control | Keys.Shift, Keys.T);
				case Consonant.HighB:
					return Tuple.Make(Keys.Alt, Keys.F4);
				default:
					return Tuple.Make(Keys.None, Keys.None);
			}
		}
	}
}
