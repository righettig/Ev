using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using System;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators
{
    public abstract class DecoratorBehaviourTreeNode : IBehaviourTreeNode
    {
        protected readonly IBehaviourTreeNode _child;

        protected NodeResult _state = NodeResult.NotStarted;

        public NodeResult State => _state;

        protected DecoratorBehaviourTreeNode(IBehaviourTreeNode child)
        {
            _child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public abstract NodeResult Tick(IBehaviourTreeContext context);

        public abstract void Reset();
    }
}