using Ev.Domain.Behaviours.BehaviourTrees.Core;
using System;

namespace Ev.Domain.Behaviours.BehaviourTrees.Composite
{
    public abstract class CompositeBehaviourTreeNode : IBehaviourTreeNode
    {
        public NodeResult State => _state;

        protected NodeResult _state;
        protected readonly IBehaviourTreeNode[] _children;
        protected int _i;

        protected CompositeBehaviourTreeNode(params IBehaviourTreeNode[] children)
        {
            _children = children ?? throw new ArgumentNullException(nameof(children));
            _state = NodeResult.NotStarted;
        }

        public abstract NodeResult Tick(IBehaviourTreeContext context);

        public void Reset()
        {
            _i = 0;
            _state = NodeResult.NotStarted;
        }
    }
}
