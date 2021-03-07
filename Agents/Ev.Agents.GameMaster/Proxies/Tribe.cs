using Ev.Agents.GameMaster.Proxies.Core;
using Ev.Tribe;
using Grpc.Core;
using static Ev.Tribe.EvTribe;

namespace Ev.Agents.GameMaster.Proxies
{
    class Tribe : ITribeProxy
    {
        public string Name => _name;

        private readonly string _name;

        private readonly Channel _tribeChannel;
        private readonly EvTribeClient _tribeClient;

        public Tribe(string name, string url)
        {
            _name = name;

            _tribeChannel = new Channel(url, ChannelCredentials.Insecure);
            _tribeClient = new EvTribeClient(_tribeChannel);
        }

        public void Start() // TODO: this could be async
        {
            // StartAsync
            _tribeClient.Start(new StartRequest());
        }

        public DoMoveReply DoMove(DoMoveRequest request)
        {
            return _tribeClient.DoMove(request);
        }

        public void End()
        {
            _tribeClient.End(new EndRequest());
        }

        public void Shutdown()
        {
            _tribeChannel.ShutdownAsync().Wait();
        }
    }
}