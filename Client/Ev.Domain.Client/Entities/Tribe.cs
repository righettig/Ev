using Ev.Common.Utils;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Entities
{
    public class Tribe : ITribe
    {
        public string Name { get; }
        public Color Color { get; }

        public int Population { get; }

        public (int x, int y) Position { get; }

        public (int x, int y) PrevPosition { get; }

        public int Wood { get; }

        public int Iron { get; }


        public Tribe(ITribe other)
        {
            Name         = other.Name;
            Color        = other.Color;
            Population   = other.Population;
            Position     = other.Position;
            PrevPosition = other.PrevPosition;
            Wood         = other.Wood;
            Iron         = other.Iron;
        }

        public bool StrongerThan(ITribe other) => Population > other.Population;

        public bool WeakerThan(ITribe other) => Population < other.Population;
    }
}
