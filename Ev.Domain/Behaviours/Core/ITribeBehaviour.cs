using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;

namespace Ev.Domain.Behaviours.Core
{
    public interface ITribeBehaviour 
    {
        public IWorldState State { get; set; }

        IGameAction DoMove(IWorldState state, ITribeState tribe);
    }
}