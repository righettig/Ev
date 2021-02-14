using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions
{
    public class AttackAction : GameAction
    {
        public ITribe Target { get; set; }

        public AttackAction(ITribe target) => Target = target;

        public override string ToString() => "Attack " + Target.Name;
    }
}