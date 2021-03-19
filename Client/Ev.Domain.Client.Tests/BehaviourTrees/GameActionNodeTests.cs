using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class GameActionNodeTests
    {
        private readonly GameActionNode uat = new((worldState, tribeState) => new HoldAction());

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Behaviour_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new GameActionNode((ITribeBehaviour) null));
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_BehaviourFn_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new GameActionNode((Func<IWorldState, ITribe, IGameAction>) null));
        }

        [TestMethod]
        public void Ctor_Initial_State_Should_Be_Not_Started()
        {
            Assert.AreEqual(NodeResult.NotStarted, uat.State);
        }

        #endregion

        #region Tick

        [TestMethod]
        public void Tick_Should_Throw_ArgumentNullException_If_Null_Context()
        {
            ShouldThrowArgumentNullException(() => uat.Tick(null));
        }

        [TestMethod]
        public void Tick_Should_Set_State_To_Success()
        {
            // Act
            uat.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, uat.State);
        }

        [TestMethod]
        public void Tick_Should_Return_Success()
        {
            // Act
            var actual = uat.Tick(Stubs.IBehaviourTreeContext);

            // Assert
            Assert.AreEqual(NodeResult.Success, actual);
        }

        [TestMethod]
        public void Tick_Should_Set_Move_In_Context()
        {
            // Arrange
            var action = new HoldAction();

            var node = new GameActionNode((worldState, tribeState) => action);

            var context = new BehaviourTreeContext(Stubs.IIWorldState, Stubs.Client.ITribe);

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
            uat.Tick(Stubs.IBehaviourTreeContext);
            
            // Act
            uat.Reset();

            // Assert
            Assert.AreEqual(NodeResult.NotStarted, uat.State);
        }

        #endregion
    }
}
