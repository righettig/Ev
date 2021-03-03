using Ev.Domain.Actions.Core;
using Ev.Domain.World.Core;

namespace Ev.Domain.Entities.Core
{
    public interface ITribe : IWorldEntity
    {
        string Name { get; }

        int Population { get; set; }
        int PrevPopulation { get; set; }
        int DeadAtIteration { get; set; }

        bool IsAttacking { get; set; }

        (int x, int y) Position { get; set; }
        (int x, int y) PrevPosition { get; set; }

        IGameAction DoMove(IWorldState state);

        bool StrongerThan(ITribe other);
        bool WeakerThan(ITribe other);

        int? LockedForNTurns { get; internal set; }
        internal float Attack { get; set; }
        internal float Defense { get; set; }
        IGameAction BusyDoing { get; set; }

        #region Resources
        
        int Wood { get; internal set; }
        
        int Iron { get; internal set; }

        #endregion
    }
}