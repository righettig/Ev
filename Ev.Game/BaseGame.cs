using Ev.Domain.Actions.Core;
using Ev.Domain.Actions.Core.Processors;
using Ev.Domain.Actions.Processors;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using Ev.Helpers;
using Ev.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ev.Game
{
    public abstract class BaseGame
    {
        protected readonly IWorld _world;

        private readonly GameOptions _options;
        private readonly IGameActionProcessor _gameActionProcessor;

        protected BaseGame(GameOptions options, IWorld world, IRandom rnd)
        {
            _options = options;
            _world = world;
            _gameActionProcessor = new GameActionProcessor(new AttackOutcomePredictor(rnd));
        }
        
        public event EventHandler<IWorld> OnGameStart;

        public event EventHandler OnGameEnd;

        public event EventHandler OnStartTurn;

        public event EventHandler OnEndTurn;

        protected abstract IGameAction OnDoMove(IWorldState state, ITribe tribe);

        public async Task GameLoop()
        {
            var finished = false;
            var iteration = 0;

            // TODO: for this to be useful I need to be able to record the game/world parameters too
            var history = new GameHistory();

            OnGameStart?.Invoke(this, _world);

            do
            {
                var alive = _world.GetAliveTribes();

                for (var i = 0; i < alive.Length; i++)
                {
                    var tribe = alive[i];

                    OnStartTurn?.Invoke(this, EventArgs.Empty);

                    var state = _world.GetWorldState(tribe);

                    var move = OnDoMove(state, tribe);

                    move.Tribe = tribe;

                    // mapping client-side action to server-side
                    if (move is Domain.Actions.AttackAction a)
                    {
                        a.Target = alive.First(el => el.Name == a.TargetName);
                    }

                    history.Add((move, state));

                    finished = _world.Update(tribe, move, iteration, _gameActionProcessor);

                    OnEndTurn?.Invoke(this, EventArgs.Empty);

                    if (_options.RenderEachTurn)
                    {
                        // TODO: move in "Dump"?
                        var next = alive[(i + 1) % alive.Length];

                        Debug.Dump(_world, iteration, move, next);
                    }

                    if (_options.WaitAfterEachMove)
                    {
                        Console.ReadLine();
                    }
                }

                iteration++;

            } while (!finished);

            Debug.Dump(_world, iteration);

            if (_options.DumpWinnerHistory)
            {
                var winnerHistory = 
                    history.States.Where(el => el.Item1.Tribe.Name == _world.Winner.Name);

                Debug.DumpHistory(winnerHistory.Select(el => el.Item1).ToList());

                var serializer = new EvGameHistorySerializer();
                await serializer.SaveToFile(winnerHistory, _options.WinnerHistoryFilename);

                // TODO: for the game history to be useful when future tribe agents training and analysis we should probably be able to dump the entire map in a file
                // await serializer.SaveMapToFile(world, "world");
            }

            OnGameEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}