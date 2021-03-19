using Ev.Tests.Common.Builders;

namespace Ev.Tests.Common
{
    public static class Mocks
    {
        public static TribeAgentBuilder ITribeAgent => new();

        public static TribeBuilder ITribe() => new();
    }
}