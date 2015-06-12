using System;

namespace Souse
{
	class ConsonantChangedEventArgs : EventArgs
	{
		public Consonant OldConsonant { get; private set; }
		public Consonant NewConsonant { get; private set; }

		public ConsonantChangedEventArgs(Consonant oldConsonant, Consonant newConsonant)
		{
			OldConsonant = oldConsonant;
			NewConsonant = newConsonant;
		}
	}

	class AudioMaster : IDisposable
    {
        public bool Running { get; private set; }
        private bool enabled;
		IAudioSource waveIn;
		Consonant curConsonant;

        public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<ConsonantChangedEventArgs> ConsonantChanged;

		public AudioMaster()
		{
            this.enabled = true;
			waveIn = new WaveInAudioSource();
			waveIn.GotAudio += GotAudioData;
		}

		public void Dispose()
		{
			Stop();
			waveIn.Dispose();
		}

		public void Start()
		{
			if (Running)
				return;
			waveIn.Start();
			Running = true;
            enabled = true;
			if (EnabledChanged != null)
				EnabledChanged(this, EventArgs.Empty);
		}

		public void Stop()
		{
			if (!Running)
				return;
			waveIn.Stop();
			Running = false;
			if (EnabledChanged != null)
				EnabledChanged(this, EventArgs.Empty);
		}

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value && !Running)
                    Start();
                enabled = value;
			    if (EnabledChanged != null)
				    EnabledChanged(this, EventArgs.Empty);
            }
        }

		public void GotAudioData(object sender, AudioDataEventArgs e)
		{
            if (!this.Enabled)
                return;

			var info = AudioAnalyzer.AnalyzeFrame(e.Data);
			if (info.Consonant != curConsonant && info.Consonant == Consonant.None || curConsonant == Consonant.None)
			{
				//Console.WriteLine(info.Consonant.ToString() + " - " + info.LoudestFreq.ToString());
				if (ConsonantChanged != null)
					ConsonantChanged(this, new ConsonantChangedEventArgs(curConsonant, info.Consonant));
				curConsonant = info.Consonant;
			}
		}
	}
}
