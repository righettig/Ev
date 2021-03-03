using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using System.Diagnostics;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor 
    {
        public void Update(MoveAction action, ITribe tribe, IWorld world, int iteration)
        {
            Debug.Assert(action != null);
            Debug.Assert(tribe != null);
            Debug.Assert(world != null);
            Debug.Assert(iteration >= 0);

            var direction = action.Direction;
            var oldPos = tribe.Position;

            if (world.CanMove(tribe.Position, direction))
            {
                tribe.Population -= 3;

                world.Move(tribe, direction);
            }
            else // Hold
            {
                tribe.Population--;
            }

            tribe.PrevPosition = oldPos;
        }
    }
}