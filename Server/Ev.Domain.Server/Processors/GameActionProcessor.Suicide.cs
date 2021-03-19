using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;
using System.Diagnostics;

namespace Ev.Domain.Server.Processors
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