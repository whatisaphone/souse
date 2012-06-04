using System;
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
		public const double AudioSensitivity = 0.1;

		public static AudioMaster audioMaster;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
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
			MouseButtons btn = ConsonantToButton(e.OldConsonant);
			if (btn != MouseButtons.None)
				InputInjector.MouseEvent(btn, false);

			btn = ConsonantToButton(e.NewConsonant);
			if (btn != MouseButtons.None)
				InputInjector.MouseEvent(btn, true);
		}

		static MouseButtons ConsonantToButton(Consonant consonant)
		{
			switch (consonant)
			{
				case Consonant.T:
					return MouseButtons.Left;
				case Consonant.K:
					return MouseButtons.Right;
				case Consonant.S:
					return MouseButtons.Middle;
				default:
					return MouseButtons.None;
			}
		}
	}
}
