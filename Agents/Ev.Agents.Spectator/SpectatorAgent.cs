using Ev.Agents.Core;
using Ev.Server;
using Ev.Spectator;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static Ev.Server.EvGameMaster;
using static Ev.Spectator.EvSpectator;

namespace Ev.Agents.Spectator
{
    class SpectatorAgent : EvSpectatorBase, IAgent<SpectatorAgent>
    {
        const int PORT = 30053;

        private readonly string _name;

        private Grpc.Core.Server _server;
        private Channel _channel;
        private EvGameMasterClient _client;

        public SpectatorAgent(string name)
        {
            _name = name;
        }

        public override Task<StartReply> Start(StartRequest request, ServerCallContext context)
        {
            return Task.FromResult(new StartReply());
        }

        public override Task<UpdateReply> Update(UpdateRequest request, ServerCallContext context)
        {
            //var foo = new GameSerialiser();
            //var (_world, iteration, move, next) = foo.deserialize(request);

            // TODO need some deserialise thing going on here too
            // Helpers.Debug.Dump(_world, iteration, move, next);

            return Task.FromResult(new UpdateReply());
        }

        public override Task<EndReply> End(EndRequest request, ServerCallContext context)
        {
            return Task.FromResult(new EndReply());
        }

        public SpectatorAgent Start()
        {
            Console.Write($"Starting SPECTATOR process at port: {PORT} ... ");

            _server = new Grpc.Core.Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort("localhost", PORT, ServerCredentials.Insecure) }
            };

            _server.Start();

            Console.WriteLine("started!" + Environment.NewLine);

            return this;
        }

        public SpectatorAgent Connect(string serverUrl)
        {
            Console.Write($"Connecting to server at {serverUrl} ... ");

            _channel = new Channel(serverUrl, ChannelCredentials.Insecure);
            _client = new EvGameMasterClient(_channel);

            try
            {
                var reply = _client.Join(new JoinRequest { Name = _name, Url = "127.0.0.1:" + PORT, IsTribe = false });
            }
            catch (RpcException exception) when (exception.StatusCode == StatusCode.Unavailable)
            {
                Console.WriteLine("FAILED!");

                return null;
            }

            Console.WriteLine("done!");

            return this;
        }

        public void Shutdown()
        {
            _channel.ShutdownAsync().Wait();
            _server.ShutdownAsync().Wait();
        }
    }
}
