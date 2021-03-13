using Ev.Domain.Client.Core;
using System;
using System.Linq;

namespace Ev.Domain.Client.World
{
    public sealed class WorldState : IWorldState
    {
        public const int WORLD_STATE_SIZE = 2;

        public IWorldEntity[,] State { get; }

        public WorldState(IWorldEntity[,] state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        /// <summary>
        /// Gets the entity at the given position.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entityPos">The world state position where the entity is.</param>
        /// <returns>The entity casted as an instance of T, or null it not found or if the wrong type is provided.</returns>
        public T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity
        {
            return State[entityPos.x, entityPos.y] as T;
        }

        /// <summary>
        /// Iterates through the world state applying the provided action to each cell.
        /// </summary>
        /// <param name="fn">The action to apply.</param>
        /// <param name="ignoreSelf">Whether to ignore or not the current tribe at position (2,2).</param>
        public void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true)
        {
            if (fn is null)
            {
                throw new ArgumentNullException(nameof(fn));
            }

            for (var x = 0; x < State.GetLength(0); x++)
            {
                for (var y = 0; y < State.GetLength(1); y++)
                {
                    if ((x, y) != (WORLD_STATE_SIZE, WORLD_STATE_SIZE) || !ignoreSelf) // ignore self
                    {
                        fn(State[x, y], x, y);
                    }
                }
            }
        }

        //public ICollectable[] GetCollectables(CollectableType? type = null)
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
        public ICollectableWorldEntity[] GetCollectables()
        {
            return State.OfType<ICollectableWorldEntity>().ToArray();
        }

        /// <summary>
        /// Returns all the available tribes with the exception of the one at position (2,2).
        /// </summary>
        /// <returns>An array of tribes.</returns>
        public ITribe[] GetTribes()
        {
            return State.OfType<ITribe>().Where(el => el.Position != (2, 2)).ToArray();
        }

        /// <summary>
        /// Returns all the available blocking entities.
        /// </summary>
        /// <returns>An array of blocking entities.</returns>
        public IBlockingWorldEntity[] GetBlockings()
        {
            return State.OfType<IBlockingWorldEntity>().ToArray();
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

        private readonly int[][,] traverseList = new int[2][,]
        {
            new int[,] {
                { 1, 1 }, { 2, 1 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 2, 3 }, { 1, 3 }, { 1, 2 }
            },
            new int[,] {
                { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 3, 4 }, { 2, 4 }, { 1, 4 }, { 0, 4 }, { 0, 3 }, {0, 2 }, { 0, 1 }
            }
        };

        /// <summary>
        /// Gets the closest entity.
        /// </summary>
        /// <returns>The closest world entity to the center of the world state, i.e. the (2,2) position.</returns>
        public IWorldEntity Closest()
        {
            return FindClosest<IWorldEntity>(el => el != null); // TODO: also check if el != Blocking.NotReachable
        }

        /// <summary>
        /// Gets the closest entity among those provided as parameters. 
        /// </summary>
        /// <param name="entities">The world entities to check.</param>
        /// <returns>The closest world entity.</returns>
        public IWorldEntity Closest(params IWorldEntity[] entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            return FindClosest<IWorldEntity>(el => entities.Contains(el));
        }

        /// <summary>
        /// Gets the closest entity of the given type.
        /// </summary>
        /// <typeparam name="T">The type which is used to filter the world entities.</typeparam>
        /// <returns>The closest world entity to the center of the world state, i.e. the (2,2) position.</returns>
        public T Closest<T>() where T : class, IWorldEntity
        {
            return FindClosest<T>(el => el is T);
        }

        /// <summary>
        /// Gets the closest entity among those provided as parameters. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The world entities to check.</param>
        /// <returns>The closest world entity.</returns>
        public T Closest<T>(params T[] entities) where T : class, IWorldEntity
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            return FindClosest<T>(el => entities.Contains(el));
        }

        /// <summary>
        /// Returns the closest entity that satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fn">The predicate which is used to filter the world entities.</param>
        /// <returns>The closest entity or null if none of the entities satisfy the predicate.</returns>
        private T FindClosest<T>(Predicate<T> fn) where T : class, IWorldEntity
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

                    var el = State[x, y];

                    if (fn(el as T))
                    {
                        return el as T;
                    }
                }
            }

            return null;
        }
    }
}