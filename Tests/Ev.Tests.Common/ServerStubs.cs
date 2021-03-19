using Ev.Domain.Server.Core;
using Moq;

namespace Ev.Tests.Common
{
    public class ServerStubs
    {
        public readonly ITribe ITribe = new Mock<ITribe>().Object;

        public readonly IGameAction IGameAction = new Mock<IGameAction>().Object;
    }
}