using Ev.Domain.Entities.Core;
using System;

namespace Ev.Domain.World.Core
{
    public interface IWorldState
    {
        IWorldEntity[,] State { get; }

        T GetEntity<T>((int x, int y) entityPos) where T : class;

        void Traverse(Action<IWorldEntity, int, int> fn);
    }
}