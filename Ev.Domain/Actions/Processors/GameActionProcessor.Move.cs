using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
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
                tribe.Population -= GameParams.Instance.MoveLoss;

                world.Move(tribe, direction);
            }
            else // Hold
            {
                tribe.Population -= GameParams.Instance.IdleLoss;
            }

            tribe.PrevPosition = oldPos;
        }
    }
}