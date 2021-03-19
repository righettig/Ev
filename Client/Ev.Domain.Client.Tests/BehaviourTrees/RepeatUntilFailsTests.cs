using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Domain.Client.Tests.BehaviourTrees.Helpers;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class RepeatUntilFails
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNull_Exception_If_Child_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new RepeatUntilFail(null));
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Success_If_Child_Fails()
        {
            // Arrange
            var node = new RepeatUntilFail(FailingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_If_Child_Running()
        {
            // Arrange
            var node = new RepeatUntilFail(RunningTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_If_Child_Succeeds()
        {
            // Arrange
            var node = new RepeatUntilFail(SucceedingTreeNode());

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset_Should_Reset_To_NotStarted_After_Success()
        {
            // Arrange
            var node = new RepeatUntilFail(SucceedingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Reset_To_NotStarted_After_Failure()
        {
            // Arrange
            var node = new RepeatUntilFail(FailingTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Reset_To_NotStarted_If_Running()
        {
            // Arrange
            var node = new RepeatUntilFail(RunningTreeNode());

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}