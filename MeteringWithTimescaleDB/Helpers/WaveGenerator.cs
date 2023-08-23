namespace MeteringTest.Helpers
{
    public class WaveGenerator
    {
        private readonly DateTime startTime;
        private readonly TimeSpan period;
        private readonly double minValue;
        private readonly double maxValue;
        private readonly double amplitude;
        private readonly Random random;

        public WaveGenerator(DateTime startTime, TimeSpan period, double minValue, double maxValue)
        {
            this.startTime = startTime;
            this.period = period;
            this.minValue = minValue;
            this.maxValue = maxValue;
            amplitude = maxValue - minValue;
            random = new Random();
        }


        public double GenerateSawtooth(DateTime inputTime)
        {
            TimeSpan elapsedTime = inputTime - startTime;
            double phase = elapsedTime.TotalSeconds % period.TotalSeconds / period.TotalSeconds;
            double value = minValue + phase * (maxValue - minValue);
            return value + getRandomNoice();
        }

        public double GenerateSquare(DateTime inputTime)
        {
            TimeSpan elapsedTime = inputTime - startTime;
            double phase = elapsedTime.TotalSeconds % period.TotalSeconds / period.TotalSeconds;
            double value = phase < 0.5 ? minValue : maxValue;
            return value + getRandomNoice();
        }

        private double getRandomNoice()
        {
            // Generate random noise between -(20% * amplitude) and (20% * amplitude)
            return amplitude / 5 * (random.NextDouble() - 0.5);
        }
    }
}
