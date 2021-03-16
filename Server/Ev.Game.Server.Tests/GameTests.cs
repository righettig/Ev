using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Game.Server.Tests
{
    [TestClass]
    public class GameTests
    {
        private IPlatform _platform;
        private IWorld    _world;
        private IRandom   _random;

        public GameTests()
        {
            _platform = Stubs.IPlatform;
            _world    = Stubs.IWorld;
            _random   = Stubs.IRandom;
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Platform_Is_Null()
        {
            _platform = null;

            ShouldThrowArgumentNullException(CreateUat);
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_World_Is_Null()
        {
            _world = null;

            ShouldThrowArgumentNullException(CreateUat);
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Random_Is_Null()
        {
            _random = null;

            ShouldThrowArgumentNullException(CreateUat);
        }

        #endregion

        [TestMethod]
        public async Task GameLoop_Should_Throw_ArgumentNullException_If_EvGameOptions_Is_Null()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => CreateUat().GameLoop(null));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void RegisterAgent_Should_Throw_ArgumentException_If_AgentName_Is_NullEmptyOrWhiteSpace(string agentName)
        {
            ShouldThrowArgumentException(() => CreateUat().RegisterAgent(agentName, Color.Black));
        }

        private static void ShouldThrowArgumentNullException(Func<Game> func)
        {
            Assert.ThrowsException<ArgumentNullException>(func);
        }

        private Game CreateUat() => new(_platform, _world, _random);
    }
}
