using Ev.Domain.Server.Core;
using System;

namespace Ev.Domain.Server.World
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
    }
}