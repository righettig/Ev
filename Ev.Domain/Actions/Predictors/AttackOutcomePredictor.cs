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

            return _rnd.NextDouble() <= (double)attacker.Population / (attacker.Population + defender.Population);
        }
    }
}