using System;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Actions
{
    public class AttackAction : IGameAction
    {
        public string TargetName { get; }

        public AttackAction(string target) => TargetName = target ?? throw new ArgumentNullException(nameof(target));
    }
}