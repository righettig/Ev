using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators
{
    public class RepeatUntilFail : DecoratorBehaviourTreeNode
    {
        public RepeatUntilFail(IBehaviourTreeNode child) : base(child)
        {
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            if (_state == NodeResult.Failed) return NodeResult.Success;

            var result = _child.Tick(context);

            if (result == NodeResult.Failed)
            {
                _state = NodeResult.Success;
            }
            else
            {
                _state = NodeResult.Running;
            }

            return _state;
        }

        public override void Reset()
        {
            _state = NodeResult.NotStarted;
        }
    }
}