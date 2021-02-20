using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public const int WIN_GAIN = 20;
        public const int DEFEAT_LOSS = 20;

        public void Update(AttackAction action, ITribe tribe, IWorld world, int iteration)
        {
            tribe.IsAttacking = true;
            action.Target.IsAttacking = true; // TODO: introduce IsDefending ?

            if (_predictor.CanWin(tribe, action.Target))
            {
                tribe.Population += WIN_GAIN;
                action.Target.Population -= DEFEAT_LOSS;

                if (action.Target.Population <= 0)
                {
                    world.WipeTribe(action.Target, iteration);
                }
            }
            else
            {
                tribe.Population -= DEFEAT_LOSS;
                action.Target.Population += WIN_GAIN;
            }
        }
    }
}