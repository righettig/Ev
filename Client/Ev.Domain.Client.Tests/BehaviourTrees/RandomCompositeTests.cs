using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Composite;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class RandomCompositeTests
    {
        private readonly RandomComposite uat = new(new Common.Utils.Random(), new Mock<IBehaviourTreeNode>().Object);

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Rnd_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new RandomComposite((IRandom) null, new Mock<IBehaviourTreeNode>().Object);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Children_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new RandomComposite(new Mock<IRandom>().Object, null);
            });
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

            var child2 = new Mock<IBehaviourTreeNode>().Object;

            var rnd = new Mock<IRandom>();
            rnd
                .Setup(m => m.Next(It.IsAny<int>()))
                .Returns(0); // always choose the first child;

            var node = new RandomComposite(rnd.Object, child1.Object, child2);

            var context = new Mock<IBehaviourTreeContext>().Object;

            // Act
            node.Tick(context);
            node.Tick(context);
            var actual = node.Tick(context);

            // Assert
            child1.Verify(m => m.Tick(It.IsAny<IBehaviourTreeContext>()), Times.Exactly(3));

            Assert.AreEqual(NodeResult.Success, actual);
        }

        // Tick_After_Success_Should_Return_Success
        // Tick_After_Failure_Should_Return_Failure

        #endregion

        #region Reset

        // Reset_After_Success_Should_Set_State_To_NotStarted
        // Reset_After_Failure_Should_Set_State_To_NotStarted

        #endregion
    }
}
