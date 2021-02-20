using Ev.Domain.Entities.Core;
using Moq;

namespace Ev.Domain.Tests.Unit.Helpers
{
    public class TestHelpers 
    {
        public static ITribe TestTribe(int population)
        {
            var result = new Mock<ITribe>()
                .SetupProperty(m => m.Population)
                .SetupProperty(m => m.Position)
                .SetupProperty(m => m.DeadAtIteration)
                .Object;

            result.Population = population;

            return result;
        }
    }
}