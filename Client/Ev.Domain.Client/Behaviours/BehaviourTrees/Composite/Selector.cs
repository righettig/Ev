using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Composite
{
    public class Selector : CompositeBehaviourTreeNode
    {
        public Selector(params IBehaviourTreeNode[] children) : base(children)
        {
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            var result = _children[_i].Tick(context);

            if (result == NodeResult.Running)
            {
                _state = NodeResult.Running;
            }

            if (result == NodeResult.Success)
            {
                _state = NodeResult.Success;

                return _state;
            }

            _i++;

            if (_i == _children.Length)
            {
                _state = NodeResult.Failed;
            }
            else
            {
                _state = NodeResult.Running;
            }

            return _state;
        }
    }
}