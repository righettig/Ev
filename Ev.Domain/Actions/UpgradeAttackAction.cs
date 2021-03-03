using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions
{
    public class UpgradeAttackAction : BlockingGameAction
    {
        public override string Result() => "+.1 Attack";

        public override string ToString() => "UpgradeAttack";

        internal void OnComplete(ITribe tribe)
        {
            tribe.Wood -= 10;
            tribe.Iron -= 5;

            tribe.Attack += .1f;
        }
    }
}