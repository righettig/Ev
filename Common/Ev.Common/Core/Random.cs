using Ev.Common.Core.Interfaces;

namespace Ev.Common.Core
{
    public class Random : IRandom
    {
        private readonly System.Random _rnd;

        public Random() => _rnd = new System.Random();

        public Random(int seed) => _rnd = new System.Random(seed);

        public int Next(int maxValue) => _rnd.Next(maxValue);

        public int Next(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);

        public double NextDouble() => _rnd.NextDouble();
    }
}
