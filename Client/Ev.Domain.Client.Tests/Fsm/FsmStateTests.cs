using Ev.Domain.Client.Behaviours.Fsm;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.Fsm
{
    [TestClass]
    public class FsmStateTests
    {
        [TestMethod]
        public void Ctor_Should_Throw_ArgumentNullException_If_Id_Is_Null()
        {
            ShouldThrowArgumentNullException(() => new FsmState(null));
        }

        [TestMethod]
        public void Ctor_Should_Assign_Id()
        {
            var id = Stubs.IEnumeration;

            var uat = new FsmState(id);

            Assert.AreSame(id, uat.Id);
        }
    }
}
