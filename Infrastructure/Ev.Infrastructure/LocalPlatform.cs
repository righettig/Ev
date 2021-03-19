using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Core;
using Ev.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static Ev.Helpers.Debug;
using ITribe = Ev.Domain.Server.Core.ITribe;

namespace Ev.Infrastructure
{
    public class LocalPlatform : IPlatform
    {
        private readonly IMapper _mapper;
        private readonly Dictionary<string, ITribeBehaviour> _behaviours = new ();
        
        private ITribeAgent[] _agents;

        public LocalPlatform(IMapper mapper) => _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public void OnGameStart() => ForEachTribeAgent(tribeAgent => tribeAgent.OnGameStart());

        public void OnGameEnd()   => ForEachTribeAgent(tribeAgent => tribeAgent.OnGameEnd());

        public void OnTurnStart() => ForEachTribeAgent(tribeAgent => tribeAgent.OnTurnStart());

        public void OnTurnEnd()   => ForEachTribeAgent(tribeAgent => tribeAgent.OnTurnEnd());

        public void RegisterAgent(IGame game, params ITribeAgent[] agents)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (agents == null) throw new ArgumentNullException(nameof(agents));
            if (agents.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(agents));

            _agents = agents;

            foreach (var tribeAgent in agents)
            {
                // TODO: should use the unique tribe agent Id
                if (_behaviours.ContainsKey(tribeAgent.Name))
                {
                    throw new ArgumentException("Cannot register two or more agents with the same name.", nameof(agents));
                }

                _behaviours[tribeAgent.Name] = tribeAgent.Behaviour;

                // TODO: server should return a unique Id for each tribe agent
                game.RegisterAgent(tribeAgent.Name, tribeAgent.Color);
            }
        }

        public Domain.Server.Core.IGameAction Update(IWorldState worldState, ITribe tribe)
        {
            if (worldState == null) throw new ArgumentNullException(nameof(worldState));
            if (tribe == null) throw new ArgumentNullException(nameof(tribe));

            var clientWorldState = _mapper.Map(worldState);
            var clientTribe      = _mapper.Map(tribe);

            _agents.First(el => el.Name == tribe.Name).OnBeforeMove(clientWorldState, clientTribe);

            var action = _behaviours[tribe.Name].DoMove(clientWorldState, clientTribe);

            if (action is PlayerControlledGameAction)
            {
                DumpActions();
                action = ReadAction(worldState);
            }

            var serverAction = _mapper.Map(action);
            
            serverAction.Tribe = tribe;
            return serverAction;
        }

        private void ForEachTribeAgent(Action<ITribeAgent> action)
        {
            foreach (var tribeAgent in _agents)
            {
                action(tribeAgent);
            }
        }
    }
}
