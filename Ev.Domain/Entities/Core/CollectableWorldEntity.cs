using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Core
{
    public abstract class CollectableWorldEntity : ICollectableWorldEntity
    {
        public int Value { get; protected set; }

        public CollectableWorldEntity(IRandom rnd, int maxValue)
        {
            Value = rnd.Next(1, maxValue + 1);
        }

        public CollectableWorldEntity()
        {
        }

        public IWorldEntity ToImmutable()
        {
            return this; // CollectableWorldEntity are inherently immutable
        }
    }
}