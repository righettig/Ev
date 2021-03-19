using Ev.Domain.Client.Core;
using Moq;

namespace Ev.Tests.Common
{
    public class ClientStubs
    {
        public readonly ITribe ITribe = new Mock<ITribe>().Object;

        public readonly IGameAction IGameAction = new Mock<IGameAction>().Object;
    }
}
