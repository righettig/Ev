using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Actions.Processors;
using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit
{
    using static Helpers.TestHelpers;

    [TestClass]
    public class GameActionProcessorTests
    {
        private readonly GameActionProcessor uat;

        public GameActionProcessorTests()
        {
            uat = new GameActionProcessor(new Mock<IAttackOutcomePredictor>().Object);
        }

        #region Null Checks

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_AttackOutcomePredictor_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var gap = new GameActionProcessor(null);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Action_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                IGameAction nullAction = null;

                uat.Update(nullAction, new Mock<ITribe>().Object, new Mock<IWorld>().Object, 0);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.Update(new Mock<IGameAction>().Object, null, new Mock<IWorld>().Object, 0);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_World_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.Update(new Mock<IGameAction>().Object, new Mock<ITribe>().Object, null, 0);
            });
        }

        #endregion

        #region Hold

        [TestMethod]
        public void Hold_Update()
        {
            // Arrange
            var tribe = new Mock<ITribe>().SetupProperty(m => m.Population).Object;
            
            tribe.Population = 10;

            // Act
            uat.Update(new HoldAction(), tribe, new Mock<IWorld>().Object, 0);


            // Assert
            Assert.AreEqual(9, tribe.Population);
        }

        #endregion

        #region Attack

        [TestMethod]
        public void Attack_Update_Should_Set_IsAttacking_For_Both_Attacker_And_Defender()
        {
            // Arrange
            var attacker = new Mock<ITribe>().SetupProperty(m => m.IsAttacking).Object;
            var defender = new Mock<ITribe>().SetupProperty(m => m.IsAttacking).Object;

            // Act
            uat.Update(new AttackAction(defender), attacker, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.IsTrue(attacker.IsAttacking);
            Assert.IsTrue(defender.IsAttacking);
        }

        [TestMethod]
        public void Attack_Update_Attacker_Wins_Loser_Survives()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: true));

            var attacker = new Mock<ITribe>().SetupProperty(m => m.Population).Object;
            var defender = new Mock<ITribe>().SetupProperty(m => m.Population).Object;

            attacker.Population = 100;
            defender.Population = 100;

            // Act
            uat.Update(new AttackAction(defender), attacker, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.AreEqual(100 + GameActionProcessor.WIN_GAIN, attacker.Population);
            Assert.AreEqual(100 - GameActionProcessor.DEFEAT_LOSS, defender.Population);
        }

        [TestMethod]
        public void Attack_Update_Defender_Wins_Attacker_Survives()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: false));

            var attacker = new Mock<ITribe>().SetupProperty(m => m.Population).Object;
            var defender = new Mock<ITribe>().SetupProperty(m => m.Population).Object;

            attacker.Population = 100;
            defender.Population = 100;

            // Act
            uat.Update(new AttackAction(defender), attacker, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.AreEqual(100 - GameActionProcessor.DEFEAT_LOSS, attacker.Population);
            Assert.AreEqual(100 + GameActionProcessor.WIN_GAIN,    defender.Population);
        }

        [TestMethod]
        public void Attack_Update_Attacker_Wins_Defender_Dies()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: true));

            var attacker = new Mock<ITribe>().SetupProperty(m => m.Population).Object;
            var defender = new Mock<ITribe>().SetupProperty(m => m.Population).Object;

            var world = new Mock<IWorld>();
            world.Setup(m => m.WipeTribe(It.Is<ITribe>(t => t == defender), It.IsAny<int>())).Verifiable();

            attacker.Population = 100;
            defender.Population = 20;

            // Act
            uat.Update(new AttackAction(defender), attacker, world.Object, 0);

            // Assert
            Assert.AreEqual(0, defender.Population);
            world.Verify();
        }

        [TestMethod]
        public void Attack_Update_Defender_Wins_Attacker_Dies()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: false));

            var attacker = new Mock<ITribe>().SetupProperty(m => m.Population).Object;
            var defender = new Mock<ITribe>().SetupProperty(m => m.Population).Object;

            attacker.Population = 20;
            defender.Population = 100;

            // Act
            uat.Update(new AttackAction(defender), attacker, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.AreEqual(0, attacker.Population);
        }

        private static IAttackOutcomePredictor Predictor(bool outcome)
        {
            var predictor = new Mock<IAttackOutcomePredictor>();

            predictor.Setup(
                m => m.CanWin(It.IsAny<ITribe>(), It.IsAny<ITribe>())).Returns(outcome);

            return predictor.Object;
        }

        #endregion

        #region Move

        [TestMethod]
        public void Move_When_CanMove()
        {
            // Arrange
            var world = new Mock<IWorld>();
            world.Setup(m => m.CanMove(It.IsAny<(int,int)>(), It.IsAny<Direction>())).Returns(true);
            world.Setup(m => m.Move(It.IsAny<ITribe>(), It.IsAny<Direction>()));

            var tribe = TestTribe(100);

            // Act
            uat.Update(new MoveAction(Direction.E), tribe, world.Object, 0);

            // Assert
            Assert.AreEqual(97, tribe.Population);
            world.Verify(m => m.Move(tribe, Direction.E));
        }

        public void Move_When_CannotMove_Should_Hold()
        {
            // Arrange
            var world = new Mock<IWorld>();
            world.Setup(m => m.CanMove(It.IsAny<(int, int)>(), It.IsAny<Direction>())).Returns(false);
            
            var tribe = TestTribe(100);

            // Act
            uat.Update(new MoveAction(Direction.E), TestTribe(100), world.Object, 0);

            // Assert
            Assert.AreEqual(99, tribe.Population);
        }

        #endregion
    }
}