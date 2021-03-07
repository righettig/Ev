using Ev.Agents.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using Ev.Game;
using Ev.Helpers;
using Ev.Server;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Ev.Server.EvGameMaster;
using static System.Console;

namespace Ev.Agents.GameMaster
{
    public class GameMasterAgent : EvGameMasterBase
    {
        const int PORT = 30051;

        private readonly List<Proxies.Tribe> _tribes = new();
        private readonly List<Proxies.Spectator> _spectators = new();

        private readonly GameOptions _options;
        private readonly GameStateSerialiser _gameStateSerialiser;

        private readonly IWorld _world;
        private readonly IRandom _rnd;

        private readonly int _howManyTribes;

        private bool _started;
        private int _joinedTribes;

        private Grpc.Core.Server _server;

        public GameMasterAgent(GameOptions options, IWorld world, IRandom rnd, int howManyTribes)
        {
            _options       = options ?? throw new ArgumentNullException(nameof(options));
            _world         = world   ?? throw new ArgumentNullException(nameof(world));
            _rnd           = rnd     ?? throw new ArgumentNullException(nameof(rnd));

            _howManyTribes = howManyTribes;

            _gameStateSerialiser = new GameStateSerialiser();
        }

        public override Task<JoinReply> Join(JoinRequest request, ServerCallContext context)
        {
            if (request.IsTribe)
            {
                Write($"{request.Name} has just joined. ");

                _tribes.Add(new Proxies.Tribe(request.Name, request.Url));

                _world.AddTribe(request.Name, Enum.Parse<Color>(request.Color));

                _joinedTribes++;

                if (_joinedTribes == _howManyTribes)
                {
                    WriteLine("Starting game" + Environment.NewLine);

                    _tribes.ForEach(el => el.Start());
                    _spectators.ForEach(el => el.Start());

                    _started = true;

                    _ = CreateGame().GameLoop();
                }
                else
                {
                    WriteLine($"Waiting for {_howManyTribes - _joinedTribes} more tribe(s) to join");
                }
            }
            else
            {
                WriteLine($"Spectator {request.Name} has just joined");

                var spectator = new Proxies.Spectator(request.Url);

                // if game has already started send the current game state
                if (_started)
                {
                    spectator.Update(_world);
                }

                _spectators.Add(spectator);
            }

            return Task.FromResult(new JoinReply());
        }

        public GameMasterAgent Start()
        {
            Write($"Starting GAME MASTER process at port: {PORT} ... ");

            _server = new Grpc.Core.Server
            {
                Services = { BindService(this) },
                Ports = { new ServerPort("localhost", PORT, ServerCredentials.Insecure) }
            };

            _server.Start();

            WriteLine("started!" + Environment.NewLine);
            WriteLine($"Waiting for {_howManyTribes} tribes to join" + Environment.NewLine);

            return this;
        }

        public void Shutdown()
        {
            _spectators.ForEach(el => el.Shutdown());
            _tribes.ForEach(el => el.Shutdown());

            _server.ShutdownAsync().Wait();
        }

        private void UpdateSpectators()
        {
            // each spectator need to have access to
            // IWorld, int IGameAction, ITribe
            // Helpers.Debug.Dump(_world, iteration, move, next);

            foreach (var spectator in _spectators)
            {
                spectator.Update(_world);
            }
        }

        private void End()
        {
            // TODO: send final state to both tribes and spectators

            _tribes.ForEach(el => el.End());
            _spectators.ForEach(el => el.End());

            WriteLine($"Game has ended. The winner is {_world.Winner.Name}!" + Environment.NewLine);
            WriteLine("Press any key to exit...");
        }

        #region Game event handlers

        private void OnGameStart(object sender, IWorld e)
        {
            Debug.Dump(_world, 0);

            UpdateSpectators();
        }

        private void OnGameEnd(object sender, EventArgs e) => End();

        private void OnStartTurn(object sender, EventArgs e)
        {
            // TODO: send message to notify the other Tribes that "It's now Tribe1 turn ..."
        }

        private void OnEndTurn(object sender, EventArgs e)
        {
            // TODO: send message to notify the other Tribes that "Tribe1 turn has ended!"
            UpdateSpectators();
        }

        private BaseGame CreateGame()
        {
            var game = new EvGameRemote(_options, _world, _rnd, _tribes, _gameStateSerialiser);

            game.OnGameStart += OnGameStart;
            game.OnStartTurn += OnStartTurn;
            game.OnEndTurn   += OnEndTurn;
            game.OnGameEnd   += OnGameEnd;

            return game;
        }

        #endregion
    }
}