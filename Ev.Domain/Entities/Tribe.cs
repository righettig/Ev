using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Domain.Entities
{
    public class Tribe : ITribe 
    {
        public int Population { get; set; }
        public int PrevPopulation { get; set; }
        public int DeadAtIteration { get; set; }
        public string Name { get; }
        public (int x, int y) Position { get; set; }
        public (int x, int y) PrevPosition { get; set; }
        public Color Color { get; init; }
        public bool IsAttacking { get; set; }
        int? ITribe.LockedForNTurns { get; set; }
        int ITribe.Defense { get; set; }
        public IGameAction BusyDoing { get; set; }
        int ITribe.Wood { get; set; }
        int ITribe.Iron { get; set; }

        private readonly ITribeBehaviour _behaviour;

        public Tribe(string name, (int x, int y) position, Color color, ITribeBehaviour behaviour)
        {
            Name = name;
            Position = position;
            PrevPosition = Position;
            Color = color;
            Population = 100;
            PrevPopulation = Population;

            _behaviour = behaviour;
        }

        public bool StrongerThan(ITribe t) => Population > t.Population;

        public bool WeakerThan(ITribe t) => Population < t.Population;

        public IGameAction DoMove(IWorldState state)
        {
            if (BusyDoing != null) return BusyDoing;

            var move = _behaviour.DoMove(state, this);
            
            move.Tribe = this;

            return move;
        }
    }
}
