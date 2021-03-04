using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class EngineerTribeBehaviour : TribeBehaviour 
    {
        public EngineerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
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