using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using System;

namespace Ev.Domain.Actions.Core.Processors
{
    public class AttackOutcomePredictor : IAttackOutcomePredictor
    {
        private readonly IRandom _rnd;

        public AttackOutcomePredictor(IRandom rnd) => _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));

        public bool CanWin(ITribe attacker, ITribe defender)
        {
            if (attacker is null)
            {
                throw new ArgumentNullException(nameof(attacker));
            }

            if (defender is null)
            {
                throw new ArgumentNullException(nameof(defender));
            }

            // Example
            //--------
            // Attacker: 100, Defender = 100
            // 
            // Normal     = 100 / 200 => p = .5
            // +1 Attack  = 110 / 210 => p = .5238
            // +1 Defense = 100 / 210 => p = .4761

            return _rnd.NextDouble() <= (attacker.Population + (attacker.Population * attacker.Attack)) / (attacker.Population + (attacker.Population * attacker.Attack) + defender.Population + (defender.Population * defender.Defense));
        }
    }
}