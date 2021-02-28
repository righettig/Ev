namespace Ev_NEW
{
    public interface ICollectable : IWorldEntity
    {
        CollectableType Type { get; }
        int Value { get; }
    }

    public class Collectable : ICollectable
    {
        public CollectableType Type { get; init; }
        public int Value { get; init; }

        public Collectable(CollectableType type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}