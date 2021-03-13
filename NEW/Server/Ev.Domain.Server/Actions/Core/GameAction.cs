using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Actions.Core
{
    public abstract class GameAction : IGameAction
    {
        public ITribe Tribe { get; set; }
    }
}
