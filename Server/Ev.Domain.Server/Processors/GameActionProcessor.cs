using System;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;

namespace Ev.Domain.Server.Processors
{
    public partial class GameActionProcessor :
        IGameActionProcessor,
        IGameActionProcessor<HoldAction>,
        IGameActionProcessor<MoveAction>,
        IGameActionProcessor<AttackAction>,
        IGameActionProcessor<BlockingGameAction>
    {
        private readonly IAttackOutcomePredictor _predictor;

        public GameActionProcessor(IAttackOutcomePredictor predictor)
        {
            _predictor = predictor ?? throw new ArgumentNullException(nameof(predictor));
        }

        public void Update(IGameAction action, ITribe tribe, IWorld world, int iteration) 
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            if (world is null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            switch (action)
            {
                case HoldAction h:          Update(h,  tribe, world, iteration); break;
                case MoveAction m:          Update(m,  tribe, world, iteration); break;
                case AttackAction a:        Update((IGameAction) a,  tribe, world, iteration); break;
                case SuicideAction s:       Update(s,  tribe, world, iteration); break;
                case BlockingGameAction ba: Update((IGameAction) ba, tribe, world, iteration); break;
            }
        }
    }
}