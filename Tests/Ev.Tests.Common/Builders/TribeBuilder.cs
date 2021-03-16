using Moq;

namespace Ev.Tests.Common.Builders
{
    public class TribeBuilder
    {
        private readonly Mock<Domain.Server.Core.ITribe> _mock = new();

        private int _population;

        public TribeBuilder WithPopulation(int population)
        {
            _population = population;

            _mock.SetupGet(m => m.Population).Returns(() => _population);
            _mock.SetupSet(p => p.Population = It.IsAny<int>()).Callback<int>(value => _population = value);

            return this;
        }

        public Domain.Server.Core.ITribe Build() => _mock.Object;
    }
}