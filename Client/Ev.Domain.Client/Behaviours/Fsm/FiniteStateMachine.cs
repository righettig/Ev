using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using System;
using System.Collections.Generic;

namespace Ev.Domain.Client.Behaviours.Fsm
{
    public class FiniteStateMachine 
    {
        private readonly Dictionary<string, FsmState> _states = new();

        public FsmState Current { get; private set; }

        private string _debug;

        public FiniteStateMachine WithState(FsmState state) 
        {
            if (state == null)              throw new ArgumentNullException(nameof(state));
            if (state.ActionFn == null)     throw new InvalidFsmStateNotFoundException("Missing ActionFn.", nameof(state));
            if (state.TransitionFn == null) throw new InvalidFsmStateNotFoundException("Mission TransitionFn", nameof(state));

            _states.Add(state.Id.Name, state);

            return this;
        }

        public FiniteStateMachine WithInitialState(Enumeration stateId)
        {
            if (stateId == null) throw new ArgumentNullException(nameof(stateId));

            SetState(stateId);

            return this;
        }

        public IGameAction DoMove(IWorldState state, ITribe tribe) 
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (tribe == null) throw new ArgumentNullException(nameof(tribe));

            var action = Current.ActionFn(state, tribe);

            _debug = Current.ToString();
            
            SetState(Current.TransitionFn(state, tribe));

            _debug += $" -> {Current}";

            return action;
        }

        private void SetState(IEnumeration stateId)
        {
            try
            {
                Current = _states[stateId.Name];
            }
            catch (KeyNotFoundException)
            {
                throw new FsmStateNotFoundException();
            }
        }

        public override string ToString() => _debug;
    }

    public class FsmStateNotFoundException : Exception
    {
    }
    
    public class InvalidFsmStateNotFoundException : ArgumentException
    {
        public InvalidFsmStateNotFoundException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
