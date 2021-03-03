﻿using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions
{
    public class UpgradeDefensesAction : BlockingGameAction 
    {
        public override string Result() => "+.1 Defense";
        
        public override string ToString() => "UpgradeDefenses";

        internal void OnComplete(ITribe tribe)
        {
            tribe.Wood -= 10;
            tribe.Iron -= 5;

            tribe.Defense += .1f;
        }
    }
}