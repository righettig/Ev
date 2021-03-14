using Ev.Common.Core.Interfaces;

namespace Ev.Domain.Client.Entities.Core
{
    public abstract class CollectableWorldEntity : ICollectableWorldEntity
    {
        public int Value { get; init; }
    }
}