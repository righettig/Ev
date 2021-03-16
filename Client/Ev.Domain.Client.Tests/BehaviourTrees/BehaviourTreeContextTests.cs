using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class BehaviourTreeContextTests
    {
        private readonly BehaviourTreeContext uat = new(Stubs.IIWorldState, Stubs.Client.ITribe);

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Raise_ArgumentNullException_When_WorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new BehaviourTreeContext(null, Stubs.Client.ITribe));
        }

        [TestMethod]
        public void Ctor_Should_Raise_ArgumentNullException_When_TribeState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new BehaviourTreeContext(Stubs.IIWorldState, null));
        }

        #endregion

        #region Set

        [TestMethod]
        public void Set_Cannot_Use_Reserved_Entry_WorldState()
        {
            ShouldThrowArgumentException(() => uat.Set("worldState", new object()));
        }

        [TestMethod]
        public void Set_Cannot_Use_Reserved_Entry_TribeState()
        {
            ShouldThrowArgumentException(() => uat.Set("tribeState", new object()));
        }

        [TestMethod]
        public void Set_Should_Save_The_Value()
        {
            // Arrange
            var expected = new object();

            // Act
            uat.Set("foo", expected);

            // Assert
            Assert.AreSame(expected, uat["foo"]);
        }

        #endregion

        #region Indexer

        [TestMethod]
        public void Indexer_Cannot_Use_Reserved_Entry_WorldState()
        {
            ShouldThrowArgumentException(() => uat["worldState"] = new object());
        }

        [TestMethod]
        public void Indexer_Cannot_Use_Reserved_Entry_TribeState()
        {
            ShouldThrowArgumentException(() => uat["tribeState"] = new object());
        }

        [TestMethod]
        public void Indexer_Should_Save_The_Value()
        {
            // Arrange
            var expected = new object();

            // Act
            uat["foo"] = expected;

            // Assert
            Assert.AreSame(expected, uat["foo"]);
        }

        #endregion

        [TestMethod]
        public void Get_Should_Return_The_Correct_Value()
        {
            // Arrange
            var expected = new object();

            uat["foo"] = expected;

            // Act
            var actual = uat.Get("foo");

            // Assert
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void WorldState_Getter_Should_Return_Correct_Value()
        {
            // Arrange
            var worldState = Stubs.IIWorldState;

            var context = new BehaviourTreeContext(worldState, Stubs.Client.ITribe);

            // Act
            var actual = context.WorldState;

            // Assert
            Assert.AreSame(worldState, actual);
        }

        [TestMethod]
        public void TribeState_Getter_Should_Return_Correct_Value()
        {
            // Arrange
            var tribeState = Stubs.Client.ITribe;

            var context = new BehaviourTreeContext(Stubs.IIWorldState, tribeState);

            // Act
            var actual = context.TribeState;

            // Assert
            Assert.AreSame(tribeState, actual);
        }

        [TestMethod]
        public void Move_Setter_Should_Set_Correct_Value()
        {
            // Arrange
            var expected = Stubs.Client.IGameAction;

            // Act
            uat.Move = expected;

            // Assert
            Assert.AreSame(expected, uat.Move);
        }
    }
}