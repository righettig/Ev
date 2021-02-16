using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Core.Processors
{
    public partial class GameActionProcessor :
        IGameActionProcessor,
        IGameActionProcessor<HoldAction>,
        IGameActionProcessor<MoveAction>,
        IGameActionProcessor<AttackAction>
    {
        private readonly IRandom _rnd;

        public GameActionProcessor(IRandom rnd)
        {
            _rnd = rnd;
        }

        public void Update(IGameAction action, ITribe tribe, IWorld world, int iteration) 
        {
            switch (action) 
            { 
                case HoldAction h:   Update(h, tribe, world, iteration); break;
                case MoveAction m:   Update(m, tribe, world, iteration); break;
                case AttackAction a: Update(a, tribe, world, iteration); break;
            }
        }
    }
}