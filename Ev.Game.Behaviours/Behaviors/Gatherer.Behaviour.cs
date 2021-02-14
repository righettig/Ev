﻿using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;

namespace Ev.Behaviours
{
    public class GathererTribeBehaviour : TribeBehaviour
    {
        public GathererTribeBehaviour(IRandom rnd) : base(rnd) { }
        
        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var food = FindHighestValueFood(state);

            if (NotFound(food))
            {
                return RandomWalk();
            }
            else 
            {
                return MoveTowards(food);
            }            
        }
    }
}