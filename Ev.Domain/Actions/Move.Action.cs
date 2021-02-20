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

        public MoveAction(Direction direction) => Direction = direction;

        public override string ToString() => "Move " + Direction;
    }
}