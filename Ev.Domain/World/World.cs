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
    public class WorldResources 
    {
        public int FoodCount { get; set; }
        public int WoodCount { get; set; }
        public int IronCount { get; set; }
    }

    public class World : IWorld
    {
        public IWorldEntity[,] State => _state;

        public int Size => _size;

        public ITribe[] Tribes => _tribes.ToArray();

        public bool Finished { get; private set; }

        public ITribe Winner { get; private set; }

        private readonly IWorldEntity[,] _state;
        private readonly IRandom _rnd;
        private readonly int _size;

        private readonly List<ITribe> _tribes = new List<ITribe>();

        public World(int size, WorldResources resources, IRandom rnd)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive value.");

            if (resources is null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            if ((resources.FoodCount + resources.WoodCount + resources.IronCount) > size * size)
                throw new ArgumentOutOfRangeException(nameof(resources), "Too many collectables requested.");

            _state = new IWorldEntity[size, size];
            _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));
            _size = size;

            InitCollectable(resources.FoodCount, (rnd) => new Food(rnd));
            InitCollectable(resources.WoodCount, (rnd) => new Wood(rnd));
            InitCollectable(resources.IronCount, (rnd) => new Iron(rnd));
        }

        // for unit-testing purpose only
        internal World(int size, WorldResources resources, IRandom rnd, IEnumerable<ITribe> tribes) 
            : this(size, resources, rnd)
        {
            _tribes = tribes.ToList();
        }

        public bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            if (move is null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (actionProcessor is null)
            {
                throw new ArgumentNullException(nameof(actionProcessor));
            }

            actionProcessor.Update(move, tribe, this, iteration);

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

            _tribes.ForEach(t => t.IsAttacking = false);

            return Finished;
        }

        public void WipeTribe(ITribe tribe, int iteration)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            State[tribe.Position.x, tribe.Position.y] = null;
            tribe.DeadAtIteration = iteration;
        }

        public IWorld WithTribe(string tribeName, Color color, ITribeBehaviour behaviour)
        {
            if (behaviour is null)
            {
                throw new ArgumentNullException(nameof(behaviour));
            }

            var coord = NextEmptyRandomTile();

            var tribe = new Tribe(tribeName, coord, color, behaviour);
            State[coord.x, coord.y] = tribe;

            _tribes.Add(tribe);

            return this;
        }

        // TODO: unit test
        public IWorldState GetWorldState(ITribe tribe)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var pos = tribe.Position;

            var result = new IWorldEntity[1 + 2 * WorldState.WORLD_STATE_SIZE, 1 + 2 * WorldState.WORLD_STATE_SIZE];

            var ws_y = 0;

            for (var y = pos.y - WorldState.WORLD_STATE_SIZE; y <= pos.y + WorldState.WORLD_STATE_SIZE; y++)
            {
                var ws_x = 0;

                for (var x = pos.x - WorldState.WORLD_STATE_SIZE; x <= pos.x + WorldState.WORLD_STATE_SIZE; x++)
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

        public ITribe[] GetAliveTribes()
        {
            return _tribes.FindAll(t => t.Population > 0).ToArray();
        }

        bool IWorld.CanMove((int x, int y) position, Direction direction)
        {
            return CanMove(position, direction);
        }

        void IWorld.Move(ITribe tribe, Direction direction)
        {
            Move(tribe, direction);
        }

        #region Internal members

        // TODO: unit test
        internal bool CanMove((int x, int y) pos, Direction direction) => direction switch
        {
            Direction.N => pos.y > 0,
            Direction.S => pos.y < Size - 1,
            Direction.W => pos.x > 0,
            Direction.E => pos.x < Size - 1,
            Direction.NW => pos.x > 0 && pos.y > 0,
            Direction.SE => pos.x < Size - 1 && pos.y < Size - 1,
            Direction.NE => pos.x < Size - 1 && pos.y > 0,
            Direction.SW => pos.x > 0 && pos.y < Size - 1,
            _ => false,
        };

        // TODO: unit test
        internal void Move(ITribe tribe, Direction direction)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var (x, y) = tribe.Position;

            switch (direction)
            {
                case Direction.N:
                    tribe.Position = (tribe.Position.x, tribe.Position.y - 1);
                    break;

                case Direction.S:
                    tribe.Position = (tribe.Position.x, tribe.Position.y + 1);
                    break;

                case Direction.W:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y);
                    break;

                case Direction.E:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y);
                    break;

                case Direction.NW:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y - 1);
                    break;

                case Direction.SE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y + 1);
                    break;

                case Direction.NE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y - 1);
                    break;

                case Direction.SW:
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

        #endregion

        #region Private members

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

        #endregion
    }
}
