using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using System;

namespace Ev.Domain.World
{
    public class WorldState : IWorldState
    {
        public const int WORLD_STATE_SIZE = 2;

        public IWorldEntity[,] State { get; }

        public WorldState(IWorldEntity[,] state)
        {
            State = state;
        }

        public T GetEntity<T>((int x, int y) entityPos) where T : class
        {
            return State[entityPos.x, entityPos.y] as T;
        }

        public void Traverse(Action<IWorldEntity, int, int> fn)
        {
            for (var x = 0; x < State.GetLength(0); x++)
            {
                for (var y = 0; y < State.GetLength(1); y++)
                {
                    if ((x, y) != (WORLD_STATE_SIZE, WORLD_STATE_SIZE)) // ignore self
                    {
                        fn(State[x, y], x, y);
                    }
                }
            }
        }
    }
}