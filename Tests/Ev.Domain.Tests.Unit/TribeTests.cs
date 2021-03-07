using Ev.Domain.Entities;
using Ev.Domain.Entities.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ev.Domain.Tests.Unit
{
    using static Helpers.TestHelpers;

    [TestClass]
    public class TribeTests
    {
        private readonly ITribe uat;

        public TribeTests()
        {
            uat = new Tribe("t1", (0, 0), Utils.Color.DarkYellow);
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Assign_Parameters_To_Members()
        {
            var tribe = new Tribe("t1", (0, 0), Utils.Color.DarkYellow);

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

            var actual = uat.StrongerThan(TestTribeState(20));

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void StrongerThan_Should_Return_False()
        {
            uat.Population = 20;

            var actual = uat.StrongerThan(TestTribeState(100));

            Assert.IsFalse(actual);
        }

        #endregion

        #region WeakerThan

        [TestMethod]
        public void WeakerThan_Should_Return_True()
        {
            uat.Population = 20;

            var actual = uat.WeakerThan(TestTribeState(100));

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void WeakerThan_Should_Return_False()
        {
            uat.Population = 100;

            var actual = uat.WeakerThan(TestTribeState(20));

            Assert.IsFalse(actual);
        }

        #endregion
    }
}