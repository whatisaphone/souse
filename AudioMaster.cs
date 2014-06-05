using System;

namespace MouseAhead
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
        public bool Enabled { get; set; }
		IAudioSource waveIn;
		Consonant curConsonant;

		public event EventHandler<EventArgs> Started;
		public event EventHandler<EventArgs> Stopped;
		public event EventHandler<ConsonantChangedEventArgs> ConsonantChanged;

		public AudioMaster()
		{
            this.Enabled = true;
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
			if (Started != null)
				Started(this, EventArgs.Empty);
		}

		public void Stop()
		{
			if (!Running)
				return;
			waveIn.Stop();
			Running = false;
			if (Stopped != null)
				Stopped(this, EventArgs.Empty);
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
