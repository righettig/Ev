namespace Ev.Domain.Behaviours.BehaviourTrees.Core
{
    public interface IBehaviourTreeNode
    {
        NodeResult State { get; }

        NodeResult Tick(IBehaviourTreeContext context);

        void Reset();
    }

    // TODO: Random composite node, e.g. Random((n1,.2), (n2,.4), (n3, .4))
    // TODO: Random sequences/selectors work identically to their namesakes, except the actual order the child nodes are processed is determined randomly.
    // TODO: Succeeder
    // TODO: Nonreactive Sequence Node

    // lazy load behaviour tree
    // max nodes, pruning nodes that are "no longer relevant"
}
