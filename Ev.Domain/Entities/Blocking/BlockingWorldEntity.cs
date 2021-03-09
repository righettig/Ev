using System;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Entities.Blocking
{
    public static class Blocking
    {
        public static IBlockingWorldEntity Wall()         => new BlockingWorldEntity { Type = BlockingWorldEntityType.Wall };
        public static IBlockingWorldEntity Water()        => new BlockingWorldEntity { Type = BlockingWorldEntityType.Water };
        public static IBlockingWorldEntity NotReachable() => new BlockingWorldEntity { Type = BlockingWorldEntityType.NotReachable };
    }

    public class BlockingWorldEntity : IBlockingWorldEntity
    {
        public BlockingWorldEntityType Type { get; init; }

        public BlockingWorldEntity(string type)
        {
            Type = Enum.Parse<BlockingWorldEntityType>(type);
        }

        public BlockingWorldEntity()
        {
        }

        public IWorldEntity ToImmutable()
        {
            return this;
        }
    }
}