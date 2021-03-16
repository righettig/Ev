using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using System;
using Random = Ev.Common.Core.Random;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees.Composite
{
    /// <summary>
    /// Randomly selects a node and and executes it until it either fails or succeeds.
    /// </summary>
    public class RandomComposite : CompositeBehaviourTreeNode
    {
        private readonly IRandom _rnd;

        public RandomComposite(IRandom rnd, params IBehaviourTreeNode[] children) : base(children)
        {
            _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));
        }

        public RandomComposite(params IBehaviourTreeNode[] children) : this(new Random(), children)
        {
        }

        public override NodeResult Tick(IBehaviourTreeContext context)
        {
            switch (_state)
            {
                case NodeResult.Failed:
                case NodeResult.Success:
                    return _state;

                case NodeResult.NotStarted:
                    _i = _rnd.Next(_children.Length);
                    break;
            }

            _state = _children[_i].Tick(context);

            return _state;
        }
    }
}