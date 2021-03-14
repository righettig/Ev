using Ev.Common.Core;
using Ev.Domain.Server.Actions.Core;

namespace Ev.Domain.Server.Actions
{
    public class MoveAction : GameAction
    {
        public Direction Direction { get; }

        public MoveAction(Direction direction) => Direction = direction;

        public override string ToString() => "Move " + Direction;
    }
}