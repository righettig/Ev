using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions.Core
{
    public abstract class GameAction : IGameAction
    {
        ITribe IGameAction.Tribe { get; set; }
    }
}
