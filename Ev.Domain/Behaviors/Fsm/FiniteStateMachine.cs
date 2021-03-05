using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System.Collections.Generic;

namespace Ev.Domain.Behaviours.Fsm
{
    public class FiniteStateMachine 
    {
        private Dictionary<string, FsmState> _states = new Dictionary<string, FsmState>();

        private FsmState _current;

        private string debug;

        public FiniteStateMachine WithState(FsmState state) 
        {
            _states.Add(state.Id.Name, state);

            return this;
        }

        public FiniteStateMachine WithInitialState(Enumeration stateId)
        {
            SetState(stateId);

            return this;
        }

        public IGameAction DoMove(IWorldState state, ITribeState tribe) 
        {
            var action = _current.ActionFn(state, tribe);

            debug = _current.ToString();
            SetState(_current.TransitionFn(state, tribe));
            debug += $" -> {_current}";

            return action;
        }

        private void SetState(Enumeration stateId)
        {
            _current = _states[stateId.Name];
        }

        public override string ToString() => debug;
    }
}