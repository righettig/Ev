using Ev.Domain.Behaviours.Fsm;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Samples.Behaviours
{
    public class StateEnum : Enumeration
    {
        public static StateEnum Idle     = new StateEnum(1, nameof(Idle));
        public static StateEnum SeekFood = new StateEnum(2, nameof(SeekFood));
        public static StateEnum Flee     = new StateEnum(3, nameof(Flee));

        public StateEnum(int id, string name) : base(id, name)
        {
        }
    }

    public class FsmTribeBehaviour : BaseFsmTribeBehaviour
    {
        public FsmTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override FiniteStateMachine BuildFsm()
        {
            return new FiniteStateMachine()

                .WithState(new FsmState(StateEnum.Idle)
                {
                    ActionFn = (state, tribe) => Hold(),
                    TransitionFn = (state, tribe) =>
                    {
                        if (tribe.Population < 40) return StateEnum.SeekFood;

                        if (state.Closest<ITribeState>() != null)
                        {
                            return StateEnum.Flee;
                        }

                        return StateEnum.Idle;
                    }
                })

                .WithState(new FsmState(StateEnum.SeekFood)
                {
                    ActionFn = (state, tribe) => RandomWalk(),
                    TransitionFn = (state, tribe) =>
                    {
                        if (tribe.Population > 70) return StateEnum.Idle;

                        if (state.Closest<ITribeState>() != null)
                        {
                            return StateEnum.Flee;
                        }

                        return StateEnum.SeekFood;
                    }
                })

                .WithState(new FsmState(StateEnum.Flee)
                {
                    ActionFn = (state, tribe) => {
                        var enemy = state.Closest<ITribeState>();

                        return MoveAwayFrom(enemy);
                    },
                    TransitionFn = (state, tribe) =>
                    {
                        if (tribe.Population < 40) return StateEnum.SeekFood;

                        if (state.Closest<ITribeState>() is null)
                        {
                            return StateEnum.Idle;
                        }

                        return StateEnum.Flee;
                    }
                })

                .WithInitialState(StateEnum.Idle);
        }
    }
}