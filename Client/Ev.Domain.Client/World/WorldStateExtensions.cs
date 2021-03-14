using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using System;
using System.Linq;

namespace Ev.Domain.Client.World
{
    public static class WorldStateExtensions
    {
        //public static ICollectable[] GetCollectables(this WorldState state, CollectableType? type = null)
        //{
        //    if (type is null)
        //    {
        //        return State.OfType<ICollectable>().ToArray();
        //    }

        //    return State.OfType<ICollectable>().Where(el => el.Type == type).ToArray();
        //}

        /// <summary>
        /// Returns all the available collectable entities.
        /// </summary>
        /// <returns>An array of collectables.</returns>
        public static ICollectableWorldEntity[] GetCollectables(this IWorldState state)
        {
            return state.State.OfType<ICollectableWorldEntity>().ToArray();
        }

        public static ITribe[] GetTribes(this IWorldState state)
        {
            return state.State.OfType<ITribe>().Where(
                el => el.Position != (WorldState.WORLD_STATE_SIZE, WorldState.WORLD_STATE_SIZE)).ToArray();
        }

        /// <summary>
        /// Returns all the available blocking entities.
        /// </summary>
        /// <returns>An array of blocking entities.</returns>
        public static IBlockingWorldEntity[] GetBlockings(this IWorldState state)
        {
            return state.State.OfType<IBlockingWorldEntity>().ToArray();
        }

        /// <summary>
        /// Gets the closest entity.
        /// </summary>
        /// <returns>The closest world entity to the center of the world state, i.e. the (2,2) position.</returns>
        public static IWorldEntity Closest(this IWorldState state)
        {
            return FindClosest<IWorldEntity>(state, el => el != null); // TODO: also check if el != Blocking.NotReachable
        }

        /// <summary>
        /// Gets the closest entity among those provided as parameters. 
        /// </summary>
        /// <param name="entities">The world entities to check.</param>
        /// <returns>The closest world entity.</returns>
        public static IWorldEntity Closest(this IWorldState state, params IWorldEntity[] entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            return FindClosest<IWorldEntity>(state, entities.Contains);
        }

        /// <summary>
        /// Gets the closest entity of the given type.
        /// </summary>
        /// <typeparam name="T">The type which is used to filter the world entities.</typeparam>
        /// <returns>The closest world entity to the center of the world state, i.e. the (2,2) position.</returns>
        public static T Closest<T>(this IWorldState state) where T : class, IWorldEntity
        {
            return FindClosest<T>(state, el => el is T);
        }

        /// <summary>
        /// Gets the closest entity among those provided as parameters. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The world entities to check.</param>
        /// <returns>The closest world entity.</returns>
        public static T Closest<T>(this IWorldState state, params T[] entities) where T : class, IWorldEntity
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            return FindClosest<T>(state, entities.Contains);
        }

        /// <summary>
        /// Returns the closest entity that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fn">The predicate which is used to filter the world entities.</param>
        /// <returns>The closest entity or null if none of the entities satisfy the predicate.</returns>
        private static T FindClosest<T>(this IWorldState state, Predicate<T> fn) where T : class, IWorldEntity
        {
            if (fn is null)
            {
                throw new ArgumentNullException(nameof(fn));
            }

            for (var i = 0; i < traverseList.Length; i++)
            {
                for (var j = 0; j < traverseList[i].GetLength(0); j++)
                {
                    var x = traverseList[i][j, 0];
                    var y = traverseList[i][j, 1];

                    var el = state.State[x, y];

                    if (fn(el as T))
                    {
                        return el as T;
                    }
                }
            }

            return null;
        }

        // Traverse list (statically computed)
        //------------------------------------
        // 0 0 0 0 0 
        // 0 0 0 1 0 
        // 0 0 P 0 0
        // 0 0 0 0 0 
        // 0 0 0 0 2 

        // d=1 => [1,1] [2,1] [3,1] [3,2] [3,3] [2,3] [1.3] [1,2]
        // d=2 => [0,0] [1,0] [2,0] [3,0] [4,0] [4,1] [4,2] [4,3] [4,4] [3,4] [2,4] [1,4] [0,4] [0,3] [0,2] [0,1]

        private static readonly int[][,] traverseList = new int[2][,]
        {
            new[,] {
                { 1, 1 }, { 2, 1 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 2, 3 }, { 1, 3 }, { 1, 2 }
            },
            new[,] {
                { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 3, 4 }, { 2, 4 }, { 1, 4 }, { 0, 4 }, { 0, 3 }, {0, 2 }, { 0, 1 }
            }
        };
    }
}