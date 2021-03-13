using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Entities.Core
{
    public abstract class CollectableWorldEntity : ICollectableWorldEntity
    {
        public int Value { get; init; }
    }
}