using Ev.Common;
using Ev.Common.Utils;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ev.Domain.Server.Tests
{
    [TestClass]
    public class ActionsTests
    {
        [TestMethod]
        public void Attack_Ctor_Should_Assign_Target()
        {
            var tribe = new Tribe("t1", (0,0), Color.White);

            var action = new AttackAction(tribe.Name);

            Assert.AreSame(tribe.Name, action.TargetName);
        }

        [TestMethod]
        public void Attack_Ctor_Should_Throw_ArgumentNullException_If_Target_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var action = new AttackAction(null);
            });
        }

        [TestMethod]
        public void Attack_ToString()
        {
            var tribe = new Tribe("t1", (0, 0), Color.White);

            var action = new AttackAction(tribe.Name);

            Assert.AreEqual("Attack t1", action.ToString());
        }

        [TestMethod]
        public void Move_Ctor_Should_Assign_Direction()
        {
            var action = new MoveAction(Direction.S);

            Assert.AreEqual(Direction.S, action.Direction);
        }

        [TestMethod]
        public void Move_ToString()
        {
            var action = new MoveAction(Direction.S);

            Assert.AreEqual("Move S", action.ToString());
        }

        [TestMethod]
        public void UpgradeDefenses_OnComplete_Should_Update_Tribe()
        {
            // Arrange
            var tribe = TestHelpers.TestTribe(100);
            tribe.Iron = 5;
            tribe.Wood = 10;
            tribe.Defense = 0;

            var action = new UpgradeDefensesAction();

            // Act
            action.OnComplete(tribe);

            // Assert
            Assert.AreEqual(0, tribe.Wood);
            Assert.AreEqual(0, tribe.Iron);
            Assert.AreEqual(.1f, tribe.Defense);
        }

        [TestMethod]
        public void UpgradeAttack_OnComplete_Should_Update_Tribe()
        {
            // Arrange
            var tribe = TestHelpers.TestTribe(100);
            tribe.Iron = 5;
            tribe.Wood = 10;
            tribe.Attack = 0;

            var action = new UpgradeAttackAction();

            // Act
            action.OnComplete(tribe);

            // Assert
            Assert.AreEqual(0, tribe.Wood);
            Assert.AreEqual(0, tribe.Iron);
            Assert.AreEqual(.1f, tribe.Attack);
        }
    }
}
