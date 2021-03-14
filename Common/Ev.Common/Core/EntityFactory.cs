using Ev.Common.Core.Interfaces;

namespace Ev.Common.Core
{
    public static class EntityFactory
    {
        public static IBlockingWorldEntity    Wall()            => new BlockingWorldEntity    { Type = BlockingWorldEntityType.Wall };
        public static IBlockingWorldEntity    Water()           => new BlockingWorldEntity    { Type = BlockingWorldEntityType.Water };
        public static IBlockingWorldEntity    NotReachable()    => new BlockingWorldEntity    { Type = BlockingWorldEntityType.NotReachable };

        public static ICollectableWorldEntity Food(IRandom rnd) => new CollectableWorldEntity { Type = CollectableWorldEntityType.Food, Value = Rand(rnd, 9) };
        public static ICollectableWorldEntity Wood(IRandom rnd) => new CollectableWorldEntity { Type = CollectableWorldEntityType.Wood, Value = Rand(rnd, 9) };
        public static ICollectableWorldEntity Iron(IRandom rnd) => new CollectableWorldEntity { Type = CollectableWorldEntityType.Iron, Value = Rand(rnd, 9) };

        private static int Rand(IRandom rnd, int maxValue) => rnd.Next(1, maxValue + 1);
    }
}