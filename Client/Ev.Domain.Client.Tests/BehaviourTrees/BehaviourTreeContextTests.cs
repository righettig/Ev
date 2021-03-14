using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Client.Tests.BehaviourTrees
{
    [TestClass]
    public class BehaviourTreeContextTests
    {
        private readonly BehaviourTreeContext uat = new(new Mock<IWorldState>().Object, new Mock<ITribe>().Object);

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Raise_ArgumentNullException_When_WorldState_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new BehaviourTreeContext(null, new Mock<ITribe>().Object);
            });
        }

        [TestMethod]
        public void Ctor_Should_Raise_ArgumentNullException_When_TribeState_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new BehaviourTreeContext(new Mock<IWorldState>().Object, null);
            });
        }

        #endregion

        #region Set

        [TestMethod]
        public void Set_Cannot_Use_Reserved_Entry_WorldState()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                uat.Set("worldState", new object());
            });
        }

        [TestMethod]
        public void Set_Cannot_Use_Reserved_Entry_TribeState()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                uat.Set("tribeState", new object());
            });
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
            Assert.ThrowsException<ArgumentException>(() =>
            {
                uat["worldState"] = new object();
            });
        }

        [TestMethod]
        public void Indexer_Cannot_Use_Reserved_Entry_TribeState()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                uat["tribeState"] = new object();
            });
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
            var worldState = new Mock<IWorldState>().Object;

            var context = new BehaviourTreeContext(worldState, new Mock<ITribe>().Object);

            // Act
            var actual = context.WorldState;

            // Assert
            Assert.AreSame(worldState, actual);
        }

        [TestMethod]
        public void TribeState_Getter_Should_Return_Correct_Value()
        {
            // Arrange
            var tribeState = new Mock<ITribe>().Object;

            var context = new BehaviourTreeContext(new Mock<IWorldState>().Object, tribeState);

            // Act
            var actual = context.TribeState;

            // Assert
            Assert.AreSame(tribeState, actual);
        }

        [TestMethod]
        public void Move_Setter_Should_Set_Correct_Value()
        {
            // Arrange
            var expected = new Mock<IGameAction>().Object;

            // Act
            uat.Move = expected;

            // Assert
            Assert.AreSame(expected, uat.Move);
        }
    }
}