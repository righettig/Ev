using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;

namespace Ev.Domain.Behaviours.Core
{
    public interface ITribeBehaviour 
    {
        IGameAction DoMove(IWorldState state, ITribe tribe);
    }
}