using Ev.Domain.Entities.Collectables;

namespace Ev.Domain.Entities.Core
{
    public interface ICollectableWorldEntity : IWorldEntity 
    {
        CollectableWorldEntityType Type { get; }

        int Value { get; }
    }
}
