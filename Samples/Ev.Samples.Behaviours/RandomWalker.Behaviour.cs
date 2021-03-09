﻿using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Samples.Behaviours
{
    public class RandomWalkerTribeBehaviour : TribeBehaviour
    {
        public RandomWalkerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            var move = _rnd.Next(9);

            if (move != 8)
            {
                return Move((Direction)move);
            }
        
            return Hold();
        }
    }
}