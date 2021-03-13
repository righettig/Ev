using Ev.Domain.Server.Actions.Core;

namespace Ev.Domain.Server.Actions
{
    public enum Direction // TODO: move to Ev.Domain.Core? Ev.Domain.Core? Ev.Common?
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