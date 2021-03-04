using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities
{
    public class TribeState : ITribeState
    {
        public string Name { get; }

        public int Population { get; }

        public (int x, int y) Position { get; }

        public (int x, int y) PrevPosition { get; }

        public int Wood { get; }

        public int Iron { get; }

        public Color Color { get; }

        public TribeState(ITribe other)
        {
            Name         = other.Name;
            Population   = other.Population;
            Position     = other.Position;
            PrevPosition = other.PrevPosition;
            Wood         = other.Wood;
            Iron         = other.Iron;
            Color        = other.Color;
        }

        public bool StrongerThan(ITribeState other) => Population > other.Population;

        public bool WeakerThan(ITribeState other) => Population < other.Population;

        public IWorldEntity ToImmutable() => this;
    }
}
