using Ev.Domain.Behaviours.BehaviourTrees;
using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using Moq;

namespace Ev.Domain.Tests.Unit.BehaviourTrees
{
    static class Helpers
    {
        public static IBehaviourTreeContext CreateMockContext()
        {
            return new Mock<IBehaviourTreeContext>().Object;
        }

        public static IBehaviourTreeContext CreateContext()
        {
            return new BehaviourTreeContext(new Mock<IWorldState>().Object, new Mock<ITribeState>().Object);
        }

        public static IBehaviourTreeNode SucceedingTreeNode() => CreateBehaviourTreeNode(NodeResult.Success);

        public static IBehaviourTreeNode FailingTreeNode() => CreateBehaviourTreeNode(NodeResult.Failed);

        public static IBehaviourTreeNode RunningTreeNode() => CreateBehaviourTreeNode(NodeResult.Running);

        private static IBehaviourTreeNode CreateBehaviourTreeNode(NodeResult result)
        {
            var node = new Mock<IBehaviourTreeNode>();

            node
                .Setup(m => m.Tick(It.IsAny<IBehaviourTreeContext>()))
                .Returns(result);

            return node.Object;
        }
    }
}
