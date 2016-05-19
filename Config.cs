using Functional.Maybe;
using System;
using System.Collections.Generic;
using System.IO;

namespace Souse {
    internal sealed class Config {
        public int AudioRate;
        public int AudioBits;
        public int AudioChannels;
        public int AudioBufferSize;
        public int AudioBufferCount;
        public double AnalysisHighPassFreq;
        public double AnalysisLowPassFreq;
        public double AnalysisTotalSensitivity;
        public double AnalysisBucketSensitivity;
        public double AnalysisA440;
        public string RPCBindPrefix;

        private Config() {
        }

        public static Config Read() {
            var ret = new Config();
            ret.Reload();
            return ret;
        }

        private static Dictionary<string, string> ReadIniFile(string filename) {
            var ret = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(filename)) {
                var pair = line.Split(new[] {'='});
                ret[pair[0].Trim()] = pair[1].Trim();
            }
            return ret;
        }

        public void Reload() {
            var homeDir = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");

            Dictionary<string, string> ini;
            try {
                ini = ReadIniFile(Path.Combine(homeDir, ".souse"));
            } catch (IOException) {
                ini = new Dictionary<string, string>();
            }

            AudioRate = ini.Lookup("audio-in-sample-rate").Select(int.Parse).OrElse(44100);
            AudioBits = ini.Lookup("audio-in-bits").Select(int.Parse).OrElse(16);
            AudioChannels = ini.Lookup("audio-in-channels").Select(int.Parse).OrElse(1);
            AudioBufferSize = ini.Lookup("audio-in-buffer-size").Select(int.Parse).OrElse(6144);
            AudioBufferCount = ini.Lookup("audio-in-buffer-count").Select(int.Parse).OrElse(4);
            AnalysisHighPassFreq = ini.Lookup("analysis-high-pass-frequency").Select(int.Parse).OrElse(1000);
            AnalysisLowPassFreq = ini.Lookup("analysis-low-pass-frequency").Select(int.Parse).OrElse(20000);
            AnalysisTotalSensitivity = ini.Lookup("analysis-total-sensitivity").Select(double.Parse).OrElse(0.015);
            AnalysisBucketSensitivity = ini.Lookup("analysis-bucket-sensitivity").Select(double.Parse).OrElse(0.015);
            AnalysisA440 = ini.Lookup("analysis-a440").Select(double.Parse).OrElse(440);
            RPCBindPrefix = ini.Lookup("rpc-bind-prefix").OrElse((string) null);
        }
    }
}
