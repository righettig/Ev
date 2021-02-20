using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Ev.Domain.Tests.Unit
{
    using static Helpers.TestHelpers;

    [TestClass]
    public class WorldTests
    {
        private readonly RandomWorld uat;

        public WorldTests()
        {
            uat = new RandomWorld(
                8, 
                new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, 
                new Mock<IRandom>().Object
            );
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_When_Rnd_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var world = new RandomWorld(10, new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 }, null);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_When_WorldResources_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var world = new RandomWorld(10, null, new Mock<IRandom>().Object);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentOutOfRangeException_When_Size_Is_Not_Positive()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var world = new RandomWorld(0, new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, new Mock<IRandom>().Object);
            });
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentOutOfRangeException_When_Too_Many_Collectables()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var world = new RandomWorld(2, new WorldResources { FoodCount = 10, WoodCount = 10, IronCount = 10 }, new Mock<IRandom>().Object);
            });
        }

        [TestMethod]
        public void Ctor_Should_Assign_Size()
        {
            var world = new RandomWorld(10, new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 }, new Utils.Random());

            Assert.AreEqual(10, world.Size);
            Assert.AreEqual(10, world.State.GetLength(0));
            Assert.AreEqual(10, world.State.GetLength(1));
        }

        [TestMethod]
        public void Ctor_Should_Set_Finished_To_False()
        {
            var world = new RandomWorld(10, new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 }, new Utils.Random());

            Assert.IsFalse(world.Finished);
        }

        [TestMethod]
        public void Ctor_Should_Set_Winner_To_Null()
        {
            var world = new RandomWorld(10, new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 }, new Utils.Random());

            Assert.IsNull(world.Winner);
        }

        [TestMethod]
        public void Ctor_Should_Allocate_Correct_Number_Of_Resources()
        {
            var world = new RandomWorld(10, new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 }, new Utils.Random());

            var foodCount = 0;
            var woodCount = 0;
            var ironCount = 0;

            for (var i = 0; i < world.Size; i++) 
            {
                for (var j = 0; j < world.Size; j++)
                {
                    if (world.State[i, j] is Food) foodCount++;
                    if (world.State[i, j] is Wood) woodCount++;
                    if (world.State[i, j] is Iron) ironCount++;
                }
            }

            Assert.AreEqual(1, foodCount);
            Assert.AreEqual(1, woodCount);
            Assert.AreEqual(1, ironCount);
        }

        #endregion

        #region Update
        
        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_When_Tribe_Is_Null() 
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.Update(null, new Mock<IGameAction>().Object, 0, new Mock<IGameActionProcessor>().Object);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_When_Move_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.Update(new Mock<ITribe>().Object, null, 0, new Mock<IGameActionProcessor>().Object);
            });
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_When_ActionProcessor_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.Update(new Mock<ITribe>().Object, new Mock<IGameAction>().Object, 0, null);
            });
        }

        [TestMethod]
        public void Update_Should_Remove_Dead_Tribe()
        {
            // Arrange
            var tribe = TestTribe(0);

            // Act
            uat.Update(tribe, new Mock<IGameAction>().Object, 1, new Mock<IGameActionProcessor>().Object);

            // Assert
            Assert.AreEqual(1, tribe.DeadAtIteration);
        }

        [TestMethod]
        public void Update_Should_Set_Finished()
        {
            // Arrange
            var tribe = TestTribe(100);
            var world = new RandomWorld(8, new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, new Mock<IRandom>().Object, new[] { tribe });

            // Act
            world.Update(tribe, new Mock<IGameAction>().Object, 1, new Mock<IGameActionProcessor>().Object);

            // Assert
            Assert.IsTrue(world.Finished);
        }

        [TestMethod]
        public void Update_Should_Set_Winner()
        {
            // Arrange
            var tribe = TestTribe(100);
            var world = new RandomWorld(8, new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, new Mock<IRandom>().Object, new[] { tribe });

            // Act
            world.Update(tribe, new Mock<IGameAction>().Object, 1, new Mock<IGameActionProcessor>().Object);

            // Assert
            Assert.AreSame(tribe, world.Winner);
        }

        #endregion

        #region WipeTribe

        [TestMethod]
        public void WipeTribe_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.WipeTribe(null, 1);
            });
        }

        [TestMethod]
        public void WipeTribe_Should_Set_DeadAtIteration()
        {
            // Arrange
            var tribe = TestTribe(0);

            // Act
            uat.WipeTribe(tribe, 1);

            // Assert
            Assert.AreEqual(1, tribe.DeadAtIteration);
        }

        [TestMethod]
        public void WipeTribe_Should_Set_State_To_Null()
        {
            // Arrange
            var tribe = TestTribe(0);
            tribe.Position = (1, 1);
            uat.State[1, 1] = tribe;

            // Act
            uat.WipeTribe(tribe, 1);

            // Assert
            Assert.IsNull(uat.State[1, 1]);
        }

        #endregion

        #region WithTribe

        [TestMethod]
        public void WithTribe_Should_Throw_ArgumentNullException_When_Behaviour_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                uat.WithTribe("t1", Color.DarkYellow, null);
            });
        }

        [TestMethod]
        public void WithTribe_Should_Assign_Tribe_State()
        {
            // Arrange
            var rnd = new Mock<IRandom>();
            rnd.Setup(m => m.Next(It.IsAny<int>())).Returns(1);

            var uat = new RandomWorld(8, new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, rnd.Object);

            // Act
            uat.WithTribe("t1", Color.DarkYellow, new Mock<ITribeBehaviour>().Object);

            // Assert
            Assert.IsTrue(uat.State[1, 1] is ITribe);
            Assert.AreEqual("t1", (uat.State[1, 1] as ITribe).Name);
            Assert.AreEqual(Color.DarkYellow, (uat.State[1, 1] as ITribe).Color);
            Assert.AreSame(uat.State[1, 1], uat.Tribes[0]);
        }

        #endregion

        #region GetWorldState

        [TestMethod]
        public void GetWorldState_Should_Throw_ArgumentNullException_When_Tribe_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var state = uat.GetWorldState(null);
            });
        }

        // TODO: check size should be 1 + 2 * WORLD_STATE_SIZE

        //[TestMethod]
        public void GetWorldState_Should_Return_The_Correct_WorldState_Based_On_Tribe()
        {
            /*
            
                X X X 0 0
                X X X 0 0
                T X X 0 0
                X X X 0 0 
                X X X 0 0 

                0 0 X X X
                0 0 X X X
                0 0 X X T
                0 0 X X X 
                0 0 X X X 

                X X X X X
                X X X X X
                X X T X X
                X X X X X 
                X X X X X

                0 0 0 0 0
                0 0 0 0 0
                0 0 X X X
                0 0 X X X 
                0 0 X X T

             */
        }

        #endregion

        #region CanMove

        #endregion

        #region Move

        #endregion

        #region GetAliveTribes

        [TestMethod]
        public void GetAliveTribes_Should_Return_All_Tribes_With_NonNegative_Population()
        {
            // Arrange
            var tribe1 = TestTribe(0);
            var tribe2 = TestTribe(100);

            var world = new RandomWorld(8, new WorldResources { FoodCount = 0, WoodCount = 0, IronCount = 0 }, new Mock<IRandom>().Object, new[] { tribe1, tribe2 });

            // Act
            var actual = world.GetAliveTribes();

            // Assert
            Assert.AreEqual(1, actual.Length);
            Assert.AreSame(tribe2, actual[0]);
        }

        #endregion
    }
}