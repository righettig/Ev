using Ev.Domain.Actions;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities
{
    public class TribeState : ITribeState
    {
        public string Name { get; }
        
        public Color Color { get; }

        public int Population { get; init; }

        public (int x, int y) Position { get; init; }

        public (int x, int y) PrevPosition { get; init; }

        public int Wood { get; init; }

        public int Iron { get; init; }

        public TribeState()
        {
        }

        public TribeState(ITribe other)
        {
            Name         = other.Name;
            Color        = other.Color;
            Population   = other.Population;
            Position     = other.Position;
            PrevPosition = other.PrevPosition;
            Wood         = other.Wood;
            Iron         = other.Iron;
            //Attack       = other.Attack;
            //Defense      = other.Defense;
        }

        public bool StrongerThan(ITribeState other) => Population > other.Population;

        public bool WeakerThan(ITribeState other) => Population < other.Population;

        public IWorldEntity ToImmutable() => this;
    }
}
