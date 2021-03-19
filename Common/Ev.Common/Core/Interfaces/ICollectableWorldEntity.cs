namespace Ev.Common.Core.Interfaces
{
    public interface ICollectableWorldEntity : IWorldEntity 
    {
        CollectableWorldEntityType Type { get; }

        public int Value { get; }
    }
}
