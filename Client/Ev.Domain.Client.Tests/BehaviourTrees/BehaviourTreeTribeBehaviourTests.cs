using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    class TestBehaviourTreeTribeBehaviour : BehaviourTreeTribeBehaviour
    {
        private readonly IGameAction _move;

        public TestBehaviourTreeTribeBehaviour(IRandom rnd, IGameAction move) : base(rnd)
        {
            _move = move;
        }

        protected override IBehaviourTreeNode CreateRoot() => new GameActionNode((w, t) => _move);
    }

    [TestClass]
    public class BehaviourTreeTribeBehaviourTests
    {
        private readonly BehaviourTreeTribeBehaviour uat = new TestBehaviourTreeTribeBehaviour(Stubs.IRandom, new HoldAction());

        [TestMethod]
        public void DoMove_Should_Throw_ArgumentNullException_If_WorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.DoMove(null, Stubs.Client.ITribe));
        }

        [TestMethod]
        public void DoMove_Should_Throw_ArgumentNullException_If_TribeState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.DoMove(Stubs.IIWorldState, null));
        }

        [TestMethod]
        public void DoMove_Should_Return_Hold_If_No_Move()
        {
            // Arrange
            var uat = new TestBehaviourTreeTribeBehaviour(Stubs.IRandom, null);
            
            // Act
            var actual = uat.DoMove(Stubs.IIWorldState, Stubs.Client.ITribe);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(HoldAction));
        }

        [TestMethod]
        public void DoMove_Should_Return_Context_Move_If_Present()
        {
            // Arrange
            var expected = new MoveAction(Direction.E);

            var uat = new TestBehaviourTreeTribeBehaviour(Stubs.IRandom, expected);

            // Act
            var actual = uat.DoMove(Stubs.IIWorldState, Stubs.Client.ITribe);

            // Assert
            Assert.AreSame(expected, actual);
        }
    }
}
