using Ev.Common.Core;
using Ev.Domain.Server.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ev.Tests.Common.TestHelpers;
using Mocks = Ev.Tests.Common.Mocks;

namespace Ev.Infrastructure.Tests
{
    [TestClass]
    public class LocalPlatformTests
    {
        private readonly LocalPlatform _uat = new(Stubs.IMapper);

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Mapper_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new LocalPlatform(null));
        }

        #region RegisterAgent

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentNullException_If_Game_Is_Null()
        {
            // Arrange
            var agents = new[] {Stubs.ITribeAgent, Stubs.ITribeAgent};

            // Act & Assert
            ShouldThrowArgumentNullException(() => _uat.RegisterAgent(null, agents));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentNullException_If_Agents_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.RegisterAgent(Stubs.IGame, null));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentException_If_Agents_Is_Empty()
        {
            ShouldThrowArgumentException(() => _uat.RegisterAgent(Stubs.IGame));
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
            ShouldThrowArgumentException(() => _uat.RegisterAgent(Stubs.IGame, agents));
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
            _uat.RegisterAgent(mock.Object, agents);

            // Assert
            mock.Verify(m => m.RegisterAgent("agent1", It.IsAny<Color>()));
            mock.Verify(m => m.RegisterAgent("agent2", It.IsAny<Color>()));
        }

        #endregion

        #region Update

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_WorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.Update(null, Stubs.Server.ITribe));
        }

        [TestMethod]
        public void Update_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.Update(Stubs.IIWorldState, null));
        }

        #endregion
    }
}
