using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions
{
    public class UpgradeDefensesAction : BlockingGameAction 
    {
        public override string Result() => "+.1 Defense";
        
        public override string ToString() => "UpgradeDefenses";

        internal override void OnComplete(ITribe tribe)
        {
            tribe.Wood -= GameParams.Instance.UpgradeActionsCost.WoodCount;
            tribe.Iron -= GameParams.Instance.UpgradeActionsCost.IronCount;

            tribe.Defense += GameParams.Instance.UpgradeBonus;
        }
    }
}