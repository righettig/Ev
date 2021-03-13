using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Actions
{
    public class UpgradeAttackAction : BlockingGameAction
    {
        public override string Result() => "+.1 Attack";

        public override string ToString() => "UpgradeAttack";

        internal override void OnComplete(ITribe tribe)
        {
            tribe.Wood   -= GameParams.Instance.UpgradeActionsCost.WoodCount;
            tribe.Iron   -= GameParams.Instance.UpgradeActionsCost.IronCount;

            tribe.Attack += GameParams.Instance.UpgradeBonus;
        }
    }
}