using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using static System.Math;

namespace Ev.Domain.Behaviours.Core
{
    // TODO: move static methods into a static helper class that can be unit tested

    public abstract class TribeBehaviour : ITribeBehaviour
    {
        protected readonly IRandom _rnd;

        public TribeBehaviour(IRandom rnd)
        {
            _rnd = rnd;
        }

        public abstract IGameAction DoMove(IWorldState state, ITribe tribe);

        protected static IGameAction Attack(ITribe target) => new AttackAction(target);

        protected static IGameAction Hold() => new HoldAction();
        
        protected static IGameAction Move(Direction direction) => new MoveAction(direction);

        protected static IGameAction UpgradeDefenses() => new UpgradeDefensesAction();

        // TODO: unit test this
        protected static IGameAction MoveTowards((int x, int y) target)
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            IGameAction result = null;

            if (target.x - WorldState.WORLD_STATE_SIZE == 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.N);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE == 0 && target.y - WorldState.WORLD_STATE_SIZE >= 0)
            {
                result = Move(Direction.S);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE >= 0 && target.y - WorldState.WORLD_STATE_SIZE == 0)
            {
                result = Move(Direction.E);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE == 0)
            {
                result = Move(Direction.W);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE > 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.NE);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.NW);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE > 0 && target.y - WorldState.WORLD_STATE_SIZE > 0)
            {
                result = Move(Direction.SE);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE > 0)
            {
                result = Move(Direction.SW);
            }

            return result;
        }

        // TODO: unit test this
        protected static IGameAction MoveAwayFrom((int x, int y) target)
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            IGameAction result = null;

            if (target.x - WorldState.WORLD_STATE_SIZE == 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.S);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE == 0 && target.y - WorldState.WORLD_STATE_SIZE >= 0)
            {
                result = Move(Direction.N);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE >= 0 && target.y - WorldState.WORLD_STATE_SIZE == 0)
            {
                result = Move(Direction.W);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE == 0)
            {
                result = Move(Direction.E);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE > 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.SW);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE < 0)
            {
                result = Move(Direction.SE);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE > 0 && target.y - WorldState.WORLD_STATE_SIZE > 0)
            {
                result = Move(Direction.NW);
            }
            else if (target.x - WorldState.WORLD_STATE_SIZE < 0 && target.y - WorldState.WORLD_STATE_SIZE > 0)
            {
                result = Move(Direction.NE);
            }

            return result;
        }

        protected IGameAction RandomWalk() => new MoveAction((Direction)_rnd.Next(8));

        protected static (int x, int y) FindAnEnemy(IWorldState state)
        {
            var enemyPos = (-1, -1);

            state.Traverse((el, x, y) =>
            {
                if (el is ITribe)
                {
                    enemyPos = (x, y);
                }
            });

            return enemyPos;
        }

        protected static (int x, int y) FindACollectable(IWorldState state)
        {
            var collectablePos = (-1, -1);

            state.Traverse((el, x, y) =>
            {
                if (el is ICollectableWorldEntity)
                {
                    collectablePos = (x, y);
                }
            });

            return collectablePos;
        }

        protected static (int x, int y) FindHighestValueFood(IWorldState state)
        {
            var highestPos = (-1, -1);
            var highest = 0;

            state.Traverse((el, x, y) =>
            {
                if (el is Food food)
                {
                    if (food.Value > highest)
                    {
                        highest = food.Value;
                        highestPos = (x, y);
                    }
                }
            });

            return highestPos;
        }

        protected static bool NotFound((int x, int y) target)
        {
            return target.x == -1 && target.y == -1;
        }

        protected static bool Found((int x, int y) target) => !NotFound(target);

        // TODO: unit test this
        protected static bool Close((int x, int y) target)
        {
            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 0 0 T 0 
            // 0 0 0 0 0 

            return Abs(target.x - WorldState.WORLD_STATE_SIZE) <= 1 && Abs(target.y - WorldState.WORLD_STATE_SIZE) <= 1;
        }
    }
}