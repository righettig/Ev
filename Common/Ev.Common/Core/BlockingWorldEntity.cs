using Ev.Common.Core.Interfaces;

namespace Ev.Common.Core
{
    public enum BlockingWorldEntityType
    {
        NotReachable,
        Water,
        Wall,
    }

    public class BlockingWorldEntity : IBlockingWorldEntity
    {
        public BlockingWorldEntityType Type { get; init; }
    }
}