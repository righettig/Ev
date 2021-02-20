using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using System;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor :
        IGameActionProcessor,
        IGameActionProcessor<HoldAction>,
        IGameActionProcessor<MoveAction>,
        IGameActionProcessor<AttackAction>,
        IGameActionProcessor<UpgradeDefensesAction>
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
                case HoldAction h:   Update(h, tribe, world, iteration); break;
                case MoveAction m:   Update(m, tribe, world, iteration); break;
                case AttackAction a: Update(a, tribe, world, iteration); break;
                case UpgradeDefensesAction ad: Update(ad, tribe, world, iteration); break;
            }
        }
    }
}