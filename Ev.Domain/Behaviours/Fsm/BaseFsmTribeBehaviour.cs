using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Domain.Behaviours.Fsm
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

        public override IGameAction DoMove(IWorldState state, ITribeState tribe) => _fsm.DoMove(state, tribe);

        public override string DebugBehaviour() => _fsm.ToString();
    }
}