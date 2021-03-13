using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class EngineerTribeBehaviour : TribeBehaviour 
    {
        public EngineerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            if (tribe.Wood >= 10 && tribe.Iron >= 5) 
            {
                var p = _rnd.NextDouble();

                if (p <= 0.5) 
                {
                    return UpgradeAttack();

                } else 
                {
                    return UpgradeDefenses();
                }
            }

            return RandomWalk();
        }
    }
}