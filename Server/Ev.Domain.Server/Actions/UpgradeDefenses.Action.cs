using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Actions
{
    public class UpgradeDefensesAction : BlockingGameAction 
    {
        public override string Result() => "+.1 Defense";
        
        public override string ToString() => "UpgradeDefenses";

        public override void OnComplete(ITribe tribe)
        {
            tribe.Wood -= GameParams.Instance.UpgradeActionsCost.WoodCount;
            tribe.Iron -= GameParams.Instance.UpgradeActionsCost.IronCount;

            tribe.Defense += GameParams.Instance.UpgradeBonus;
        }
    }
}