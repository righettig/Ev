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

        #region CanWin with Defense Bonus

        /*
            0.5 (rnd)
	            20  / (20+110) = 20  / 130 = 0.153 => FALSE
	            80  / (80+110) = 80  / 190 = 0.421 => FALSE
	            100 / (100+88) = 100 / 188 = 0.531 => TRUE
        */

        [TestMethod]
        public void CanWin1_DefenseBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(20);
            ITribe defender = TestTribe(100);
            defender.Defense = .1f;

            // Act
            var actual = uat.CanWin(attacker, defender);

            // Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin2_DefenseBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(80);
            ITribe defender = TestTribe(100);
            defender.Defense = .1f;

            // Act
            var actual = uat.CanWin(attacker, defender);

            // Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin3_DefenseBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(100);
            ITribe defender = TestTribe(80);
            defender.Defense = .1f;

            // Act
            var actual = uat.CanWin(TestTribe(100), TestTribe(80));

            // Assert
            Assert.AreEqual(true, actual);
        }

        #endregion

        #region CanWin with Attack Bonus

        /*
            0.5 (rnd)
	            22  / (22+100) = 22  / 122 = 0.180 => FALSE
	            88  / (88+100) = 88  / 188 = 0.468 => FALSE
	            110 / (110+80) = 110 / 190 = 0.578 => TRUE
        */

        [TestMethod]
        public void CanWin1_AttackBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(20);
            attacker.Attack = .1f;

            ITribe defender = TestTribe(100);

            // Act
            var actual = uat.CanWin(attacker, defender);

            // Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin2_AttackBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(80);
            attacker.Attack = .1f;

            ITribe defender = TestTribe(100);

            // Act
            var actual = uat.CanWin(attacker, defender);

            // Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CanWin3_AttackBonus()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.NextDouble()).Returns(0.5);

            var uat = new AttackOutcomePredictor(rnd.Object);

            ITribe attacker = TestTribe(100);
            attacker.Attack = .1f;

            ITribe defender = TestTribe(80);

            // Act
            var actual = uat.CanWin(attacker, defender);

            // Assert
            Assert.AreEqual(true, actual);
        }

        #endregion
    }
}