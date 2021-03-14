using System;

namespace Ev.Common.Core
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
        /// <returns>The entity as an instance of T, or null it not found or if the wrong type is provided.</returns>
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
                    if ((x, y) != (WORLD_STATE_SIZE, WORLD_STATE_SIZE) || !ignoreSelf)
                    {
                        fn(State[x, y], x, y);
                    }
                }
            }
        }
    }
}