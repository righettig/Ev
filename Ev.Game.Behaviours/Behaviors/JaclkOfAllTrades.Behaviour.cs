﻿using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class JaclkOfAllTradesTribeBehaviour : TribeBehaviour
    {
        public JaclkOfAllTradesTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var collectable = FindACollectable(state);

            if (NotFound(collectable))
            {
                return RandomWalk();
            }
            else
            {
                return MoveTowards(collectable);
            }
        }
    }
}