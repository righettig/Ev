using System;
using static System.Math;

namespace Ev_NEW
{
    // Here I've also added a bunch of utility methods

    public abstract class TribeBehaviour : ITribeBehaviour
    {
        private const int WORLD_STATE_SIZE = 2; // TODO: use constant from WorldState

        protected readonly IRandom _rnd;

        private IWorldState _state;

        public TribeBehaviour(IRandom rnd)
        {
            _rnd = rnd;
        }

        // NB: this is used to cache the state so that calls from concrete behaviour can pass 
        // ref to entities and have the TribeBehaviour still expose a simply API
        internal IWorldState State 
        { 
            set 
            {
                _state = value;
            } 
        }

        public abstract IGameAction DoMove(IWorldState state, ITribe tribe);

        protected IGameAction Attack((int x, int y) target) 
        {
            ITribe enemy = null;

            _state.Traverse((el, x, y) =>
            {
                if (x == target.x && y == target.y)
                {
                    enemy = _state.GetEntity<ITribe>((x, y));

                    if (enemy is null)
                    {
                        throw new ArgumentException(); // not a tribe!
                    }
                }
            });

            if (enemy is null)
            {
                // TODO: throw TribeNotFoundException
                throw new ArgumentException();
            }
            else 
            {
                return new AttackAction(enemy.Name);
            }
        }

        protected static IGameAction Attack(ITribe target) => new AttackAction(target.Name);

        protected static IGameAction Hold() => new HoldAction();

        protected static IGameAction Move(Direction direction) => new MoveAction(direction);

        protected static IGameAction UpgradeDefenses() => new UpgradeDefensesAction();

        protected IGameAction MoveTowards(IWorldEntity entity) 
        {
            var result = (-1, -1);

            _state.Traverse((el, x, y) =>
            {
                if (el == entity)
                {
                    result = (x, y);
                }
            });

            return MoveTowards(result);
        }

        protected static IGameAction MoveTowards((int x, int y) target) => target switch
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.N),
            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE >= 0 => Move(Direction.S),
            (int, int) t when t.x - WORLD_STATE_SIZE >= 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.E),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.W),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.NE),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.NW),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.SE),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.SW),

            _ => null
        };

        protected IGameAction MoveAwayFrom(IWorldEntity entity)
        {
            var result = (-1, -1);

            _state.Traverse((el, x, y) =>
            {
                if (el == entity)
                {
                    result = (x, y);
                }
            });

            return MoveAwayFrom(result);
        }

        // TODO: unit test this
        protected static IGameAction MoveAwayFrom((int x, int y) target) => target switch
        {
            // Example: Target (1,3)

            // World state:
            //-------------
            // 0 0 0 0 0 
            // 0 0 0 0 0 
            // 0 0 P 0 0
            // 0 T 0 0 0 
            // 0 0 0 0 0 

            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.S),
            (int, int) t when t.x - WORLD_STATE_SIZE == 0 && t.y - WORLD_STATE_SIZE >= 0 => Move(Direction.N),
            (int, int) t when t.x - WORLD_STATE_SIZE >= 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.W),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE == 0 => Move(Direction.E),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.SW),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  < 0 => Move(Direction.SE),
            (int, int) t when t.x - WORLD_STATE_SIZE  > 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.NW),
            (int, int) t when t.x - WORLD_STATE_SIZE  < 0 && t.y - WORLD_STATE_SIZE  > 0 => Move(Direction.NE),

            _ => null
        };

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
                if (el is ICollectable)
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
                if (el is ICollectable c && c.Type == CollectableType.Food)
                {
                    if (c.Value > highest)
                    {
                        highest = c.Value;
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

            return Abs(target.x - WORLD_STATE_SIZE) <= 1 && Abs(target.y - WORLD_STATE_SIZE) <= 1;
        }
    }
}