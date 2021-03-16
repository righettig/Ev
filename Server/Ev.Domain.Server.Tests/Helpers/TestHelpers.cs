using Ev.Domain.Server.Core;
using Moq;

namespace Ev.Domain.Server.Tests.Helpers
{
    public static class TestHelpers 
    {
        public static ITribe TestTribe(int population)
        {
            var result = new Mock<ITribe>()
                .SetupProperty(m => m.Population)
                .SetupProperty(m => m.Position)
                .SetupProperty(m => m.DeadAtIteration)
                .SetupProperty(m => m.Wood)
                .SetupProperty(m => m.Iron)
                .SetupProperty(m => m.Defense)
                .SetupProperty(m => m.Attack)
                .SetupProperty(m => m.LockedForNTurns)
                .SetupProperty(m => m.BusyDoing)
                .Object;

            result.Population = population;

            return result;
        }
    }
}