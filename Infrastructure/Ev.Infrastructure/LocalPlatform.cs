using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Core;
using Ev.Infrastructure.Core;
using System;
using System.Collections.Generic;
using ITribe = Ev.Domain.Server.Core.ITribe;

namespace Ev.Infrastructure
{
    public class LocalPlatform : IPlatform
    {
        private readonly Mapper _mapper;
        private readonly Dictionary<string, ITribeBehaviour> _behaviours = new ();

        public LocalPlatform()
        {
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

        public void RegisterAgent(IGame game, params ITribeAgent[] agents)
        {
            foreach (var tribeAgent in agents)
            {
                // TODO: server should return a unique Id for each tribe agent
                game.RegisterAgent(tribeAgent.Name, tribeAgent.Color);

                // TODO: should use the unique tribe agent Id
                if (_behaviours.ContainsKey(tribeAgent.Name))
                {
                    throw new ArgumentException("Cannot register two or more agents with the same name.", nameof(agents));
                }

                _behaviours[tribeAgent.Name] = tribeAgent.Behaviour;
            }
        }

        public Domain.Server.Core.IGameAction Update(IWorldState worldState, ITribe tribe)
        {
            var clientWorldState = _mapper.Map(worldState);
            var clientTribe      = _mapper.Map(tribe);

            var behaviour = _behaviours[tribe.Name];
            behaviour.State = clientWorldState;

            var action = behaviour.DoMove(clientWorldState, clientTribe);

            var serverAction = _mapper.Map(action);

            serverAction.Tribe = tribe;

            return serverAction;
        }
    }
}
