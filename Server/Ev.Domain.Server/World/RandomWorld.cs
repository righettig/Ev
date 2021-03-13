using Ev.Common.Utils;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Collectables;
using Ev.Domain.Server.Entities.Core;
using Ev.Domain.Server.World.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ev.Domain.Server.World
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

            InitCollectable(resources.FoodCount, rnd => new Food(rnd));
            InitCollectable(resources.WoodCount, rnd => new Wood(rnd));
            InitCollectable(resources.IronCount, rnd => new Iron(rnd));
        }

        // for unit-testing purpose only
        //internal RandomWorld(int size, WorldResources resources, IRandom rnd, IEnumerable<ITribe> tribes) 
        public RandomWorld(int size, WorldResources resources, IRandom rnd, IEnumerable<ITribe> tribes)
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
