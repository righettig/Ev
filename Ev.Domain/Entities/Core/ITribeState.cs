using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Core
{
    public interface ITribeState : IWorldEntity
    {
        string Name { get; }
        
        Color Color { get; }

        int Population { get; }

        (int x, int y) Position { get; }
        (int x, int y) PrevPosition { get; }

        bool StrongerThan(ITribeState other);
        bool WeakerThan(ITribeState other);

        #region Resources

        int Wood { get; }

        int Iron { get; }

        #endregion
    }
}