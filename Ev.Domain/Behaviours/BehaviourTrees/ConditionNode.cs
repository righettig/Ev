using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using System;

namespace Ev.Domain.Behaviours.BehaviourTrees
{
    public class ConditionNode : IBehaviourTreeNode
    {
        private readonly Func<IWorldState, ITribeState, bool> _predicate;

        private NodeResult _state = NodeResult.NotStarted;

        public NodeResult State => _state;

        public ConditionNode(Func<IWorldState, ITribeState, bool> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public NodeResult Tick(IBehaviourTreeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (_predicate(context.WorldState, context.TribeState))
            {
                _state = NodeResult.Success;
            }
            else
            {
                _state = NodeResult.Failed;
            }

            return _state;
        }

        public void Reset()
        {
            _state = NodeResult.NotStarted;
        }
    }
}