namespace Ev.Domain.Server.Core
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

        //IGameAction DoMove(IWorldState state);

        int? LockedForNTurns { get; set; }
        float Attack { get; set; }
        float Defense { get; set; }
        
        IGameAction BusyDoing { get; set; }

        #region Resources
        
        int Wood { get; set; }
        
        int Iron { get; set; }

        #endregion

        //public string DebugBehaviour();
    }
}