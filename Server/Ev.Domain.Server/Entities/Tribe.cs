using Ev.Common.Core;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Entities
{
    public class Tribe : ITribe 
    {
        public string Name { get; }

        public Color Color { get; }

        public int Population
        {
            get => _population;
            set
            {
                PrevPopulation = _population;

                _population = value;
            }
        }
        public int PrevPopulation { get; private set; }
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

        public Tribe(string name, (int x, int y) position, Color color)
        {
            Name           = name;
            Position       = position;
            PrevPosition   = Position;
            Color          = color;
            Population     = GameParams.Instance.InitialPopulation;
            PrevPopulation = Population;
        }
    }
}
