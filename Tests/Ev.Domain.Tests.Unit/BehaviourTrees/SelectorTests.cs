﻿using Ev.Domain.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit.BehaviourTrees
{
    [TestClass]
    public class SelectorTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Children_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new Selector(null);
            });
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            // Arrange & Act
            var child1 = new Mock<IBehaviourTreeNode>().Object;
            var child2 = new Mock<IBehaviourTreeNode>().Object;

            var node = new Selector(child1, child2);

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Running_If_First_Child_Fails_But_Evaluation_Not_Finished()
        {
            // Arrange
            var node = new Selector(Helpers.FailingTreeNode(), Helpers.SucceedingTreeNode());

            // Act
            var actual = node.Tick(Helpers.CreateMockContext());

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_A_Child_Succeeds()
        {
            // Arrange
            var node = new Selector(Helpers.SucceedingTreeNode(), Helpers.FailingTreeNode());

            // Act
            var actual = node.Tick(Helpers.CreateMockContext());

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_A_Child_Succeeds2()
        {
            // Arrange
            var node = new Selector(Helpers.FailingTreeNode(), Helpers.SucceedingTreeNode());

            var context = Helpers.CreateMockContext();

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
            var node = new Selector(Helpers.FailingTreeNode());

            // Act
            var actual = node.Tick(Helpers.CreateMockContext());

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Failure()
        {
            // Arrange
            var node = new Selector(Helpers.FailingTreeNode());

            node.Tick(Helpers.CreateMockContext());

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Success()
        {
            // Arrange
            var node = new Selector(Helpers.SucceedingTreeNode());

            node.Tick(Helpers.CreateMockContext());

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        [TestMethod]
        public void Reset_Should_Set_State_To_NotStarted_After_Running()
        {
            // Arrange
            var node = new Selector(Helpers.FailingTreeNode(), Helpers.SucceedingTreeNode());

            node.Tick(Helpers.CreateMockContext());

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
