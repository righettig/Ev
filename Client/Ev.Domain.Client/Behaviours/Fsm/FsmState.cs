using Ev.Common.Core;
using Ev.Domain.Client.Core;
using System;

namespace Ev.Domain.Client.Behaviours.Fsm
{
    public class FsmState 
    {
        public Enumeration Id { get; }

        public Func<IWorldState, ITribe, IGameAction> ActionFn { get; init; }

        public Func<IWorldState, ITribe, Enumeration> TransitionFn { get; init; }

        public FsmState(Enumeration id)
        {
            Id = id;
        }

        public override string ToString() => Id.ToString();
    }
}