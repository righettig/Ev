using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Predictors;
using Ev.Domain.Server.Processors;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure.Core;
using Ev.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Ev.Helpers.Debug;

namespace Ev.Game.Server
{
    public class Game : IGame
    {
        private readonly IWorld _world;
        private readonly IPlatform _platform;
        private readonly EvGameHistory _history;
        private readonly IGameActionProcessor _actionProcessor;

        public Game(IPlatform platform, IWorld world, IRandom rnd)
        {
            if (rnd == null) throw new ArgumentNullException(nameof(rnd));

            _world = world ?? throw new ArgumentNullException(nameof(world));
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));

            // TODO: for this to be useful I need to be able to record the game/world parameters too
            _history = new EvGameHistory();
            _actionProcessor = new GameActionProcessor(new AttackOutcomePredictor(rnd));
        }

        // onRegisterAgent
        public void RegisterAgent(string agentName, Color agentColor)
        {
            if (string.IsNullOrWhiteSpace(agentName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(agentName));

            _world.AddTribe(agentName, agentColor);
            
            // TODO: if #agents is OK -> start game loop
        }

        public async Task GameLoop(EvGameOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var finished = false;
            var iteration = 0;

            _platform.OnGameStart();

            // TODO: delegate to Spectator?
            Dump(_world, iteration); 

            do
            {
                var alive = _world.GetAliveTribes();

                for (var i = 0; i < alive.Length; i++)
                {
                    _platform.OnTurnStart();

                    var tribe = alive[i];
                    var state = _world.GetWorldState(tribe);

                    //DumpWorldState(state as WorldState);

                    var move = _platform.Update(state, tribe);

                    if (move is AttackAction a)
                    {
                        // TODO: it would be nice if we could delegate this responsibility to Platform, so that game logic already sees the final server action
                        // without having to perform this mapping logic.
                        // For now I'm gonna add TargetName as an additional property in the server action model.
                        // mapping client-side action to server-side
                        a.Target = alive.First(el => el.Name == a.TargetName);
                    }

                    _history.Add((move, state));

                    finished = _world.Update(tribe, move, iteration, _actionProcessor);

                    _platform.OnTurnEnd();

                    if (options.RenderEachTurn)
                    {
                        var next = alive[(i + 1) % alive.Length];
                        Dump(_world, iteration, move, next);
                    }

                    if (options.WaitAfterEachMove)
                    {
                        Console.ReadLine();
                    }
                }

                iteration++;

            } while (!finished);

            _platform.OnGameEnd();

            // TODO: delegate to Spectator?
            Dump(_world, iteration);

            if (options.DumpWinnerHistory)
            {
                var winnerHistory = _history.States.Where(el => el.Item1.Tribe.Name == _world.Winner.Name);

                DumpHistory(winnerHistory.Select(el => el.Item1).ToList());

                var serializer = new EvGameHistorySerializer();
                await serializer.SaveToFile(winnerHistory, options.WinnerHistoryFilename);
            }
        }
    }
}
