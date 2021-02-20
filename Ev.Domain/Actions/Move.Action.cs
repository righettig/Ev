using Ev.Domain.Actions.Core;

namespace Ev.Domain.Actions
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

    public class MoveAction : GameAction
    {
        public Direction Direction { get; }

        // unit test assignment
        public MoveAction(Direction direction) => Direction = direction;

        // unit test this
        public override string ToString() => "Move " + Direction;
    }
}