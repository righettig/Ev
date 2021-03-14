using Ev.Common.Core;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Core;
using System;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees
{
    public class GameActionNode : IBehaviourTreeNode
    {
        private readonly Func<IWorldState, ITribe, IGameAction> _behaviourFn;

        private NodeResult _state = NodeResult.NotStarted;

        public NodeResult State => _state;

        public GameActionNode(ITribeBehaviour behaviour)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            _behaviourFn = behaviour.DoMove;
        }

        public GameActionNode(Func<IWorldState, ITribe, IGameAction> behaviourFn)
        {
            _behaviourFn = behaviourFn ?? throw new ArgumentNullException(nameof(behaviourFn));
        }

        public NodeResult Tick(IBehaviourTreeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var move = _behaviourFn(context.WorldState, context.TribeState);

            context.Move = move;

            _state = NodeResult.Success;

            return _state;
        }

        public void Reset()
        {
            _state = NodeResult.NotStarted;
        }
    }
}