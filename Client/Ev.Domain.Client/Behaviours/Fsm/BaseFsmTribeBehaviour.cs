using Ev.Common.Core;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Behaviours.Fsm
{
    /// <summary>
    /// The base class for all Finite State Machine tribe behaviours.
    /// </summary>
    public abstract class BaseFsmTribeBehaviour : TribeBehaviour 
    {
        private readonly FiniteStateMachine _fsm;

        protected BaseFsmTribeBehaviour(IRandom rnd) : base(rnd) 
        {
            _fsm = BuildFsm();    
        }

        protected abstract FiniteStateMachine BuildFsm();

        public override IGameAction DoMove(IWorldState state, ITribe tribe) => _fsm.DoMove(state, tribe);

        public override string DebugBehaviour() => _fsm.ToString();
    }
}