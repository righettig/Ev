using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ev.Domain.World
{
    public class World : IWorld
    {
        public IWorldEntity[,] State => _state;

        public int Size => _size;

        public IEnumerable<ITribe> Tribes => _tribes;

        public bool Finished { get; private set; }

        public ITribe Winner { get; private set; }

        private const int WORLD_STATE_SIZE = 2;

        private readonly IWorldEntity[,] _state;
        private readonly IRandom _rnd;
        private readonly int _size;

        private readonly List<ITribe> _tribes = new List<ITribe>();

        // creates a squared matrix of size 'size'
        // assigns 'food' tiles
        // assigns 'wood' tiles
        // assigns 'iron' tiles
        public World(int size, int food, int wood, int iron, IRandom rnd)
        {
            if ((food + wood + iron) > size * size)
                throw new ArgumentOutOfRangeException("food, wood, iron", "Too many collectables requested.");

            _state = new IWorldEntity[size, size];
            _rnd = rnd;
            _size = size;

            InitCollectable(food, (rnd) => new Food(rnd));
            InitCollectable(wood, (rnd) => new Wood(rnd));
            InitCollectable(iron, (rnd) => new Iron(rnd));
        }

        public bool Update(ITribe tribe, IGameAction move, int iteration)
        {
            if (move is HoldAction)
            {
                tribe.Population--;
            }
            else if (move is MoveAction m)
            {
                var direction = m.Direction;
                var oldPos = tribe.Position;

                if (CanMove(tribe.Position, direction))
                {
                    tribe.Population -= 3;

                    Move(tribe, direction);
                }
                else // Hold
                {
                    tribe.Population--;
                }

                tribe.PrevPosition = oldPos;
            }
            else if (move is AttackAction a)
            {
                tribe.IsAttacking = true;
                a.Target.IsAttacking = true;

                var won =
                    _rnd.NextDouble() <= (double)tribe.Population / (tribe.Population + a.Target.Population);

                if (won)
                {
                    tribe.Population += 20;
                    a.Target.Population -= 20;

                    if (a.Target.Population <= 0)
                    {
                        WipeTribe(a.Target, iteration);
                    }
                }
                else
                {
                    tribe.Population -= 20;
                    a.Target.Population += 20;
                }
            } 
            else if (move is SuicideGameAction)
            {
                tribe.Population = 0;
            }

            // remove losers
            if (tribe.Population <= 0)
            {
                WipeTribe(tribe, iteration);
            }

            if (_tribes.Count(t => t.Population > 0) == 1) // when only one tribe is left alive
            {
                Finished = true;
                Winner = _tribes.First(t => t.Population > 0);
            }

            return Finished;
        }

        public IWorld WithTribe(string tribeName, Color color, ITribeBehaviour behaviour)
        {
            var coord = NextEmptyRandomTile();

            var tribe = new Tribe(tribeName, coord, color, behaviour);
            State[coord.x, coord.y] = tribe;

            _tribes.Add(tribe);

            return this;
        }

        public IWorldState GetWorldState(ITribe tribe)
        {
            var pos = tribe.Position;

            var result = new IWorldEntity[1 + 2 * WORLD_STATE_SIZE, 1 + 2 * WORLD_STATE_SIZE];

            var ws_y = 0;

            for (var y = pos.y - WORLD_STATE_SIZE; y <= pos.y + WORLD_STATE_SIZE; y++)
            {
                var ws_x = 0;

                for (var x = pos.x - WORLD_STATE_SIZE; x <= pos.x + WORLD_STATE_SIZE; x++)
                {
                    if (x >= 0 && y >= 0 && x < Size && y < Size)
                    {
                        result[ws_x, ws_y] = State[x, y];
                    }

                    ws_x++;
                }

                ws_y++;
            }

            return new WorldState(result);
        }

        public IEnumerable<ITribe> GetAliveTribes()
        {
            return _tribes.FindAll(t => t.Population > 0);
        }

        private void InitCollectable(int howMany, Func<IRandom, ICollectableWorldEntity> fn)
        {
            var left = howMany;

            while (left > 0)
            {
                var (x, y) = NextEmptyRandomTile();

                _state[x, y] = fn(_rnd);

                left--;
            }
        }

        private void WipeTribe(ITribe tribe, int iteration)
        {
            State[tribe.Position.x, tribe.Position.y] = null;
            tribe.DeadAtIteration = iteration;
        }

        private bool CanMove((int x, int y) pos, Directions direction)
        {
            return direction switch
            {
                Directions.N => pos.y > 0,
                Directions.S => pos.y < Size - 1,
                Directions.W => pos.x > 0,
                Directions.E => pos.x < Size - 1,
                Directions.NW => pos.x > 0 && pos.y > 0,
                Directions.SE => pos.x < Size - 1 && pos.y < Size - 1,
                Directions.NE => pos.x < Size - 1 && pos.y > 0,
                Directions.SW => pos.x > 0 && pos.y < Size - 1,
                _ => true,
            };
        }

        private void Move(ITribe tribe, Directions direction)
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

            State[x, y] = null;

            if (State[tribe.Position.x, tribe.Position.y] is Food)
            {
                tribe.Population += (State[tribe.Position.x, tribe.Position.y] as Food).Value;
            }

            // TODO: add logic to handle iron and wood

            State[tribe.Position.x, tribe.Position.y] = tribe;
        }

        // return an empty random tile
        private (int x, int y) NextEmptyRandomTile()
        {
            (int x, int y) result;

            var found = false;

            do
            {
                result = NextRandomTile(_rnd, Size);

                if (State[result.x, result.y] == null)
                { // found an empty tile
                    found = true;
                }

            } while (!found);

            return result;
        }

        // returns a random tile. NB: it's not guaranteed to be empty
        private static (int x, int y) NextRandomTile(IRandom r, int size) => (r.Next(size), r.Next(size));
    }
}
