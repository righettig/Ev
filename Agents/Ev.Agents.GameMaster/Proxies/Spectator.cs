using Ev.Agents.GameMaster.Proxies.Core;
using Ev.Domain.World.Core;
using Ev.Spectator;
using Grpc.Core;
using static Ev.Spectator.EvSpectator;

namespace Ev.Agents.GameMaster.Proxies
{
    public class Spectator : ISpectatorProxy
    {
        private readonly Channel _spectatorChannel;
        private readonly EvSpectatorClient _spectatorClient;

        public Spectator(string url)
        {
            _spectatorChannel = new Channel(url, ChannelCredentials.Insecure);
            _spectatorClient = new EvSpectatorClient(_spectatorChannel);
        }

        public void Start() // TODO: this could be async
        {
            // StartAsync
            _spectatorClient.Start(new StartRequest());
        }

        public void Update(IWorld world)
        {
            _spectatorClient.Update(new UpdateRequest());
        }

        public void End()
        {
            _spectatorClient.End(new EndRequest());
        }

        public void Shutdown()
        {
            _spectatorChannel.ShutdownAsync().Wait();
        }
    }
}