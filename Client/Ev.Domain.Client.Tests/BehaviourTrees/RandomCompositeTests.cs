using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class RandomCompositeTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Rnd_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new RandomComposite((IRandom) null, Stubs.IBehaviourTreeNode));
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Children_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new RandomComposite(Stubs.IRandom, null));
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Continue_Executing_Chosen_Child_Until_It_Finishes()
        {
            // Arrange
            var child1 = new Mock<IBehaviourTreeNode>();

            child1
                .SetupSequence(m => m.Tick(It.IsAny<IBehaviourTreeContext>()))
                .Returns(NodeResult.Running)
                .Returns(NodeResult.Running)
                .Returns(NodeResult.Success);

            var child2 = Stubs.IBehaviourTreeNode;

            var rnd = new Mock<IRandom>();
            rnd
                .Setup(m => m.Next(It.IsAny<int>()))
                .Returns(0); // always choose the first child;

            var node = new RandomComposite(rnd.Object, child1.Object, child2);

            var context = Stubs.IBehaviourTreeContext;

            // Act
            node.Tick(context);
            node.Tick(context);
            var actual = node.Tick(context);

            // Assert
            child1.Verify(m => m.Tick(It.IsAny<IBehaviourTreeContext>()), Times.Exactly(3));

            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_If_Child_Succeeds()
        {
            // Arrange
            var node = new RandomComposite(Helpers.SucceedingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Failure_If_Child_Fails()
        {
            // Arrange
            var node = new RandomComposite(Helpers.FailingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        [TestMethod]
        public void Tick_After_Success_Should_Return_Success()
        {
            // Arrange
            var node = new RandomComposite(Helpers.SucceedingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_After_Failure_Should_Return_Failure()
        {
            // Arrange
            var node = new RandomComposite(Helpers.FailingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset_After_Success_Should_Set_State_To_NotStarted()
        {
            // Arrange
            var node = new RandomComposite(Helpers.SucceedingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_After_Failure_Should_Set_State_To_NotStarted()
        {
            // Arrange
            var node = new RandomComposite(Helpers.FailingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
