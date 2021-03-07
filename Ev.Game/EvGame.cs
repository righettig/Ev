using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System.Collections.Generic;
using static Ev.Helpers.Debug;

namespace Ev.Game
{
    public class EvGame : BaseGame
    {
        private readonly IDictionary<string, ITribeBehaviour> _behaviours;

        public EvGame(
            IDictionary<string, ITribeBehaviour> behaviours, 
            GameOptions options, 
            IWorld world, 
            IRandom rnd) : base(options, world, rnd)
        {
            _behaviours = behaviours;
        }

        protected override IGameAction OnDoMove(IWorldState state, ITribe tribe)
        {
            var behaviour = _behaviours[tribe.Name];

            behaviour.State = state;
            var move = behaviour.DoMove(state, new TribeState(tribe));

            if (move is PlayerControlledGameAction)
            {
                DumpActions();
                move = ReadAction(state);
            }

            return move;
        }
    }
}
