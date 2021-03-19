using Ev.Domain.Client.Core;
using Moq;

namespace Ev.Tests.Common.Builders
{
    public class TribeAgentBuilder
    {
        private readonly Mock<ITribeAgent> _mock = new();

        public TribeAgentBuilder WithName(string name)
        {
            _mock.SetupGet(m => m.Name).Returns(name);
            return this;
        }

        public ITribeAgent Build() => _mock.Object;
    }
}