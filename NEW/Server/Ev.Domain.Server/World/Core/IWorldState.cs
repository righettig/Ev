using System;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.World.Core
{
    public interface IWorldState
    {
        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);
    }
}