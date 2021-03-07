using Ev.Domain.Actions.Core;

namespace Ev.Domain.Entities.Core
{
    public interface ITribe : ITribeState
    {
        new int Population { get; set; }
        int PrevPopulation { get; set; }
        int DeadAtIteration { get; set; }

        bool IsAttacking { get; set; }

        new (int x, int y) Position { get; set; }
        new (int x, int y) PrevPosition { get; set; }

        int? LockedForNTurns { get; internal set; }
        internal float Attack { get; set; }
        internal float Defense { get; set; }
        IGameAction BusyDoing { get; set; }

        #region Resources
        
        new int Wood { get; set; }

        new int Iron { get; set; }

        #endregion
    }
}