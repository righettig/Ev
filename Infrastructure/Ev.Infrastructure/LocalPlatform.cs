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
        }

        public void OnGameEnd()
        {
        }

        public void OnTurnStart()
        {
        }

        public void OnTurnEnd()
        {
        }

        public void RegisterAgent(params ITribeAgent[] agents)
        {
            foreach (var tribeAgent in agents)
            {
                _behaviours[tribeAgent.Name] = tribeAgent.Behaviour;

                _game.RegisterAgent(tribeAgent.Name, tribeAgent.Color);
            }
        }

        public Domain.Server.Core.IGameAction Update(IWorldState worldState, ITribe tribe)
        {
            Domain.Client.Core.IWorldState clientWorldState = _mapper.Map(worldState);
            Domain.Client.Core.ITribe      clientTribe      = _mapper.Map(tribe);

            var behaviour = _behaviours[tribe.Name];
            behaviour.State = clientWorldState;

            var action = behaviour.DoMove(clientWorldState, clientTribe);

            Domain.Server.Core.IGameAction result = _mapper.Map(action);

            return result;
        }
    }
}
