using Ev.Domain.Actions;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit
{
    using static Helpers.TestHelpers;

    [TestClass]
    public class TribeTests
    {
        private readonly ITribe uat;

        public TribeTests()
        {
            var tribeBehaviour = new Mock<ITribeBehaviour>();
            tribeBehaviour.Setup(m => m.DoMove(It.IsAny<IWorldState>(), It.IsAny<ITribe>())).Returns(new HoldAction());

            uat = new Tribe("t1", (0, 0), Utils.Color.DarkYellow, tribeBehaviour.Object);
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Behaviour_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var tribe = new Tribe("t1", (0,0), Utils.Color.DarkYellow, null);
            });
        }

        [TestMethod]
        public void Ctor_Should_Assign_Parameters_To_Members()
        {
            var tribe = new Tribe("t1", (0, 0), Utils.Color.DarkYellow, new Mock<ITribeBehaviour>().Object);

            Assert.AreEqual("t1", tribe.Name);
            Assert.AreEqual((0, 0), tribe.Position);
            Assert.AreEqual((0, 0), tribe.PrevPosition);
            Assert.AreEqual(100, tribe.Population);
            Assert.AreEqual(100, tribe.PrevPopulation);
            Assert.AreEqual(Utils.Color.DarkYellow, tribe.Color);
        }

        #endregion

        #region StrongerThan

        [TestMethod]
        public void StrongerThan_Should_Return_True()
        {
            uat.Population = 100;

            var actual = uat.StrongerThan(TestTribe(20));

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void StrongerThan_Should_Return_False()
        {
            uat.Population = 20;

            var actual = uat.StrongerThan(TestTribe(100));

            Assert.IsFalse(actual);
        }

        #endregion

        #region WeakerThan

        [TestMethod]
        public void WeakerThan_Should_Return_True()
        {
            uat.Population = 20;

            var actual = uat.WeakerThan(TestTribe(100));

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void WeakerThan_Should_Return_False()
        {
            uat.Population = 100;

            var actual = uat.WeakerThan(TestTribe(20));

            Assert.IsFalse(actual);
        }

        #endregion

        #region DoMove

        [TestMethod]
        public void DoMove_Should_Throw_ArgumentNullException_If_World_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.DoMove(null);
            });
        }

        [TestMethod]
        public void DoMove()
        {
            var move = uat.DoMove(new Mock<IWorldState>().Object);

            Assert.AreSame(uat, move.Tribe);
        }

        #endregion
    }
}