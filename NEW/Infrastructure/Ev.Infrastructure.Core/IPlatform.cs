namespace Ev.Infrastructure.Core
{
    // IGameBus / IChannel
    public interface IPlatform
    {
        void OnGameStart();

        void OnGameEnd();

        void OnTurnStart();

        void OnTurnEnd();

        void RegisterAgent(Domain.Client.Core.ITribeAgent agent);

        Domain.Server.Core.IGameAction Update(Domain.Server.Core.IWorldState worldState, Domain.Server.Core.ITribe tribe);
    }
}
