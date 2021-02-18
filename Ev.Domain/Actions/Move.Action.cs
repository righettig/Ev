using Ev.Domain.Actions.Core;

namespace Ev.Domain.Actions
{
    public enum Directions
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
        public Directions Direction { get; }

        // unit test assignment
        public MoveAction(Directions direction) => Direction = direction;

        // unit test this
        public override string ToString() => "Move " + Direction;
    }
}