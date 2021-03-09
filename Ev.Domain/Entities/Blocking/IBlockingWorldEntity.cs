using Ev.Domain.Entities.Core;

namespace Ev.Domain.Entities.Blocking
{
    public interface IBlockingWorldEntity : IWorldEntity
    {
        BlockingWorldEntityType Type { get; }
    }
}