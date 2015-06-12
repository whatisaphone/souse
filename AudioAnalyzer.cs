using System;
using System.Collections.Generic;

namespace Souse
{
	enum Consonant
	{
		None, K, S, T, A, As, B, C, Cs, D, Ds, E, F, Fs, G, Gs, HighA, HighAs, HighB,
	}

	struct FreqTrigger
	{
		public double MinFreq { get; private set; }
		public double MaxFreq { get; private set; }
		public double Strength { get; private set; }
		public Consonant Consonant { get; private set; }

		public FreqTrigger(double minFreq, double maxFreq, double strength, Consonant consonant) : this()
		{
			MinFreq = minFreq;
			MaxFreq = maxFreq;
			Strength = strength;
			Consonant = consonant;
		}
	}

	struct AudioAnalysisResult
	{
		public Consonant Consonant { get; private set; }
		public double AverageVolume { get; private set; }
		public double LoudestFreq { get; private set; }

		public AudioAnalysisResult(Consonant consonant, double averageVolume, double loudestFreq)
			: this()
		{
			Consonant = consonant;
			AverageVolume = averageVolume;
			LoudestFreq = loudestFreq;
		}
	}

	static class AudioAnalyzer
	{
		static List<FreqTrigger> freqTriggers;

		static AudioAnalyzer()
		{
			freqTriggers = new List<FreqTrigger>();
			freqTriggers.Add(new FreqTrigger( 900.0, 1500.0, 0.9, Consonant.K));
			freqTriggers.Add(new FreqTrigger(1500.0, 3000.0, 1.5, Consonant.T));
			freqTriggers.Add(new FreqTrigger(3800.0, 9000.0, 1.0, Consonant.S));
			for (var semi = 0; semi <= 14; ++semi)
			{
				var tone = App.A880 * Math.Pow(2, semi / 12.0);
				var strength = Math.Max(0.25, 0.3333 / Math.Pow(2, semi / 12.0));
				freqTriggers.Add(new FreqTrigger(tone * 0.95, tone * 1.05, strength, Consonant.A + semi));
			}
		}

		public static AudioAnalysisResult AnalyzeFrame(double[] data)
		{
			var buckets = SoundAnalysis.FftAlgorithm.Calculate(data);

			double totalAvg = AverageRange(buckets, App.AudioHighPassFreq, App.AudioLowPassFreq);
			double peakFreq = GetPeakFreq(buckets, App.AudioHighPassFreq, App.AudioLowPassFreq);
			var cons = totalAvg < App.AudioTotalSensitivity ? Consonant.None : DetermineConsonant(buckets);

			return new AudioAnalysisResult(cons, totalAvg, peakFreq);
		}

		private static Consonant DetermineConsonant(double[] buckets)
		{
			Consonant ret = Consonant.None;
			double maxAvg = 0;
			foreach (var trigger in freqTriggers)
			{
				var avg = AverageRange(buckets, trigger.MinFreq, trigger.MaxFreq) * trigger.Strength;
				if (avg > maxAvg && avg > App.AudioBucketSensitivity)
				{
					maxAvg = avg;
					ret = trigger.Consonant;
				}
			}

			return ret;
		}

		public static int FreqToIndex(double freq, int length)
		{
			var ret = (int)(freq / App.AudioRate * length);
			if (ret < 0)
				return 0;
			if (ret >= length)
				return length - 1;
			return ret;
		}

		public static double IndexToFreq(int index, int length)
		{
			return index * App.AudioRate / length;
		}

		public static double AverageRange(double[] data, double startFreq, double stopFreq)
		{
			int startIndex = FreqToIndex(startFreq, data.Length);
			int stopIndex = FreqToIndex(stopFreq, data.Length);

			double sum = 0;
			for (var i = startIndex; i <= stopIndex; ++i)
				sum += data[i];
			return sum / (stopIndex - startIndex + 1);
		}

		public static double GetPeakFreq(double[] data, double startFreq, double stopFreq)
		{
			int startIndex = FreqToIndex(startFreq, data.Length);
			int stopIndex = FreqToIndex(stopFreq, data.Length);

			double maxValue = 0;
			int maxIndex = 0;
			for (var i = startIndex; i < stopIndex; ++i)
				if (data[i] > maxValue)
				{
					maxIndex = i;
					maxValue = data[i];
				}

			return IndexToFreq(maxIndex, data.Length);
		}
	}
}
