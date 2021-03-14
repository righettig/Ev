using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class GameActionNodeTests
    {
        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Behaviour_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new GameActionNode((ITribeBehaviour) null);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_BehaviourFn_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var node = new GameActionNode((Func<IWorldState, ITribe, IGameAction>) null);
            });
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            // Arrange & Act
            var node = new GameActionNode((worldState, tribeState) => new HoldAction());

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Throw_ArgumentNullException_If_Null_Context()
        {
            var node = new GameActionNode((worldState, tribeState) => new HoldAction());

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                node.Tick(null);
            });
        }

        [TestMethod]
        public void Tick_Should_Set_State_To_Success()
        {
            // Arrange
            var node = new GameActionNode((worldState, tribeState) => new HoldAction());

            // Act
            node.Tick(Helpers.CreateContext());

            // Assert
            Assert.AreEqual(NodeResult.Success, node.State);
        }

        [TestMethod]
        public void Tick_Should_Return_Success()
        {
            // Arrange
            var node = new GameActionNode((worldState, tribeState) => new HoldAction());

            // Act
            var actual = node.Tick(Helpers.CreateContext());

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Set_Move_In_Context()
        {
            // Arrange
            var action = new HoldAction();

            var node = new GameActionNode((worldState, tribeState) => action);

            var context = new BehaviourTreeContext(new Mock<IWorldState>().Object, new Mock<ITribe>().Object);

            // Act
            node.Tick(context);

            // Assert
            Assert.AreSame(action, context.Move);
        }

        #endregion

        #region Reset

        [TestMethod]
        public void Reset()
        {
            // Arrange
            var action = new HoldAction();

            var node = new GameActionNode((worldState, tribeState) => action);

            node.Tick(Helpers.CreateContext());
            
            // Act
            node.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, node.State);
        }

        #endregion
    }
}
