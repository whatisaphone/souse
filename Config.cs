namespace Souse {
    internal sealed class Config {
        public int AudioRate = 44100;
        public int AudioBits = 16;
        public int AudioChannels = 1;
        public int AudioBufferSize = 6144;
        public int AudioBufferCount = 4;
        public double AudioHighPassFreq = 1000;
        public double AudioLowPassFreq = 20000;
        public double AudioTotalSensitivity = 0.015;
        public double AudioBucketSensitivity = 0.015;
        public double A440 = 437.5;  // frequency your mic hears for 440Hz input
        public string RPCBindAddress = "http://127.0.0.1:50253/";
    }
}
