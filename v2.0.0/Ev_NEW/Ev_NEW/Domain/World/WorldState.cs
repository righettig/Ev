using System;
using System.Linq;

namespace Ev_NEW
{
    public sealed class WorldState : IWorldState
    {
        public const int WORLD_STATE_SIZE = 2;

        private IWorldEntity[,] State { get; }

        public WorldState(IWorldEntity[,] state)
        {
            State = state;
        }

        public T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity
        {
            return State[entityPos.x, entityPos.y] as T;
        }

        public void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true)
        {
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

        public ICollectable[] GetCollectables(CollectableType? type = null)
        {
            if (type is null) 
            {
                return State.OfType<ICollectable>().ToArray();
            }

            return State.OfType<ICollectable>().Where(el => el.Type == type).ToArray();
        }

        // all tribes except "self"
        public ITribe[] GetTribes()
        {
            return State.OfType<ITribe>().Where(el => el.Position != (2, 2)).ToArray();
        }

        public IBlocking[] GetBlockings()
        {
            return State.OfType<IBlocking>().ToArray();
        }

        public IWorldEntity Closest()
        {
            return null;
        }

        public IWorldEntity Closest(ITribe tribe, params IWorldEntity[] entities) 
        {
            // at least 1 in entities

            return null;
        }

        public T Closest<T>() where T : class, IWorldEntity
        {
            throw new NotImplementedException();
        }

        public T Closest<T>(ITribe tribe, params T[] entities) where T : class, IWorldEntity
        {
            // if entities is empty search in State
            // otherwise return the closest among entities

            return entities[0];
        }
    }
}