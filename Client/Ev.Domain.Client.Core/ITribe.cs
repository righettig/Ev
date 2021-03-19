using Ev.Common.Core;
using Ev.Common.Core.Interfaces;

namespace Ev.Domain.Client.Core
{
    public interface ITribe : IWorldEntity
    {
        string Name { get; }

        Color Color { get; }

        int Population { get; }

        (int x, int y) Position { get; }
        (int x, int y) PrevPosition { get; }

        bool StrongerThan(ITribe other);
        bool WeakerThan(ITribe other);

        #region Resources

        int Wood { get; }

        int Iron { get; }

        #endregion
    }
}