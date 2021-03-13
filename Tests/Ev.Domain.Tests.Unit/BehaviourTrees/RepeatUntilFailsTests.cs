using Ev.Domain.Behaviours.BehaviourTrees;
using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Behaviours.BehaviourTrees.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using static Ev.Domain.Tests.Unit.BehaviourTrees.Helpers;

namespace Ev.Domain.Tests.Unit.BehaviourTrees
{
    [TestClass]
    public class RepeatUntilFails
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNull_Exception_If_Child_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new RepeatUntilFail(null);
            });
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Success_If_Child_Fails()
        {
            // Arrange
            var node = new RepeatUntilFail(FailingTreeNode());

            // Act
            var actual = node.Tick(new Mock<IBehaviourTreeContext>().Object);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_If_Child_Running()
        {
            // Arrange
            var node = new RepeatUntilFail(RunningTreeNode());

            // Act
            var actual = node.Tick(new Mock<IBehaviourTreeContext>().Object);

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_If_Child_Succeeds()
        {
            // Arrange
            var node = new RepeatUntilFail(SucceedingTreeNode());

            // Act
            var actual = node.Tick(new Mock<IBehaviourTreeContext>().Object);

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

            node.Tick(new Mock<IBehaviourTreeContext>().Object);

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

            node.Tick(new Mock<IBehaviourTreeContext>().Object);

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

            node.Tick(new Mock<IBehaviourTreeContext>().Object);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}