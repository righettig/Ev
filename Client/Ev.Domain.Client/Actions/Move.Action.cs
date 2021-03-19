using Ev.Common.Core;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Actions
{
    public class MoveAction : IGameAction
    {
        public Direction Direction { get; }

        public MoveAction(Direction direction) => Direction = direction;
    }
}