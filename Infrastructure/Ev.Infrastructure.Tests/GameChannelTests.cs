using Ev.Domain.Client.Core;
using Ev.Infrastructure.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Infrastructure.Tests
{
    [TestClass]
    public class GameChannelTests
    {
        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Game_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new GameChannel(null, Stubs.IPlatform));
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Platform_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new GameChannel(Stubs.IGame, null));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentNullException_If_Agents_Is_Null()
        {
            var uat = new GameChannel(Stubs.IGame, Stubs.IPlatform);

            ShouldThrowArgumentNullException(() => uat.RegisterAgent(null));
        }

        [TestMethod]
        public void RegisterAgent_Should_Throw_ArgumentException_If_Agents_Is_Empty()
        {
            var uat = new GameChannel(Stubs.IGame, Stubs.IPlatform);

            var agents = new List<ITribeAgent>().ToArray();

            ShouldThrowArgumentException(() => uat.RegisterAgent(agents));
        }

        [TestMethod]
        public void RegisterAgent_Should_Invoke_RegisterAgent_On_The_Platform()
        {
            // Arrange
            var platform = new Mock<IPlatform>();

            var game = Stubs.IGame;
            var agents = new[] {Stubs.ITribeAgent};

            var uat = new GameChannel(Stubs.IGame, platform.Object);

            // Act
            uat.RegisterAgent(agents);

            // Assert
            platform.Verify(m => m.RegisterAgent(game, agents), Times.Once);
        }
    }
}
