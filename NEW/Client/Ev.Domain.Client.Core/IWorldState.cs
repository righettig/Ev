using System;

namespace Ev.Domain.Client.Core
{
    // TODO: I could return IWorldEntity instead of ICollectableWorldEntity, IBlockingWorldEntity this way
    // I can remove those interfaces from this project

    public interface IWorldState
    {
        T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity;

        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);

        // TODO: I haven't introduced the enum in this branch yet
        //ICollectableWorldEntity[] GetCollectables(CollectableType? type = null);
        ICollectableWorldEntity[] GetCollectables();

        ITribe[] GetTribes(); // tribes except for "self"

        IBlockingWorldEntity[] GetBlockings();

        IWorldEntity Closest();

        IWorldEntity Closest(params IWorldEntity[] entities);

        T Closest<T>() where T : class, IWorldEntity;

        T Closest<T>(params T[] entities) where T : class, IWorldEntity;
    }
}