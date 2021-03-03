using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using System.Diagnostics;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(UpgradeAttackAction action, ITribe tribe, IWorld world, int iteration)
        {
            Debug.Assert(action != null);
            Debug.Assert(tribe != null);
            Debug.Assert(world != null);
            Debug.Assert(iteration >= 0);

            if (!tribe.LockedForNTurns.HasValue)
            {
                tribe.LockedForNTurns = 2;
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
