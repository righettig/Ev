using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Actions;
using Ev.Domain.Server.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Ev.Infrastructure.Tests
{
    [TestClass]
    public class MapperTests
    {
        private readonly Mapper _uat = new();

        [TestMethod]
        public void Map_Should_Map_Tribe()
        {
            var tribe = new Tribe("t1", (1, 1), Color.Blue) {Population = 123, PrevPosition = (2, 2), Iron = 1, Wood = 2};

            var actual = _uat.Map(tribe);

            Assert.AreEqual("t1",       actual.Name);
            Assert.AreEqual(Color.Blue, actual.Color);
            Assert.AreEqual(123,        actual.Population);
            Assert.AreEqual((1, 1),     actual.Position);
            Assert.AreEqual((2, 2),     actual.PrevPosition);
            Assert.AreEqual(1,          actual.Iron);
            Assert.AreEqual(2,          actual.Wood);
        }

        #region Actions

        [TestMethod]
        public void Map_Should_Map_Actions1()
        {
            // Act
            var actual = _uat.Map(new HoldAction());

            Assert.IsInstanceOfType(actual, typeof(Domain.Server.Actions.HoldAction));
        }

        [TestMethod]
        public void Map_Should_Map_Actions2()
        {
            // Act
            var actual = _uat.Map(new MoveAction(Direction.E));

            Assert.IsInstanceOfType(actual, typeof(Domain.Server.Actions.MoveAction));
            Assert.AreEqual(Direction.E, ((Domain.Server.Actions.MoveAction) actual).Direction);
        }

        [TestMethod]
        public void Map_Should_Map_Actions3()
        {
            var actual = _uat.Map(new AttackAction("target"));

            Assert.IsInstanceOfType(actual, typeof(Domain.Server.Actions.AttackAction));
            Assert.AreEqual("target", ((Domain.Server.Actions.AttackAction) actual).TargetName);
        }

        #endregion

        [TestMethod]
        public void Map_Should_Map_WorldState()
        {
            // Arrange
            var entities = new IWorldEntity[2, 2];
            entities[0, 0] = new Tribe("t1", (1, 1), Color.Yellow);
            entities[0, 1] = new CollectableWorldEntity {Type = CollectableWorldEntityType.Food, Value = 123};
            entities[1, 1] = new BlockingWorldEntity {Type = BlockingWorldEntityType.Water};

            IWorldState worldState = new WorldState(entities);

            // Act
            var actual = _uat.Map(worldState);

            // Assert
            var clientTribe = (actual.State[0, 0] as Domain.Client.Core.ITribe);

            Debug.Assert(clientTribe != null, nameof(clientTribe) + " != null");

            Assert.AreEqual("t1", clientTribe.Name);
            Assert.AreEqual(Color.Yellow, clientTribe.Color);
            Assert.AreEqual((1,1), clientTribe.Position);

            var collectable = actual.State[0, 1] as CollectableWorldEntity;
            
            Debug.Assert(collectable != null, nameof(collectable) + " != null");

            Assert.AreEqual(CollectableWorldEntityType.Food, collectable.Type);
            Assert.AreEqual(123, collectable.Value);

            var blocking = actual.State[1, 1] as BlockingWorldEntity;

            Debug.Assert(blocking != null, nameof(blocking) + " != null");
            Assert.AreEqual(BlockingWorldEntityType.Water, blocking.Type);

            Assert.IsNull(actual.State[1, 0]);
        }
    }
}