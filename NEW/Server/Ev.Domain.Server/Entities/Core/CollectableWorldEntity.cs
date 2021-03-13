using Ev.Common.Utils;

namespace Ev.Domain.Server.Entities.Core
{
    public abstract class CollectableWorldEntity : ICollectableWorldEntity
    {
        public int Value { get; protected set; }

        public Color Color { get; init; }

        public CollectableWorldEntity(IRandom rnd, int maxValue)
        {
            Value = rnd.Next(1, maxValue + 1);
        }

        //public IWorldEntity ToImmutable()
        //{
        //    return this; // CollectableWorldEntity are inherently immutable
        //}
    }
}