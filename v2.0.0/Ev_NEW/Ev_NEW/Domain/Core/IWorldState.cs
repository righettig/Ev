using System;

namespace Ev_NEW
{
    // Improved client-side world state. I've added a bunch of utility methods that case make like easier
    // for behaviour developers.

    /// <summary>
    /// Client-side world state. This is a subset of the entire game world state.
    /// </summary>
    public interface IWorldState
    {
        T GetEntity<T>((int x, int y) entityPos) where T : class, IWorldEntity;

        #region NEW STUFF
        ICollectable[] GetCollectables(CollectableType? type = null);

        ITribe[] GetTribes(); // tribes except for "self"

        IBlocking[] GetBlockings();

        IWorldEntity Closest();

        IWorldEntity Closest(ITribe tribe, params IWorldEntity[] entities);

        T Closest<T>() where T : class, IWorldEntity;

        T Closest<T>(ITribe tribe, params T[] entities) where T : class, IWorldEntity;
        #endregion

        void Traverse(Action<IWorldEntity, int, int> fn, bool ignoreSelf = true);
    }
}