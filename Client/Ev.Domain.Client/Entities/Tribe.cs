using Ev.Common.Utils;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Entities
{
    public class Tribe : ITribe
    {
        public string Name { get; init; }

        public Color Color { get; init; }

        public int Population { get; init; }

        public (int x, int y) Position { get; init; }

        public (int x, int y) PrevPosition { get; init; }

        public int Wood { get; init; }

        public int Iron { get; init; }

        public bool StrongerThan(ITribe other) => Population > other.Population;

        public bool WeakerThan(ITribe other) => Population < other.Population;
    }
}
