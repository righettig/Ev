using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions.Core
{
    public abstract class GameAction : IGameAction
    {
        public ITribe Tribe { get; set; }
    }
}
