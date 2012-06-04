using System.Collections.Generic;

namespace MouseAhead
{
	enum Consonant
	{
		None, K, S, T
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

	static class AudioAnalyzer
	{
		static List<FreqTrigger> freqTriggers;

		static AudioAnalyzer()
		{
			freqTriggers = new List<FreqTrigger>();
			freqTriggers.Add(new FreqTrigger(1000.0, 2000.0, 0.3, Consonant.K));
			freqTriggers.Add(new FreqTrigger(2500.0, 4000.0, 1.0, Consonant.T));
			freqTriggers.Add(new FreqTrigger(6000.0, 9000.0, 1.0, Consonant.S));
		}

		public static Tuple<Consonant, double> DetermineConsonant(double[] data)
		{
			var buckets = SoundAnalysis.FftAlgorithm.Calculate(data);

			Consonant cons = Consonant.None;
			double maxAvg = 0;
			foreach (var trigger in freqTriggers)
			{
				var avg = AverageRange(buckets, trigger.MinFreq, trigger.MaxFreq) * trigger.Strength;
				if (avg > maxAvg && avg > App.AudioSensitivity)
				{
					maxAvg = avg;
					cons = trigger.Consonant;
				}
			}

			double peakFreq = GetPeakFreq(buckets, 500, 20000);
			if (peakFreq < 1000 && cons != Consonant.None)
				cons = Consonant.None;

			return Tuple.Make(cons, peakFreq);
		}

		public static int FreqToIndex(double freq, int length)
		{
			return (int)(freq / App.AudioRate * length);
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
			return sum / (stopIndex - startIndex);
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
