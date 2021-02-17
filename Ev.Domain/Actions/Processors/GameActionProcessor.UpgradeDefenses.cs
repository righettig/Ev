using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(UpgradeDefensesAction action, ITribe tribe, IWorld world, int iteration)
        {
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
