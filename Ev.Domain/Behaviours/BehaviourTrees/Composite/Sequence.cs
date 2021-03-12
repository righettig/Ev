using Ev.Domain.Behaviours.BehaviourTrees.Core;

namespace Ev.Domain.Behaviours.BehaviourTrees.Composite
{
    public class Sequence : CompositeBehaviourTreeNode
    {
        public Sequence(params IBehaviourTreeNode[] children) : base(children)
        {
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            if (_state == NodeResult.Failed) return NodeResult.Failed;

            var result = _children[_i].Tick(context);

            if (result == NodeResult.Failed)
            {
                _state = NodeResult.Failed;

                return _state;
            }

            if (result == NodeResult.Success && _i < _children.Length)
            {
                _i++;
                _state = NodeResult.Running;

                return NodeResult.Running;
            }

            if (result == NodeResult.Running)
            {
                _state = NodeResult.Running;

                return NodeResult.Running;
            }

            _state = NodeResult.Success;
            return NodeResult.Success;
        }
    }
}