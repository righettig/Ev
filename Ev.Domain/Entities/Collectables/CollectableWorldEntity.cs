using System;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
{
    public static class Collectables
    {
        public static ICollectableWorldEntity Food(IRandom rnd) => new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Food };
        public static ICollectableWorldEntity Wood(IRandom rnd) => new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Wood };
        public static ICollectableWorldEntity Iron(IRandom rnd) => new CollectableWorldEntity(rnd, 9) { Type = CollectableWorldEntityType.Iron };
    }

    public class CollectableWorldEntity : ICollectableWorldEntity
    {
        public CollectableWorldEntityType Type { get; init; }

        public int Value { get; }
        
        public CollectableWorldEntity(IRandom rnd, int maxValue)
        {
            Value = rnd.Next(1, maxValue + 1);
        }

        public CollectableWorldEntity(string type, int value)
        {
            Type = Enum.Parse<CollectableWorldEntityType>(type);
            Value = value;
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
