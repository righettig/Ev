using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using System.Diagnostics;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(SuicideAction action, ITribe tribe, IWorld world, int iteration)
        {
            Debug.Assert(action != null);
            Debug.Assert(tribe != null);
            Debug.Assert(world != null);
            Debug.Assert(iteration >= 0);

            tribe.Population = 0;
        }
    }
}