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
    public class RandomWorld : BaseWorld
    {
        public RandomWorld(int size, WorldResources resources, IRandom rnd) : base(size, rnd)
        {
            if (resources is null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            if ((resources.FoodCount + resources.WoodCount + resources.IronCount) > size * size)
                throw new ArgumentOutOfRangeException(nameof(resources), "Too many collectables requested.");

            InitCollectable(resources.FoodCount, Collectables.Food);
            InitCollectable(resources.WoodCount, Collectables.Wood);
            InitCollectable(resources.IronCount, Collectables.Iron);
        }

        // for unit-testing purpose only
        internal RandomWorld(int size, WorldResources resources, IRandom rnd, IEnumerable<ITribe> tribes) 
            : this(size, resources, rnd)
        {
            _tribes = tribes.ToList();
        }

        public override IWorld AddTribe(string tribeName, Color color)
        {
            var coord = NextEmptyRandomTile();

            var tribe = new Tribe(tribeName, coord, color);
            State[coord.x, coord.y] = tribe;

            _tribes.Add(tribe);

            return this;
        }

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
