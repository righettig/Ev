using System;

namespace Ev.Common.Core
{
    public interface IWorldState
    {
        IWorldEntity[,] State { get; }

        T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity;

        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);
    }
}
