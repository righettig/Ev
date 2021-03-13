using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.Fsm;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class StateEnum : Enumeration
    {
        public static StateEnum Idle     = new(1, nameof(Idle));
        public static StateEnum SeekFood = new(2, nameof(SeekFood));
        public static StateEnum Flee     = new(3, nameof(Flee));

        private StateEnum(int id, string name) : base(id, name)
        {
        }
    }

    public class FsmTribeBehaviour : BaseFsmTribeBehaviour
    {
        public FsmTribeBehaviour(IRandom rnd) : base(rnd) { }

        protected override FiniteStateMachine BuildFsm()
        {
            return new FiniteStateMachine()

                .WithState(new FsmState(StateEnum.Idle)
                {
                    ActionFn = (state, tribe) => Hold(),
                    TransitionFn = (state, tribe) =>
                    {
                        if (tribe.Population < 40) return StateEnum.SeekFood;

                        if (state.Closest<ITribe>() != null)
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

                        if (state.Closest<ITribe>() != null)
                        {
                            return StateEnum.Flee;
                        }

                        return StateEnum.SeekFood;
                    }
                })

                .WithState(new FsmState(StateEnum.Flee)
                {
                    ActionFn = (state, tribe) => {
                        var enemy = state.Closest<ITribe>();

                        return MoveAwayFrom(enemy);
                    },
                    TransitionFn = (state, tribe) =>
                    {
                        if (tribe.Population < 40) return StateEnum.SeekFood;

                        if (state.Closest<ITribe>() is null)
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