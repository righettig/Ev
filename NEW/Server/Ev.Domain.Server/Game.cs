using Ev.Common.Utils;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Predictors;
using Ev.Domain.Server.Processors;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure;
using Ev.Infrastructure.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Ev.Domain.Server
{
    public class Game : IGame
    {
        private readonly IPlatform _platform;

        public Game(string type, IWorld world)
        {
            // return LocalPlatform or RemotePlatform depending on type
            _platform = new LocalPlatform(this); // use a factory?
        }

        public IPlatform GetPlatform()
        {
            return _platform;
        }

        // onRegisterAgent
        public void RegisterAgent(string agentName, string agentColor)
        {
            // adds server-side tribe to the world
            // if #agents is OK -> start game loop
        }

        public void Start()
        {
        }

        //private void GameLoop()
        public async Task GameLoop(EvGameOptions options, IWorld world, IRandom rnd)
        {
            var finished = false;
            var iteration = 0;

            // TODO: for this to be useful I need to be able to record the game/world parameters too
            var history = new EvGameHistory();
            var actionProcessor = new GameActionProcessor(new AttackOutcomePredictor(rnd));

            _platform.OnGameStart();

            // TODO: delegate to Spectator?
            // Dump(world, iteration); 

            do
            {
                var alive = world.GetAliveTribes();

                for (var i = 0; i < alive.Length; i++)
                {
                    _platform.OnTurnStart();

                    var tribe = alive[i];
                    var state = world.GetWorldState(tribe);

                    var move = _platform.Update(state, tribe);

                    //if (move is PlayerControlledGameAction)
                    //{
                    //    DumpActions();
                    //    move = ReadAction(state);
                    //    move.Tribe = tribe;
                    //}

                    // TODO: it would be nice if we could delegate this responsibility to Platform, so that game logic already sees the final server action
                    // without having to perform this mapping logic. For now I'm gonna add TargetName as an additional property in the server action model.
                    if (move is AttackAction a)
                    { 
                        // mapping client-side action to server-side
                        a.Target = alive.First(el => el.Name == a.TargetName);
                    }

                    history.Add((move, state));

                    finished = world.Update(tribe, move, iteration, actionProcessor);

                    _platform.OnTurnEnd();

                    if (options.RenderEachTurn)
                    {
                        //var next = alive[(i + 1) % alive.Length];
                        //Dump(world, iteration, move, next);
                    }

                    if (options.WaitAfterEachMove)
                    {
                        System.Console.ReadLine();
                    }
                }

                iteration++;

            } while (!finished);

            _platform.OnGameEnd();

            // TODO: delegate to Spectator?
            // Dump(world, iteration);

            //if (options.DumpWinnerHistory)
            //{
            //    var winnerHistory = history.States.Where(el => el.Item1.Tribe.Name == world.Winner.Name);

            //    DumpHistory(winnerHistory.Select(el => el.Item1).ToList());

            //    var serializer = new EvGameHistorySerializer();
            //    await serializer.SaveToFile(winnerHistory, options.WinnerHistoryFilename);
            //}
        }
    }
}
