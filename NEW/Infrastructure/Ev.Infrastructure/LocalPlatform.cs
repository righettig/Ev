using Ev.Common;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Core;
using Ev.Infrastructure.Core;
using System.Collections.Generic;
using ITribe = Ev.Domain.Server.Core.ITribe;
using IWorldState = Ev.Domain.Server.Core.IWorldState;

namespace Ev.Infrastructure
{
    public class LocalPlatform : IPlatform
    {
        private readonly IGame _game;
        private readonly Mapper _mapper;
        private readonly Dictionary<string, ITribeBehaviour> _behaviours = new ();

        public LocalPlatform(IGame game)
        {
            _game = game;
            _mapper = new Mapper();
        }

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

        public void RegisterAgent(Domain.Client.Core.ITribeAgent agent)
        {
            _behaviours[agent.Name] = agent.Behaviour;

            _game.RegisterAgent(agent.Name, agent.Color);
        }

        public IGameAction Update(IWorldState worldState, ITribe tribe)
        {
            Domain.Client.Core.IWorldState clientWorldState = _mapper.Map(worldState);
            Domain.Client.Core.ITribe      clientTribe      = _mapper.Map(tribe);

            var behaviour = _behaviours[tribe.Name];

            var action = behaviour.DoMove(clientWorldState, clientTribe);

            return null;
        }
    }

    public class Mapper
    {
        public Domain.Client.Core.IWorldState Map(Domain.Server.Core.IWorldState worldState)
        {
            return null;
        }

        public Domain.Client.Core.ITribe Map(Domain.Server.Core.ITribe tribe)
        {
            return null;
        }
    }
}
