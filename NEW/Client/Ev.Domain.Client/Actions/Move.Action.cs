using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Actions
{
    public enum Direction
    {
        N,
        S,
        E,
        W,
        NE,
        NW,
        SE,
        SW
    }

    public class MoveAction : IGameAction
    {
        public Direction Direction { get; }

        public MoveAction(Direction direction) => Direction = direction;

        public override string ToString() => "Move " + Direction;
    }
}