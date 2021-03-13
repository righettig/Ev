using Ev.Domain.Server.Core;

namespace Ev.Infrastructure.Core
{
    // IGameBus / IChannel
    public interface IPlatform
    {
        void OnGameStart();

        void OnGameEnd();

        void OnTurnStart();

        void OnTurnEnd();

        void RegisterAgent(IGame game, params Domain.Client.Core.ITribeAgent[] agents);

        Domain.Server.Core.IGameAction Update(Domain.Server.Core.IWorldState worldState, Domain.Server.Core.ITribe tribe);
    }
}
