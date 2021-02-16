using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Core.Processors
{
    public partial class GameActionProcessor 
    {
        public void Update(MoveAction action, ITribe tribe, IWorld world, int iteration)
        {
            var direction = action.Direction;
            var oldPos = tribe.Position;

            if (CanMove(tribe.Position, direction, world))
            {
                tribe.Population -= 3;

                Move(tribe, direction, world);
            }
            else // Hold
            {
                tribe.Population--;
            }

            tribe.PrevPosition = oldPos;
        }

        private static bool CanMove((int x, int y) pos, Directions direction, IWorld world)
        {
            return direction switch
            {
                Directions.N => pos.y > 0,
                Directions.S => pos.y < world.Size - 1,
                Directions.W => pos.x > 0,
                Directions.E => pos.x < world.Size - 1,
                Directions.NW => pos.x > 0 && pos.y > 0,
                Directions.SE => pos.x < world.Size - 1 && pos.y < world.Size - 1,
                Directions.NE => pos.x < world.Size - 1 && pos.y > 0,
                Directions.SW => pos.x > 0 && pos.y < world.Size - 1,
                _ => true,
            };
        }

        private static void Move(ITribe tribe, Directions direction, IWorld world)
        {
            var (x, y) = tribe.Position;

            switch (direction)
            {
                case Directions.N:
                    tribe.Position = (tribe.Position.x, tribe.Position.y - 1);
                    break;

                case Directions.S:
                    tribe.Position = (tribe.Position.x, tribe.Position.y + 1);
                    break;

                case Directions.W:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y);
                    break;

                case Directions.E:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y);
                    break;

                case Directions.NW:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y - 1);
                    break;

                case Directions.SE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y + 1);
                    break;

                case Directions.NE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y - 1);
                    break;

                case Directions.SW:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y + 1);
                    break;
            }

            world.State[x, y] = null;

            if (world.State[tribe.Position.x, tribe.Position.y] is Food)
            {
                tribe.Population += (world.State[tribe.Position.x, tribe.Position.y] as Food).Value;
            }

            // TODO: add logic to handle iron and wood

            world.State[tribe.Position.x, tribe.Position.y] = tribe;
        }
    }
}