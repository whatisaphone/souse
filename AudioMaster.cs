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
		bool running;
		IAudioSource waveIn;
		Consonant curConsonant;

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
			if (running)
				return;
			waveIn.Start();
			running = true;
		}

		public void Stop()
		{
			if (!running)
				return;
			waveIn.Stop();
			running = false;
		}

		public void GotAudioData(object sender, AudioDataEventArgs e)
		{
			var cons = AudioAnalyzer.DetermineConsonant(e.Data);
			if (cons != curConsonant)
			{
				ConsonantChanged(this, new ConsonantChangedEventArgs(curConsonant, cons));
				curConsonant = cons;
			}
		}
	}
}
