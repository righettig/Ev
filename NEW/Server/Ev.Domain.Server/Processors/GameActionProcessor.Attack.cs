using System.Diagnostics;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;

namespace Ev.Domain.Server.Processors
{
    public partial class GameActionProcessor
    {
        public static readonly int WIN_GAIN    = GameParams.Instance.WinGain;
        public static readonly int DEFEAT_LOSS = GameParams.Instance.DefeatLoss;

        public void Update(AttackAction action, ITribe tribe, IWorld world, int iteration)
        {
            Debug.Assert(action != null);
            Debug.Assert(tribe != null);
            Debug.Assert(world != null);
            Debug.Assert(iteration >= 0);

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