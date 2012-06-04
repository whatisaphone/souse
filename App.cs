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
			switch (e.NewConsonant)
			{
				case Consonant.T:
					InputInjector.MouseEvent(MouseButtons.Left, true);
					break;
				case Consonant.K:
					InputInjector.MouseEvent(MouseButtons.Right, true);
					break;
				case Consonant.S:
					InputInjector.MouseEvent(MouseButtons.Middle, true);
					break;
				case Consonant.None:
					switch (e.OldConsonant)
					{
						case Consonant.T:
							InputInjector.MouseEvent(MouseButtons.Left, false);
							break;
						case Consonant.K:
							InputInjector.MouseEvent(MouseButtons.Right, false);
							break;
						case Consonant.S:
							InputInjector.MouseEvent(MouseButtons.Middle, false);
							break;
					}
					break;
			}
		}
	}
}
