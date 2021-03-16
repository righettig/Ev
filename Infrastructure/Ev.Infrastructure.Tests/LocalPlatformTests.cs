using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using ITribe = Ev.Domain.Server.Core.ITribe;

namespace Ev.Infrastructure.Tests
{
    static class Stubs
    {
        public static readonly IWorldState IIWorldState = new Mock<IWorldState>().Object;

        public static readonly ITribeAgent ITribeAgent = new Mock<ITribeAgent>().Object;
        
        public static readonly IGame IGame = new Mock<IGame>().Object;

        public static readonly IMapper IMapper = new Mock<IMapper>().Object;

        public static readonly ITribe ITribe = new Mock<ITribe>().Object;
    }

    class TribeAgentBuilder
    {
        private readonly Mock<ITribeAgent> _mock = new();

        public TribeAgentBuilder WithName(string name)
        {
            _mock.SetupGet(m => m.Name).Returns(name);
            return this;
        }

        public ITribeAgent Build()
        {
            return _mock.Object;
        }
    }

    static class Mocks
    {
        public static TribeAgentBuilder ITribeAgent => new();
    }

    [TestClass]
    public class LocalPlatformTests
    {
        private readonly LocalPlatform uat = new(Stubs.IMapper);

        #region RegisterAgent

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentNullException_If_Game_Is_Null()
        {
            // Arrange
            var agents = new[] {Stubs.ITribeAgent, Stubs.ITribeAgent};

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => uat.RegisterAgent(null, agents));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentNullException_If_Agents_Is_Null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => uat.RegisterAgent(Stubs.IGame, null));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentException_If_Agents_Is_Empty()
        {
            Assert.ThrowsException<ArgumentException>(() => uat.RegisterAgent(Stubs.IGame));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentException_When_Registering_Two_Agents_With_Same_Name()
        {
            // Arrange
            var agents = new[]
            {
                Mocks.ITribeAgent.WithName("agent1").Build(),
                Mocks.ITribeAgent.WithName("agent1").Build()
            };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => uat.RegisterAgent(Stubs.IGame, agents));
        }

        [TestMethod]
        public void RegisterAgent_Should_Invoke_RegisterAgent_For_Every_Agent()
        {
            // Arrange
            var agents = new[]
            {
                Mocks.ITribeAgent.WithName("agent1").Build(),
                Mocks.ITribeAgent.WithName("agent2").Build()
            };

            var mock = new Mock<IGame>();

            // Act
            uat.RegisterAgent(mock.Object, agents);

            // Assert
            mock.Verify(m => m.RegisterAgent("agent1", It.IsAny<Color>()));
            mock.Verify(m => m.RegisterAgent("agent2", It.IsAny<Color>()));
        }

        #endregion

        #region Update

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_WorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.Update(null, Stubs.ITribe));
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.Update(Stubs.IIWorldState, null));
        }

        #endregion

        private static void ShouldThrowArgumentNullException(Action action)
        {
            Assert.ThrowsException<ArgumentNullException>(action);
        }
    }
}
