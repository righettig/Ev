using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client;
using Ev.Domain.Server.World;
using Ev.Domain.Server.World.Core;
using Ev.Game.Server;
using Ev.Infrastructure;
using Ev.Samples.Behaviours;
using System.Threading.Tasks;

namespace Ev.Game.Console
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

            var platform = new LocalPlatform();

            var game = new Server.Game(platform, world, new Random(1));
            //game.Start(); // server starts waiting for join requests

            var rnd = new Random(1);

            var agent1 = new TribeAgent("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd));
            var agent2 = new TribeAgent("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd));
            var agent3 = new TribeAgent("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd));
            var agent4 = new TribeAgent("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd));

            platform.RegisterAgent(game, agent1, agent2, agent3, agent4);

            await game.GameLoop(new EvGameOptions
            {
                RenderEachTurn = true,
                WaitAfterEachMove = true,
                DumpWinnerHistory = true,
            });
        }
    }
}
