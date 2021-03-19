using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Entities;
using Ev.Domain.Client.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests
{
    [TestClass]
    public class WorldStateExtensionsTests
    {
        private readonly IWorldState _state;

        public WorldStateExtensionsTests()
        {
            _state = CreateStubWorldState();
        }

        [TestMethod]
        public void GetCollectables_Should_Throw_ArgumentNullException_If_State_Is_Null()
        {
            ShouldThrowArgumentNullException(() => WorldStateExtensions.GetCollectables(null));
        }

        [TestMethod]
        public void GetCollectables_Should_Return_All_Collectables()
        {
            // Act
            var actual = _state.GetCollectables();

            // Assert
            Assert.AreEqual(3, actual.Length);
            
            Assert.AreEqual(CollectableWorldEntityType.Food, actual[0].Type);
            Assert.AreEqual(1, actual[0].Value);

            Assert.AreEqual(CollectableWorldEntityType.Wood, actual[1].Type);
            Assert.AreEqual(2, actual[1].Value);

            Assert.AreEqual(CollectableWorldEntityType.Iron, actual[2].Type);
            Assert.AreEqual(3, actual[2].Value);
        }

        [TestMethod]
        public void GetCollectables_Should_Return_All_Collectables_Of_The_Given_Type()
        {
            // Act
            var actual = _state.GetCollectables(CollectableWorldEntityType.Food);

            // Assert
            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(CollectableWorldEntityType.Food, actual[0].Type);
            Assert.AreEqual(1, actual[0].Value);
        }

        [TestMethod]
        public void GetTribes_Should_Throw_ArgumentNullException_If_State_Is_Null()
        {
            ShouldThrowArgumentNullException(() => WorldStateExtensions.GetTribes(null));
        }

        [TestMethod]
        public void GetTribes_Should_Return_All_Tribes()
        {
            // Act
            var actual = _state.GetTribes(ignoreSelf: false);

            // Assert
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("tribe1", actual[0].Name);
            Assert.AreEqual("tribe2", actual[1].Name);
        }

        [TestMethod]
        public void GetTribes_Should_Return_All_Tribes_Except_2_2_Position()
        {
            // Act
            var actual = _state.GetTribes();

            // Assert
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("tribe2", actual[0].Name);
        }

        [TestMethod]
        public void GetBlockings_Should_Throw_ArgumentNullException_If_State_Is_Null()
        {
            ShouldThrowArgumentNullException(() => WorldStateExtensions.GetBlockings(null));
        }
        
        [TestMethod]
        public void GetBlockings_Should_Return_All_Blocking_World_Entities()
        {
            // Act
            var actual = _state.GetBlockings();

            // Assert
            Assert.AreEqual(2, actual.Length);

            Assert.AreEqual(BlockingWorldEntityType.Wall,  actual[0].Type);
            Assert.AreEqual(BlockingWorldEntityType.Water, actual[1].Type);
        }

        [TestMethod]
        public void GetBlockings_Should_Return_All_Blocking_World_Entities_Of_The_Given_Type()
        {
            // Act
            var actual = _state.GetBlockings(BlockingWorldEntityType.Water);

            // Assert
            Assert.AreEqual(1, actual.Length);

            Assert.AreEqual(BlockingWorldEntityType.Water, actual[0].Type);
        }

        [TestMethod]
        public void Closest_Should_Throw_ArgumentNullException_If_State_Is_Null()
        {
            ShouldThrowArgumentNullException(() => WorldStateExtensions.Closest(null));
        }

        [TestMethod]
        public void Closest_Should_Throw_ArgumentNullException_If_State_Is_Null2()
        {
            ShouldThrowArgumentNullException(() => _state.Closest<CollectableWorldEntity>(null));
        }

        [TestMethod]
        public void Closest_Should_Return_The_Closest_Non_NotReachable_Entity()
        {
            // Arrange
            var entities = new IWorldEntity[5, 5];

            // center of the world state
            entities[2, 2] = new Tribe { Name = "tribe1" };

            // other entities, ranked by how close they are according to the traverse order (see WorldStateExtensions.cs)
            entities[1, 1] = new BlockingWorldEntity { Type = BlockingWorldEntityType.NotReachable };
            entities[3, 3] = new Tribe { Name = "tribe2" };
            entities[2, 0] = new CollectableWorldEntity { Type = CollectableWorldEntityType.Iron, Value = 3 };
            entities[0, 1] = new BlockingWorldEntity { Type = BlockingWorldEntityType.Wall };

            var state = new WorldState(entities);

            // Act
            var actual = state.Closest();

            // Assert
            Assert.AreSame(entities[3, 3], actual);
        }

        [TestMethod]
        public void Closest_Should_Throw_ArgumentNullException_If_Entities_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _state.Closest(null));
        }

        [TestMethod]
        public void Closest_Should_Return_The_Closest_Non_NotReachable_Entity2()
        {
            // Arrange
            var entities = new IWorldEntity[5, 5];

            // center of the world state
            entities[2, 2] = new Tribe {Name = "tribe1"};

            // other entities, ranked by how close they are according to the traverse order (see WorldStateExtensions.cs)
            entities[1, 1] = new BlockingWorldEntity {Type = BlockingWorldEntityType.NotReachable};
            entities[3, 3] = new Tribe {Name = "tribe2"};
            entities[2, 0] = new CollectableWorldEntity {Type = CollectableWorldEntityType.Iron, Value = 3};
            entities[0, 1] = new BlockingWorldEntity {Type = BlockingWorldEntityType.Wall};
            
            var state = new WorldState(entities);

            // Act
            var actual = state.Closest(entities[2, 0], entities[1, 1], entities[3, 3]);

            // Assert
            Assert.AreSame(entities[3, 3], actual);
        }

        [TestMethod]
        public void Closest_Should_Return_The_Closest_Entity_Of_The_Given_Type()
        {
            // Act
            var actual = _state.Closest<CollectableWorldEntity>();

            // Assert
            Assert.AreEqual(CollectableWorldEntityType.Wood, actual.Type);
            Assert.AreEqual(2, actual.Value);
        }

        [TestMethod]
        public void Closest_Should_Return_The_Closest_Entity_Of_The_Given_Type2()
        {
            // Arrange
            var entities = new IWorldEntity[5, 5];

            // center of the world state
            entities[2, 2] = new Tribe { Name = "tribe1" };

            // other entities, ranked by how close they are according to the traverse order (see WorldStateExtensions.cs)
            entities[1, 1] = new CollectableWorldEntity { Type = CollectableWorldEntityType.Food, Value = 1 };
            entities[3, 3] = new Tribe { Name = "tribe2" };
            entities[2, 0] = new CollectableWorldEntity { Type = CollectableWorldEntityType.Iron, Value = 3 };
            entities[0, 1] = new BlockingWorldEntity { Type = BlockingWorldEntityType.Wall };

            var state = new WorldState(entities);

            // Act
            var actual = state.Closest(
                entities[1, 1] as CollectableWorldEntity, 
                entities[2, 0] as CollectableWorldEntity);

            // Assert
            Assert.AreEqual(CollectableWorldEntityType.Food, actual.Type);
            Assert.AreEqual(1, actual.Value);
        }

        private static IWorldState CreateStubWorldState()
        {
            var entities = new IWorldEntity[5, 5];

            entities[0, 0] = new CollectableWorldEntity {Type = CollectableWorldEntityType.Food, Value = 1};
            entities[1, 1] = new CollectableWorldEntity {Type = CollectableWorldEntityType.Wood, Value = 2};

            entities[2, 0] = new CollectableWorldEntity {Type = CollectableWorldEntityType.Iron, Value = 3};
            entities[2, 2] = new Tribe {Name = "tribe1"};

            entities[0, 1] = new BlockingWorldEntity {Type = BlockingWorldEntityType.Wall};
            entities[1, 0] = new BlockingWorldEntity {Type = BlockingWorldEntityType.Water};
            entities[2, 1] = new BlockingWorldEntity {Type = BlockingWorldEntityType.NotReachable};

            entities[3, 3] = new Tribe {Name = "tribe2"};

            return new WorldState(entities);
        }
    }
}
