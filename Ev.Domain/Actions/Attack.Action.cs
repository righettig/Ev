using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using System;

namespace Ev.Domain.Actions
{
    public class AttackAction : GameAction
    {
        public string TargetName { get; init; } // client-side

        public ITribe Target { get; set; } // server-side

        public AttackAction(string target) => TargetName = target ?? throw new ArgumentNullException(nameof(target));

        public override string ToString() => "Attack " + TargetName;
    }
}