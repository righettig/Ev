using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Domain.Entities
{
    public class Tribe : ITribe 
    {
        public int Population
        {
            get => _population;
            set
            {
                PrevPopulation = _population;

                _population = value;
            }
        }
        public int PrevPopulation { get; set; }
        public int DeadAtIteration { get; set; }
        public string Name { get; }
        public (int x, int y) Position { get; set; }
        public (int x, int y) PrevPosition { get; set; }
        public Color Color { get; init; }
        public bool IsAttacking { get; set; }
        int? ITribe.LockedForNTurns { get; set; }
        float ITribe.Attack { get; set; }
        float ITribe.Defense { get; set; }
        public IGameAction BusyDoing { get; set; }
        int ITribe.Wood { get; set; }
        int ITribe.Iron { get; set; }

        public int Wood { get; set; }

        public int Iron { get; set; }

        private int _population;

        private readonly ITribeBehaviour _behaviour;

        public Tribe(string name,
                     (int x, int y) position,
                     Color color,
                     ITribeBehaviour behaviour)
        {
            Name           = name;
            Position       = position;
            PrevPosition   = Position;
            Color          = color;
            Population     = 100;
            PrevPopulation = 100;

            _behaviour = behaviour ?? throw new System.ArgumentNullException(nameof(behaviour));
        }

        public IGameAction DoMove(IWorldState state)
        {
            if (state is null)
            {
                throw new System.ArgumentNullException(nameof(state));
            }

            if (BusyDoing != null) return BusyDoing;

            _behaviour.State = state;

            var move = _behaviour.DoMove(state, ToImmutable() as ITribeState);

            move.Tribe = this;

            return move;
        }

        public IWorldEntity ToImmutable()
        {
            return new TribeState(this);
        }

        public bool StrongerThan(ITribeState other) => Population > other.Population;

        public bool WeakerThan(ITribeState other) => Population < other.Population;

        public string DebugBehaviour() 
        {
            return (_behaviour as TribeBehaviour).DebugBehaviour();
        }
    }
}
