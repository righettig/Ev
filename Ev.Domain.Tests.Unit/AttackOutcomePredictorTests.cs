using Ev.Domain.Actions.Core.Processors;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit
{
    using static Helpers.TestHelpers;

    [TestClass]
    public class AttackOutcomePredicotorTests
    {
        private readonly AttackOutcomePredictor uat;

        public AttackOutcomePredicotorTests()
        {
            uat = new AttackOutcomePredictor(new Mock<IRandom>().Object);
        }

        #region Null Checks

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Rnd_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var uat = new AttackOutcomePredictor(null);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Attacker_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.CanWin(null, new Mock<ITribe>().Object);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Defender_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.CanWin(new Mock<ITribe>().Object, null);
            });
        }

        #endregion

        #region CanWin

        /*
            0.5 (rnd)
	            20  / (20+100) = 20  / 120 = 0.16 => FALSE
	            80  / (80+100) = 80  / 180 = 0.44 => FALSE
	            100 / (100+80) = 100 / 180 = 0.55 => TRUE
	
            0.9 (rnd)
	            20  / (20+100) = 20  / 120 = 0.16 => FALSE
	            80  / (80+100) = 80  / 180 = 0.44 => FALSE
	            100 / (100+80) = 100 / 180 = 0.55 => FALSE
	
            0.2 (rnd)
	            20  / (20+100) = 20  / 120 = 0.16 => FALSE
	            80  / (80+100) = 80  / 180 = 0.44 => TRUE
	            100 / (100+80) = 100 / 180 = 0.55 => TRUE
        */

        [TestMethod]
        public void CanWin1()
        {
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            var actual = uat.CanWin(TestTribe(20), TestTribe(100));

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin2()
        {
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            var actual = uat.CanWin(TestTribe(80), TestTribe(100));

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin3()
        {
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            var actual = uat.CanWin(TestTribe(100), TestTribe(80));

            Assert.AreEqual(true, actual);
        }

        #endregion
    }
}