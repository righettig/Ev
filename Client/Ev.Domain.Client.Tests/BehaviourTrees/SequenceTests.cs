using Ev.Domain.Client.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class SequenceTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Children_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new Sequence(null));
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            // Arrange & Act
            var child1 = Stubs.IBehaviourTreeNode;
            var child2 = Stubs.IBehaviourTreeNode;

            var node = new Sequence(child1, child2);

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Running_If_First_Child_Succeeds_But_Evaluation_Not_Finished()
        {
            // Arrange
            var node = new Sequence(Helpers.SucceedingTreeNode(), Helpers.SucceedingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_All_Children_Succeeds()
        {
            // Arrange
            var node = new Sequence(Helpers.SucceedingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_All_Children_Succeeds2()
        {
            // Arrange
            var node = new Sequence(Helpers.SucceedingTreeNode(), Helpers.SucceedingTreeNode());

            var context = Stubs.IBehaviourTreeContext;

            // Act
            node.Tick(context);
            var actual = node.Tick(context);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Failed_If_No_Child_Succeeded()
        {
            // Arrange
            var node = new Sequence(Helpers.FailingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Failure()
        {
            // Arrange
            var node = new Sequence(Helpers.FailingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Success()
        {
            // Arrange
            var node = new Sequence(Helpers.SucceedingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Running()
        {
            // Arrange
            var node = new Sequence(Helpers.SucceedingTreeNode(), Helpers.SucceedingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
