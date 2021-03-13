using Ev.Domain.Server.Core;
using Ev.Infrastructure.Core;

namespace Ev.Infrastructure
{
    public class RemotePlatform : IPlatform
    {
        public void OnGameStart()
        {
            throw new System.NotImplementedException();
        }

        public void OnGameEnd()
        {
            throw new System.NotImplementedException();
        }

        public void OnTurnStart()
        {
            throw new System.NotImplementedException();
        }

        public void OnTurnEnd()
        {
            throw new System.NotImplementedException();
        }

        public void RegisterAgent(params Domain.Client.Core.ITribeAgent[] agents)
        {
        }

        public IGameAction Update(IWorldState worldState, ITribe tribe)
        {
            throw new System.NotImplementedException();
        }
    }
}