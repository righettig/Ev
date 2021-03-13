using Ev.Common.Utils;

namespace Ev.Domain.Server.Entities.Core
{
    public abstract class CollectableWorldEntity : ICollectableWorldEntity
    {
        public int Value { get; }

        protected CollectableWorldEntity(IRandom rnd, int maxValue)
        {
            Value = rnd.Next(1, maxValue + 1);
        }
    }
}