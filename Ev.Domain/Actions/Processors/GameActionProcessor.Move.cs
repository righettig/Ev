using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor 
    {
        public void Update(MoveAction action, ITribe tribe, IWorld world, int iteration)
        {
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