using Ev.Infrastructure.Core;

namespace Ev.Infrastructure
{
    public static class PlatformFactory
    {
        public static readonly IPlatform Local  = new LocalPlatform(new Mapper());
        public static readonly IPlatform Remote = new RemotePlatform();
    }
}