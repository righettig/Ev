using Ev.Common.Utils;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Entities
{
    public class Tribe : ITribe 
    {
        public string Name { get; }
        public Color Color { get; init; }

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
        
        public (int x, int y) Position { get; set; }
        public (int x, int y) PrevPosition { get; set; }
        
        public bool IsAttacking { get; set; }
        public int? LockedForNTurns { get; set; }
        public float Attack { get; set; }
        public float Defense { get; set; }

        public IGameAction BusyDoing { get; set; }
        public int Wood { get; set; }

        public int Iron { get; set; }

        private int _population;

        //private readonly ITribeBehaviour _behaviour;

        public Tribe(string name,
                     (int x, int y) position,
                     Color color)
        {
            Name           = name;
            Position       = position;
            PrevPosition   = Position;
            Color          = color;
            Population     = 100;
            PrevPopulation = 100;
        }

        //public IGameAction DoMove(IWorldState state)
        //{
        //    if (state is null)
        //    {
        //        throw new System.ArgumentNullException(nameof(state));
        //    }

        //    if (BusyDoing != null) return BusyDoing;

        //    _behaviour.State = state;

        //    var move = _behaviour.DoMove(state, ToImmutable() as ITribeState);

        //    move.Tribe = this;

        //    return move;
        //}

        //public IWorldEntity ToImmutable()
        //{
        //    return new TribeState(this);
        //}

        //public string DebugBehaviour() 
        //{
        //    return (_behaviour as TribeBehaviour).DebugBehaviour();
        //}
    }
}
