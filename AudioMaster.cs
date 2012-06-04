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
		IAudioSource waveIn;
		Consonant curConsonant;

		public event EventHandler<EventArgs> Started;
		public event EventHandler<EventArgs> Stopped;
		public event EventHandler<ConsonantChangedEventArgs> ConsonantChanged;

		public AudioMaster()
		{
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
			var cons = AudioAnalyzer.DetermineConsonant(e.Data);
			if (cons._1 != curConsonant)
			{
				if (ConsonantChanged != null)
					ConsonantChanged(this, new ConsonantChangedEventArgs(curConsonant, cons._1));
				curConsonant = cons._1;
			}
		}
	}
}
