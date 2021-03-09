using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
{
    public class CollectableWorldEntity : ICollectableWorldEntity
    {
        public CollectableWorldEntityType Type { get; init; }

        public int Value { get; init; }
        
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
