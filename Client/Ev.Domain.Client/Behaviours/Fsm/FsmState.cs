using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using System;

namespace Ev.Domain.Client.Behaviours.Fsm
{
    public class FsmState 
    {
        public IEnumeration Id { get; }

        public Func<IWorldState, ITribe, IGameAction> ActionFn { get; init; }

        public Func<IWorldState, ITribe, IEnumeration> TransitionFn { get; init; }

        public FsmState(IEnumeration id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public override string ToString() => Id.ToString();
    }
}