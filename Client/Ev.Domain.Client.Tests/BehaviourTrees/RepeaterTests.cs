using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Decorators;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class RepeaterTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Child_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new Repeater(null, 10));
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentException_If_Count_Is_Negative()
        {
            ShouldThrowArgumentException(() => new Repeater(Stubs.IBehaviourTreeNode, -1));
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            // Arrange & Act
            var node = new Repeater(Stubs.IBehaviourTreeNode, 3);

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
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Running_When_Invoked_Less_Than_Count_Times_And_Child_Succeeds()
        {
            // Arrange
            var node = new Repeater(Helpers.SucceedingTreeNode(), 2);

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Running, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Success_When_Invoked_Count_Times_And_Child_Succeeds()
        {
            // Arrange
            var node = new Repeater(Helpers.SucceedingTreeNode(), 3);

            var context = Stubs.IBehaviourTreeContext;

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
            var node = new Repeater(Stubs.IBehaviourTreeNode, 3);

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
