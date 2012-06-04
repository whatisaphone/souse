using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WaveLib;

namespace MouseAhead
{
	class AudioDataEventArgs : EventArgs
	{
		public double[] Data { get; private set; }

		public AudioDataEventArgs(double[] data)
		{
			Data = data;
		}
	}

	interface IAudioSource : IDisposable
	{
		event EventHandler<AudioDataEventArgs> GotAudio;

		void Start();
		void Stop();
	}

	class WaveInAudioSource : IAudioSource, IDisposable
	{
		WaveInRecorder waveIn;

		public event EventHandler<AudioDataEventArgs> GotAudio;

		public void Dispose()
		{
			Stop();
		}

		public void Start()
		{
			if (waveIn != null)
				throw new InvalidOperationException();
			var waveFmt = new WaveFormat(App.AudioRate, App.AudioBits, App.AudioChannels);
			// device -1 == wave mapper
			waveIn = new WaveInRecorder(-1, waveFmt, App.AudioBufferSize, App.AudioBufferCount, GotAudioData);
		}

		public void Stop()
		{
			if (waveIn == null)
				return;
			waveIn.Dispose();
			waveIn = null;
		}

		void GotAudioData(IntPtr data, int size)
		{
			System.Diagnostics.Debug.Assert(App.AudioBits == 16);  // hardcoded for short[] and 65535.0

			short[] ints = new short[size / 2];
			Marshal.Copy(data, ints, 0, ints.Length);

			double[] flt = new double[ints.Length];
			for (int i = 0; i < ints.Length; ++i)
				flt[i] = ints[i] / 65535.0;

			GotAudio(this, new AudioDataEventArgs(flt));
		}
	}
}
