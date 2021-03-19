using Ev.Game.Server;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Infrastructure.Tests
{
    [TestClass]
    public class GameFactoryTests
    {
        [TestMethod]
        public void Local_Should_Throw_ArgumentNullException_If_Options_Is_Null()
        {
            ShouldThrowArgumentNullException(() => GameFactory.Local(null, Stubs.IWorld));
        }

        [TestMethod]
        public void Local_Should_Throw_ArgumentNullException_If_World_Is_Null()
        {
            ShouldThrowArgumentNullException(() => GameFactory.Local(new EvGameOptions(1), null));
        }
    }
}
