﻿using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class ExplorerTribeBehaviour : TribeBehaviour 
    {
        private Direction _currentDir;

        public ExplorerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            // initial move OR can no longer move in the same direction
            if (tribe.PrevPosition.x == tribe.Position.x && tribe.PrevPosition.y == tribe.Position.y)
            {
                PickDirection();
            }

            return Move(_currentDir);
        }

        private void PickDirection()
        {
            _currentDir = (Direction)_rnd.Next(8);
        }
    }
}