using Ev.Common.Core;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests
{
    [TestClass]
    public class TribeAgentTests
    {
        private readonly TribeAgent uat = new("t1", Color.Black, Stubs.ITribeBehaviour);

        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_TribeBehaviour_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new TribeAgent("t1", Color.Black, null));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Ctor_Should_Throw_ArgumentException_If_Name_Is_NullEmptyOrWhiteSpace(string name)
        {
            ShouldThrowArgumentException(() => new TribeAgent(name, Color.Black, Stubs.ITribeBehaviour));
        }

        [TestMethod]
        public void Ctor_Should_Set_Default_Values()
        {
            var uat = new TribeAgent("t1", Color.Black, Stubs.ITribeBehaviour);

            // Assert
            Assert.AreEqual("t1", uat.Name);
            Assert.AreEqual(Color.Black, uat.Color);
            Assert.AreSame(Stubs.ITribeBehaviour, uat.Behaviour);
        }

        [TestMethod]
        public void OnBeforeMove_Should_Throw_ArgumentNullException_If_WorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.OnBeforeMove(null, Stubs.Client.ITribe));
        }

        [TestMethod]
        public void OnBeforeMove_Should_Throw_ArgumentNullException_If_Tribe_Is_Null()
        {
            ShouldThrowArgumentNullException(() => uat.OnBeforeMove(Stubs.IIWorldState, null));
        }
    }
}