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
		public const int AudioBufferSize = 6144;
		public const int AudioBufferCount = 4;
		public const double AudioHighPassFreq = 1000;
		public const double AudioLowPassFreq = 20000;
		public const double AudioTotalSensitivity = 0.05;
		public const double AudioBucketSensitivity = 0.15;
		public const double A880 = 880.0;  // frequency your mic hears for 880Hz input

		public static AudioMaster audioMaster;

		private static MouseButtons lastButton;
		private static Tuple<Keys, Keys> lastKeys;

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
