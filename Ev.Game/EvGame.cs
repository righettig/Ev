using Ev.Domain.Actions.Core;
using Ev.Domain.Actions.Core.Processors;
using Ev.Domain.Actions.Processors;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Serialization;
using System.Linq;
using System.Threading.Tasks;
using static Ev.Helpers.Debug;

namespace Ev.Game
{
    // TODO: Dump, etc. => Renderer?
    public static class EvGame
    {
        public static async Task GameLoop(EvGameOptions options, IWorld world, IRandom rnd)
        {
            var finished = false;
            var iteration = 0;

            // TODO: for this to be useful I need to be able to record the game/world parameters too
            var history = new EvGameHistory();
            var actionProcessor = new GameActionProcessor(new AttackOutcomePredictor(rnd));

            Dump(world, iteration);

            do
            {
                var alive = world.GetAliveTribes();

                for (int i = 0; i < alive.Length; i++)
                {
                    var tribe = alive[i];
                    var next = alive[(i + 1) % alive.Length];

                    var state = world.GetWorldState(tribe);
                    var move = tribe.DoMove(state);

                    if (move is PlayerControlledGameAction)
                    {
                        DumpActions();
                        move = ReadAction(state);
                        move.Tribe = tribe;
                    }
                    
                    if (move is Domain.Actions.AttackAction a) { // mapping client-side action to server-side
                        a.Target = alive.First(el => el.Name == a.TargetName);
                    }

                    history.Add((move, state));

                    finished = world.Update(tribe, move, iteration, actionProcessor);

                    if (options.RenderEachTurn) {
                        Dump(world, iteration, move, next);
                    }

                    if (options.WaitAfterEachMove) {
                        System.Console.ReadLine();
                    }
                }

                iteration++;

            } while (!finished);

            Dump(world, iteration++);

            if (options.DumpWinnerHistory) 
            {
                var winnerHistory = history.States.Where(el => el.Item1.Tribe.Name == world.Winner.Name);

                DumpHistory(winnerHistory.Select(el => el.Item1).ToList());

                var serializer = new EvGameHistorySerializer();
                await serializer.SaveToFile(winnerHistory, options.WinnerHistoryFilename);
            }
        }
    }
}
