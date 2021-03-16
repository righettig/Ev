using Ev.Common.Core;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Processors;
using Ev.Domain.Server.World.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ev.Tests.Common.TestHelpers;
using Mocks = Ev.Tests.Common.Mocks;
using TestHelpers = Ev.Domain.Server.Tests.Helpers.TestHelpers;

namespace Ev.Domain.Server.Tests
{
    using static TestHelpers;

    [TestClass]
    public class GameActionProcessorTests
    {
        private readonly GameActionProcessor _uat;

        public GameActionProcessorTests()
        {
            _uat = new GameActionProcessor(Stubs.IAttackOutcomePredictor);
        }

        #region Null Checks

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_AttackOutcomePredictor_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new GameActionProcessor(null));
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Action_Is_Null()
        {
            ShouldThrowArgumentNullException(() =>
            {
                IGameAction nullAction = null;

                _uat.Update(nullAction, new Mock<ITribe>().Object, Stubs.IWorld, 0);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.Update(Stubs.Server.IGameAction, null, Stubs.IWorld, 0));
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_World_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.Update(Stubs.Server.IGameAction, Stubs.Server.ITribe, null, 0));
        }

        #endregion

        #region Hold

        [TestMethod]
        public void Hold_Update()
        {
            // Arrange
            var tribe = Mocks.ITribe().WithPopulation(10).Build();

            // Act
            _uat.Update(new HoldAction(), tribe, Stubs.IWorld, 0);


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

            var action = new AttackAction("defender") { Target = defender };

            // Act
            _uat.Update(action, attacker, Stubs.IWorld, 0);

            // Assert
            Assert.IsTrue(attacker.IsAttacking);
            Assert.IsTrue(defender.IsAttacking);
        }

        [TestMethod]
        public void Attack_Update_Attacker_Wins_Loser_Survives()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: true));

            var attacker = Mocks.ITribe().WithPopulation(100).Build();
            var defender = Mocks.ITribe().WithPopulation(100).Build();

            var action = new AttackAction("defender") { Target = defender };

            // Act
            uat.Update(action, attacker, Stubs.IWorld, 0);

            // Assert
            Assert.AreEqual(100 + GameActionProcessor.WIN_GAIN, attacker.Population);
            Assert.AreEqual(100 - GameActionProcessor.DEFEAT_LOSS, defender.Population);
        }

        [TestMethod]
        public void Attack_Update_Defender_Wins_Attacker_Survives()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: false));

            var attacker = Mocks.ITribe().WithPopulation(100).Build();
            var defender = Mocks.ITribe().WithPopulation(100).Build();

            var action = new AttackAction("defender") { Target = defender };

            // Act
            uat.Update(action, attacker, Stubs.IWorld, 0);

            // Assert
            Assert.AreEqual(100 - GameActionProcessor.DEFEAT_LOSS, attacker.Population);
            Assert.AreEqual(100 + GameActionProcessor.WIN_GAIN,    defender.Population);
        }

        [TestMethod]
        public void Attack_Update_Attacker_Wins_Defender_Dies()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: true));

            var attacker = Mocks.ITribe().WithPopulation(100).Build();
            var defender = Mocks.ITribe().WithPopulation(20).Build();

            var world = new Mock<IWorld>();
            world.Setup(m => m.WipeTribe(It.Is<ITribe>(t => t == defender), It.IsAny<int>())).Verifiable();

            var action = new AttackAction("defender") { Target = defender };

            // Act
            uat.Update(action, attacker, world.Object, 0);

            // Assert
            Assert.AreEqual(0, defender.Population);
            world.Verify();
        }

        [TestMethod]
        public void Attack_Update_Defender_Wins_Attacker_Dies()
        {
            // Arrange
            var uat = new GameActionProcessor(Predictor(outcome: false));

            var attacker = Mocks.ITribe().WithPopulation(20).Build();
            var defender = Mocks.ITribe().WithPopulation(100).Build();

            var action = new AttackAction("defender") { Target = defender };

            // Act
            uat.Update(action, attacker, new Mock<IWorld>().Object, 0);

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
            _uat.Update(new MoveAction(Direction.E), tribe, world.Object, 0);

            // Assert
            Assert.AreEqual(97, tribe.Population);
            world.Verify(m => m.Move(tribe, Direction.E));
        }

        [TestMethod]
        public void Move_When_CannotMove_Should_Hold()
        {
            // Arrange
            var world = new Mock<IWorld>();
            world.Setup(m => m.CanMove(It.IsAny<(int, int)>(), It.IsAny<Direction>())).Returns(false);
            
            var tribe = TestTribe(100);

            // Act
            _uat.Update(new MoveAction(Direction.E), tribe, world.Object, 0);

            // Assert
            Assert.AreEqual(99, tribe.Population);
        }

        #endregion

        #region UpgradeDefenses

        [TestMethod]
        public void Update_Should_Lock_Tribe_For_2_Turns() 
        {
            // Arrange
            var tribe = TestTribe(100);
            tribe.LockedForNTurns = null;

            // Act
            var action = new UpgradeDefensesAction();
            _uat.Update(action, tribe, Stubs.IWorld, 0);

            // Assert
            Assert.AreSame(action, tribe.BusyDoing);
            Assert.AreEqual(2, tribe.LockedForNTurns);
        }

        [TestMethod]
        public void Update_When_Lock_Finishes_Should_Unlock_Tribe()
        {
            // Arrange
            var tribe = TestTribe(100);
            tribe.LockedForNTurns = 1;

            // Act
            var action = new UpgradeDefensesAction();
            _uat.Update(action, tribe, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.IsNull(tribe.LockedForNTurns);
            Assert.IsNull(tribe.BusyDoing);
            Assert.IsTrue(action.Completed);
        }

        #endregion

        #region UpgradeAttack

        [TestMethod]
        public void Update_Should_Lock_Tribe_For_2_Turns_When_Upgrading_The_Attack()
        {
            // Arrange
            var tribe = TestTribe(100);
            tribe.LockedForNTurns = null;

            // Act
            var action = new UpgradeAttackAction();
            _uat.Update(action, tribe, Stubs.IWorld, 0);

            // Assert
            Assert.AreSame(action, tribe.BusyDoing);
            Assert.AreEqual(2, tribe.LockedForNTurns);
        }

        [TestMethod]
        public void Update_When_Lock_Finishes_Should_Unlock_Tribe_When_Upgrading_The_Attack ()
        {
            // Arrange
            var tribe = TestTribe(100);
            tribe.LockedForNTurns = 1;

            // Act
            var action = new UpgradeAttackAction();
            _uat.Update(action, tribe, new Mock<IWorld>().Object, 0);

            // Assert
            Assert.IsNull(tribe.LockedForNTurns);
            Assert.IsNull(tribe.BusyDoing);
            Assert.IsTrue(action.Completed);
        }

        #endregion

        #region Suicide

        [TestMethod]
        public void Suicide_Update()
        {
            // Arrange
            var tribe = new Mock<ITribe>().SetupProperty(m => m.Population).Object;

            tribe.Population = 10;

            // Act
            _uat.Update(new SuicideAction(), tribe, Stubs.IWorld, 0);


            // Assert
            Assert.AreEqual(0, tribe.Population);
        }

        #endregion
    }
}