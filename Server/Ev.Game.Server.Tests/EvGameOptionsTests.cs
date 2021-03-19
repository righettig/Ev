using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ev.Game.Server.Tests
{
    [TestClass]
    public class EvGameOptionsTests
    {
        [TestMethod]
        public void Ctor_Should_Throw_ArgumentOutOfRangeException_If_Players_Is_Zero()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new EvGameOptions(0));
        }

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentOutOfRangeException_If_Players_Is_Negative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new EvGameOptions(-1));
        }

        [TestMethod]
        public void Ctor_Should_Assign_Players_If_Value_Is_Positive()
        {
            var uat = new EvGameOptions(1);

            Assert.AreEqual(1, uat.Players);
        }
    }
}