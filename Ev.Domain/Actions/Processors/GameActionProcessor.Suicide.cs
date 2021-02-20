using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(SuicideAction action, ITribe tribe, IWorld world, int iteration)
        {
            tribe.Population = 0;
        }
    }
}