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

        public bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor)
        {
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

            return Finished;
        }

        public void WipeTribe(ITribe tribe, int iteration)
        {
            State[tribe.Position.x, tribe.Position.y] = null;
            tribe.DeadAtIteration = iteration;
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
