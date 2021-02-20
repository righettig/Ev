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
                .SetupProperty(m => m.Wood)
                .SetupProperty(m => m.Iron)
                .SetupProperty(m => m.Defense)
                .SetupProperty(m => m.LockedForNTurns)
                .SetupProperty(m => m.BusyDoing)
                .Object;

            result.Population = population;

            return result;
        }
    }
}