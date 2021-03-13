using System;

namespace Ev.Domain.Server.Core
{
    public interface IWorldState
    {
        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);
    }
}
