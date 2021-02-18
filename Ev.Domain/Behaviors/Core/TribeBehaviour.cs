using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using static System.Math;

namespace Ev.Domain.Behaviours.Core
{
    // TODO: introduce constant for magic number "2"
    // TODO: move static methods into a static helper class that can be unit tested

    public abstract class TribeBehaviour : ITribeBehaviour
    {
        protected readonly IRandom _rnd;

        public TribeBehaviour(IRandom rnd)
        {
            _rnd = rnd;
        }

        public abstract IGameAction DoMove(IWorldState state, ITribe tribe);

        protected static AttackAction Attack(ITribe target) => new AttackAction(target);

        protected static HoldAction Hold() => new HoldAction();
        
        protected static MoveAction Move(Directions direction) => new MoveAction(direction);

        protected static MoveAction MoveTowards((int x, int y) target)
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            MoveAction result = null;

            if (target.x - 2 == 0 && target.y - 2 < 0)
            {
                // TODO: create helper "Move(Direction dir)" so that all instantiation of MoveAction are centralised
                // TODO: helper should be usabled by subclasses
                result = new MoveAction(Directions.N);
            }
            else if (target.x - 2 == 0 && target.y - 2 >= 0)
            {
                result = new MoveAction(Directions.S);
            }
            else if (target.x - 2 >= 0 && target.y - 2 == 0)
            {
                result = new MoveAction(Directions.E);
            }
            else if (target.x - 2 < 0 && target.y - 2 == 0)
            {
                result = new MoveAction(Directions.W);
            }
            else if (target.x - 2 > 0 && target.y - 2 < 0)
            {
                result = new MoveAction(Directions.NE);
            }
            else if (target.x - 2 < 0 && target.y - 2 < 0)
            {
                result = new MoveAction(Directions.NW);
            }
            else if (target.x - 2 > 0 && target.y - 2 > 0)
            {
                result = new MoveAction(Directions.SE);
            }
            else if (target.x - 2 < 0 && target.y - 2 > 0)
            {
                result = new MoveAction(Directions.SW);
            }

            return result;
        }

        protected static MoveAction MoveAwayFrom((int x, int y) target)
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            MoveAction result = null;

            if (target.x - 2 == 0 && target.y - 2 < 0)
            {
                result = new MoveAction(Directions.S);
            }
            else if (target.x - 2 == 0 && target.y - 2 >= 0)
            {
                result = new MoveAction(Directions.N);
            }
            else if (target.x - 2 >= 0 && target.y - 2 == 0)
            {
                result = new MoveAction(Directions.W);
            }
            else if (target.x - 2 < 0 && target.y - 2 == 0)
            {
                result = new MoveAction(Directions.E);
            }
            else if (target.x - 2 > 0 && target.y - 2 < 0)
            {
                result = new MoveAction(Directions.SW);
            }
            else if (target.x - 2 < 0 && target.y - 2 < 0)
            {
                result = new MoveAction(Directions.SE);
            }
            else if (target.x - 2 > 0 && target.y - 2 > 0)
            {
                result = new MoveAction(Directions.NW);
            }
            else if (target.x - 2 < 0 && target.y - 2 > 0)
            {
                result = new MoveAction(Directions.NE);
            }

            return result;
        }

        protected MoveAction RandomWalk() => new MoveAction((Directions)_rnd.Next(8));

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

        protected static bool Close((int x, int y) target)
        {
            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 0 0 T 0 
            // 0 0 0 0 0 

            return Abs(target.x - 2) <= 1 && Abs(target.y - 2) <= 1;
        }
    }
}