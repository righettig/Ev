using Ev.Common.Core.Interfaces;

namespace Ev.Common.Core
{
    public enum CollectableWorldEntityType
    {
        Food,
        Wood,
        Iron,
    }

    public class CollectableWorldEntity : ICollectableWorldEntity
    {
        public CollectableWorldEntityType Type { get; init; }

        public int Value { get; init; }
    }
}