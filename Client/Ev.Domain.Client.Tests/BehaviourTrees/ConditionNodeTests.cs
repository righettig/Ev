using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class ConditionNodeTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNull_Exception_If_Predicate_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new ConditionNode(null));
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Return_Success_If_Predicate_Eval_True()
        {
            // Arrange
            var node = new ConditionNode((w, t) => true);

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);
            
            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Return_Failure_If_Predicate_Eval_False()
        {
            // Arrange
            var node = new ConditionNode((w, t) => false);

            // Act
            var actual = node.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Failed, actual);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset_Should_Reset_To_NotStarted_After_Success()
        {
            // Arrange
            var node = new ConditionNode((w, t) => true);

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
            var node = new ConditionNode((w, t) => false);

            node.Tick(Stubs.IBehaviourTreeContext);

            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
