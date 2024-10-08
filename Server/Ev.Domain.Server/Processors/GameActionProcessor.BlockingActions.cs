﻿using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;
using System.Diagnostics;

namespace Ev.Domain.Server.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(BlockingGameAction action, ITribe tribe, IWorld world, int iteration)
        {
            Debug.Assert(action != null);
            Debug.Assert(tribe != null);
            Debug.Assert(world != null);
            Debug.Assert(iteration >= 0);

            if (!tribe.LockedForNTurns.HasValue)
            {
                tribe.LockedForNTurns = GameParams.Instance.UpgradeActionsLength;
                tribe.BusyDoing = action;
            }
            else
            {
                tribe.LockedForNTurns--;

                if (tribe.LockedForNTurns == 0)
                {
                    action.OnComplete(tribe);

                    tribe.LockedForNTurns = null;
                    tribe.BusyDoing = null;

                    action.Completed = true;
                }
            }
        }
    }
}
