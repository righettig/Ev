using System;

namespace Ev.Domain.Server.Core
{
    public interface IWorldState
    {
        T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity;

        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);
    }
}
