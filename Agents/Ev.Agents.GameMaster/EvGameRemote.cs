using Ev.Agents.Core;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using Ev.Game;
using System.Collections.Generic;

namespace Ev.Agents.GameMaster
{
    class EvGameRemote : BaseGame
    {
        private readonly List<Proxies.Tribe> _tribes;
        private readonly GameStateSerialiser _gameStateSerialiser;

        public EvGameRemote(
            GameOptions options,
            IWorld world,
            IRandom rnd,
            List<Proxies.Tribe> tribes,
            GameStateSerialiser gameStateSerialiser) : base(options, world, rnd)
        {
            _tribes = tribes;
            _gameStateSerialiser = gameStateSerialiser;
        }

        protected override IGameAction OnDoMove(IWorldState state, ITribe tribe)
        {
            var alive = _world.GetAliveTribes();

            var msg = _gameStateSerialiser.CreateDoMoveRequest(state, tribe);

            var reply = _tribes
                .Find(t => t.Name == tribe.Name)
                .DoMove(msg);

            return _gameStateSerialiser.Deserialise(reply, tribe, alive);
        }
    }
}