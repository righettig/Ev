using Ev.Domain.Entities.Core;
using Ev.Domain.World;

namespace Ev.Domain.Actions.Processors
{
    public partial class GameActionProcessor
    {
        public void Update(AttackAction action, ITribe tribe, IWorld world, int iteration)
        {
            tribe.IsAttacking = true;
            action.Target.IsAttacking = true; // TODO: introduce IsDefending ?

            var won =
                _rnd.NextDouble() <= (double)tribe.Population / (tribe.Population + action.Target.Population);

            if (won)
            {
                tribe.Population += 20;
                action.Target.Population -= 20;

                if (action.Target.Population <= 0)
                {
                    world.WipeTribe(action.Target, iteration);
                }
            }
            else
            {
                tribe.Population -= 20;
                action.Target.Population += 20;
            }
        }
    }
}