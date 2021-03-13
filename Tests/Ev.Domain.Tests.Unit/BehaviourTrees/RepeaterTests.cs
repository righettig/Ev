using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Behaviours.BehaviourTrees.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit.BehaviourTrees
{
    [TestClass]
    public class RepeaterTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Child_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new Repeater(null, 10);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentException_If_Count_Is_Negative()
        {
            var child = new Mock<IBehaviourTreeNode>().Object;

            Assert.ThrowsException<ArgumentException>(() =>
            {
                var node = new Repeater(child, -1);
            });
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            // Arrange & Act
            var child = new Mock<IBehaviourTreeNode>().Object;

            var node = new Repeater(child, 3);

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Failed_If_Child_Fails()
        {
            // Arrange
            var node = new Repeater(Helpers.FailingTreeNode(), 3);

            // Act
            var actual = node.Tick(Helpers.CreateMockContext());

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_When_Invoked_Less_Than_Count_Times_And_Child_Succeeds()
        {
            // Arrange
            var node = new Repeater(Helpers.SucceedingTreeNode(), 2);

            // Act
            var actual = node.Tick(Helpers.CreateMockContext());

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_Invoked_Count_Times_And_Child_Succeeds()
        {
            // Arrange
            var node = new Repeater(Helpers.SucceedingTreeNode(), 3);

            var context = Helpers.CreateMockContext();

            // Act
            node.Tick(context);
            node.Tick(context);

            var actual = node.Tick(context);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset()
        {
            // Arrange
            var child = new Mock<IBehaviourTreeNode>().Object;

            var node = new Repeater(child, 3);

            node.Tick(Helpers.CreateMockContext());

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
