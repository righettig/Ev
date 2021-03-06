using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions
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