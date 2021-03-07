using Ev.Agents.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Server;
using Ev.Tribe;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using Ev.Domain.Actions.Core;
using Ev.Helpers;
using static Ev.Server.EvGameMaster;
using static Ev.Tribe.EvTribe;

namespace Ev.Agents.Tribe
{
    public class TribeAgent : EvTribeBase, IAgent<TribeAgent>
    {
        const int PORT = 30052;
        private readonly int _port;

        private readonly string _name;
        private readonly Color _color;

        private readonly ITribeBehaviour _behaviour;

        private Grpc.Core.Server _server;
        private Channel _channel;
        private EvGameMasterClient _client;

        public TribeAgent(string name, Color color, ITribeBehaviour behaviour, int? port = null)
        {
            _name = name;
            _color = color;
            _behaviour = behaviour;

            _port = port ?? PORT;
        }

        public override Task<Empty> Start(StartRequest request, ServerCallContext context)
        {
            Console.WriteLine("Game started");

            // do I really need to return something here?
            return Task.FromResult(new Empty());
        }

        public override Task<DoMoveReply> DoMove(DoMoveRequest request, ServerCallContext context)
        {
            if (!request.Busy)
            {
                var (x, y) = (request.TribeState.Position.X, request.TribeState.Position.Y);
                var (x_prev, y_prev) = (request.TribeState.PrevPosition.X, request.TribeState.PrevPosition.Y);

                var self = new Domain.Entities.TribeState
                {
                    Population = request.TribeState.Population,
                    Position = (x, y),
                    PrevPosition = (x_prev, y_prev),
                    //Defense = request.TribeState.Defense,
                    //Attack = request.TribeState.Attack,
                    Wood = request.TribeState.Wood,
                    Iron = request.TribeState.Iron
                };

                var serialiser = new GameStateSerialiser();
                var worldState = serialiser.DeserialiseWorldState(request, _color);

                Debug.DumpWorldState(worldState as WorldState);

                _behaviour.State = worldState;
                var move = _behaviour.DoMove(worldState, self);

                // TODO: need to make sure client code does not end when pressing one of these keys
                if (move is PlayerControlledGameAction)
                {
                    Debug.DumpActions();
                    move = Debug.ReadAction(worldState);
                }

                // TODO: to be expanded with stats summary
                // Console.WriteLine((_behaviour as TribeBehaviour).DebugBehaviour() + Environment.NewLine);

                var reply = serialiser.CreateDoMoveReply(move);

                return Task.FromResult(reply);
            }

            return Task.FromResult(new DoMoveReply());
        }

        public override Task<Empty> End(EndRequest request, ServerCallContext context)
        {
            // Helpers.Debug.Dump(request.GameResult); // world

            return Task.FromResult(new Empty());
        }

        public TribeAgent Start()
        {
            Console.Write($"Starting TRIBE process at port: {_port} ... ");

            _server = new Grpc.Core.Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort("localhost", _port, ServerCredentials.Insecure) }
            };

            _server.Start();

            Console.WriteLine("started!" + Environment.NewLine);

            return this;
        }

        public TribeAgent Connect(string serverUrl)
        {
            Console.Write($"Connecting to server at {serverUrl} ... ");

            _channel = new Channel(serverUrl, ChannelCredentials.Insecure);
            _client = new EvGameMasterClient(_channel);

            try
            {
                var reply = _client.Join(new JoinRequest { Name = _name, Color = _color.ToString(), Url = "127.0.0.1:" + _port, IsTribe = true });
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
