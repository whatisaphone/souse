using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MouseAhead
{
	static class App
	{
		public const int AudioRate = 44100;
		public const int AudioBits = 16;
		public const int AudioChannels = 1;
		public const int AudioBufferSize = 4096;
		public const int AudioBufferCount = 4;
		public const double AudioHighPassFreq = 1000;
		public const double AudioLowPassFreq = 20000;
		public const double AudioTotalSensitivity = 0.25;
		public const double AudioBucketSensitivity = 0.15;

		public static AudioMaster audioMaster;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// in general, input devices should try to stay super responsive
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			audioMaster = new AudioMaster();
			audioMaster.ConsonantChanged += ConsonantChanged;
			audioMaster.Start();

			try
			{
				Application.Run(new MainForm());
			}
			finally
			{
				audioMaster.Dispose();
			}
		}

		static void ConsonantChanged(object sender, ConsonantChangedEventArgs e)
		{
			Debug.Assert(e.OldConsonant == Consonant.None || e.NewConsonant == Consonant.None);

			var curKeyDown = InputState.WhichNonModKeyIsDown();

			MouseButtons btn = ConsonantToButton(e.NewConsonant);
			if (curKeyDown == 0 && btn != MouseButtons.None)
				InputInjector.MouseEvent(btn, true);

			btn = ConsonantToButton(e.OldConsonant);
			if (btn != MouseButtons.None)
				InputInjector.MouseEvent(btn, false);

			var keys = ConsonantToKeystroke(e.NewConsonant);
			if (curKeyDown == 0 && keys._2 != Keys.None)
			{
				InputInjector.KeyEvent(keys._1, true);
				InputInjector.KeyEvent(keys._2, true);
			}

			keys = ConsonantToKeystroke(e.OldConsonant);
			if (keys._2 != Keys.None)
			{
				InputInjector.KeyEvent(keys._2, false);
				InputInjector.KeyEvent(keys._1, false);
			}
		}

		static MouseButtons ConsonantToButton(Consonant consonant)
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
				case Consonant.A:
					return Tuple.Make(Keys.Control, Keys.F4);
				case Consonant.As:
					return Tuple.Make(Keys.Control | Keys.Shift, Keys.T);
				case Consonant.B:
					return Tuple.Make(Keys.Control, Keys.T);
				case Consonant.C:
					return Tuple.Make(Keys.None, Keys.PageDown);
				case Consonant.D:
					return Tuple.Make(Keys.None, Keys.PageUp);
				case Consonant.E:
					return Tuple.Make(Keys.Control | Keys.Shift, Keys.Tab);
				case Consonant.G:
					return Tuple.Make(Keys.Control, Keys.Tab);
				case Consonant.Ahigh:
					return Tuple.Make(Keys.Alt, Keys.F4);
				default:
					return Tuple.Make(Keys.None, Keys.None);
			}
		}
	}
}
