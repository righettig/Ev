using Ev.Domain.Actions;
using Ev.Domain.Behaviours.BehaviourTrees;
using Ev.Domain.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Behaviours.BehaviourTrees.Decorators;
using Ev.Domain.Utils;

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
                new Repeater(new GameActionNode((worldState, tribeState) => Move(direction)), count);

            return new Sequence(
                MoveTimes(Direction.E, 2), MoveTimes(Direction.N, 3));
        }
    }
}
