using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using System;

namespace Ev.Domain.Server.Actions
{
    public class AttackAction : GameAction
    {
        // TODO: I need this until I can fully do the mapping in the platform layer
        public string TargetName { get; } // client-side

        public ITribe Target { get; set; }

        public AttackAction(string target) => TargetName = target ?? throw new ArgumentNullException(nameof(target));

        public override string ToString() => "Attack " + TargetName;
    }
}