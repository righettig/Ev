using Ev.Common.Core;
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

        IGameAction Update(IWorldState worldState, ITribe tribe);
    }
}
