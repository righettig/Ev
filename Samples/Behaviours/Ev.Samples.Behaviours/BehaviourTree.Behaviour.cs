using Ev.Common;
using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators;

namespace Ev.Samples.Behaviours
{
    public class BtTribeBehaviour : BehaviourTreeTribeBehaviour
    {
        public BtTribeBehaviour(IRandom rnd) : base(rnd)
        {
        }

        protected override IBehaviourTreeNode CreateRoot()
        {
            IBehaviourTreeNode MoveTimes(Direction direction, int count) =>
                new Repeater(new GameActionNode((state, tribe) => Move(direction)), count);

            return new Sequence(
                MoveTimes(Direction.E, 2), MoveTimes(Direction.N, 3));
        }
    }
}
