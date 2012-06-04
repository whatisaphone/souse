namespace MouseAhead
{
	enum Consonant
	{
		None, K, S, T
	}

	class AudioAnalyzer
	{
		public static Consonant DetermineConsonant(double[] data)
		{
			var buckets = SoundAnalysis.FftAlgorithm.Calculate(data);
			double kPeak = AverageRange(buckets, 1000, 2000);
			double tPeak = AverageRange(buckets, 2500, 4500);
			double sPeak = AverageRange(buckets, 5000, 8000);
			if (kPeak > App.AudioSensitivity && kPeak > tPeak && kPeak > sPeak)
				return Consonant.K;
			if (tPeak > App.AudioSensitivity && tPeak > kPeak && tPeak > kPeak)
				return Consonant.T;
			if (sPeak > App.AudioSensitivity && sPeak > kPeak && sPeak > tPeak)
				return Consonant.S;
			return Consonant.None;
		}

		public static double AverageRange(double[] data, double startFreq, double stopFreq)
		{
			int startIndex = (int)(startFreq / App.AudioRate * data.Length);
			int stopIndex = (int)(stopFreq / App.AudioRate * data.Length);
			double sum = 0;
			for (var i = startIndex; i <= stopIndex; ++i)
				sum += data[i];
			return sum / (stopIndex - startIndex);
		}
	}
}
