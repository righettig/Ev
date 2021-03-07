using Ev.Domain.Behaviours.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using Ev.Game;
using Ev.Helpers;
using Ev.Samples.Behaviours;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ev.Samples.SameProcess
{
    class Program
    {
        static IWorld CreateWorld(IRandom rnd) => new RandomWorld(
            size: 24,
            new WorldResources { FoodCount = 100, WoodCount = 40, IronCount = 10 },
            rnd);

        static async Task Main()
        {
            var world = CreateWorld(new Random(1));

            world
                //.AddTribe("Player1",  Color.White)
                //.AddTribe("Engineer", Color.DarkYellow)
                //.WithTribe("Explorer", Color.Red)
                //.WithTribe("Lazy",     Color.White)
                //.WithTribe("Flee",     Color.DarkCyan)
                .AddTribe("RandomW",  Color.DarkYellow)
                .AddTribe("Gatherer", Color.Cyan)
                .AddTribe("Aggr",     Color.Yellow)
                .AddTribe("SmrtAggr", Color.Magenta);

            var rnd = new Random(1);

            var behaviours = new Dictionary<string, ITribeBehaviour>
            {
                { "RandomW",  new RandomWalkerTribeBehaviour   (rnd) },
                { "Gatherer", new JackOfAllTradesTribeBehaviour(rnd) },
                { "Aggr",     new AggressiveTribeBehaviour     (rnd) },
                { "SmrtAggr", new SmartAggressiveTribeBehaviour(rnd) }
                //"Player1",  new PlayerControlledTribeBehaviour(rnd)
                //"Engineer", new EngineerTribeBehaviour(rnd)
                //"Lazy",     new LazyTribeBehaviour(rnd)
                //"Explorer", new ExplorerTribeBehaviour(rnd)
                //"Flee",     new FleeTribeBehaviour(rnd)
            };

            var options = new GameOptions
            {
                //RenderEachTurn = true,
                //WaitAfterEachMove = true,
                DumpWinnerHistory = true,
            };

            var game = new EvGame(behaviours, options, world, new Random(1));

            game.OnGameStart += (_, _) => Debug.Dump(world, 0);

            await game.GameLoop();
        }
    }
}