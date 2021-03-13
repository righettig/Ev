using System;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators
{
    public class Repeater : DecoratorBehaviourTreeNode
    {
        private readonly int _count;
        private int _i;

        public Repeater(IBehaviourTreeNode child, int count) : base(child)
        {
            if (count < 0) throw new ArgumentException("Count cannot be a negative value.", nameof(count));

            _count = count;
            _i = 0;
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            if (_state == NodeResult.Failed) return NodeResult.Failed;

            _state = ExecuteChild(context);

            return _state;
        }

        public override void Reset()
        {
            _i = 0;
            _state = NodeResult.NotStarted;
        }

        private NodeResult ExecuteChild(IBehaviourTreeContext context)
        {
            if (_i < _count)
            {
                var childResult = _child.Tick(context);
                _i++;

                if (childResult == NodeResult.Failed) return NodeResult.Failed;
            }

            return _i == _count ? NodeResult.Success : NodeResult.Running;
        }
    }
}