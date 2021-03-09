using Ev.Domain.Entities.Core;

namespace Ev.Domain.Entities.Collectables
{
    public interface ICollectableWorldEntity : IWorldEntity 
    {
        CollectableWorldEntityType Type { get; }

        int Value { get; }
    }
}
