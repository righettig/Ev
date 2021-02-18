using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using System;

namespace Ev.Domain.Actions
{
    public class AttackAction : GameAction
    {
        public ITribe Target { get; init; }

        public AttackAction(ITribe target) => Target = target ?? throw new ArgumentNullException(nameof(target));

        public override string ToString() => "Attack " + Target.Name;
    }
}