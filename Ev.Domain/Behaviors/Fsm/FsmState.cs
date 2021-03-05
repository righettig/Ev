using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;

namespace Ev.Domain.Behaviours.Fsm
{
    public class FsmState 
    {
        public Enumeration Id { get; }

        public Func<IWorldState, ITribeState, IGameAction> ActionFn { get; init; }

        public Func<IWorldState, ITribeState, Enumeration> TransitionFn { get; set; }

        public FsmState(Enumeration id)
        {
            Id = id;
        }

        public override string ToString() => Id.ToString();
    }
}