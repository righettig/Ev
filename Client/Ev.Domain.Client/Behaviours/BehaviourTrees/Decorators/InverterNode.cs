using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators
{
    public class InverterNode : DecoratorBehaviourTreeNode
    {
        public InverterNode(IBehaviourTreeNode child) : base(child)
        {
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            var result = _child.Tick(context);

            if (result == NodeResult.Running)
            {
                _state = NodeResult.Running;
            }
            else
            {
                _state = result == NodeResult.Success ? NodeResult.Failed : NodeResult.Success;
            }

            return _state;
        }

        public override void Reset()
        {
            _state = NodeResult.NotStarted;
        }
    }
}