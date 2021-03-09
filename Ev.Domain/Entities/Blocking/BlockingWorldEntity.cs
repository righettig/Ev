using Ev.Domain.Entities.Core;

namespace Ev.Domain.Entities.Blocking
{
    public class BlockingWorldEntity : IBlockingWorldEntity
    {
        public BlockingWorldEntityType Type { get; init; }

        public IWorldEntity ToImmutable()
        {
            return this;
        }
    }
}