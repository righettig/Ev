using Ev.Common.Utils;
using Ev.Domain.Client;
using Ev.Domain.Server;
using Ev.Domain.Server.World;
using Ev.Domain.Server.World.Core;
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

        static async Task Main(string[] args)
        {
            var world = CreateWorld(new Random(1));

            var game = new Domain.Server.Game("local", world, new Random(1));
            game.Start(); // server starts waiting for join requests

            var platform = game.GetPlatform();

            var rnd = new Random(1);

            var agent1 = new TribeAgent("t1", Color.Magenta, new LazyTribeBehaviour(rnd));
            var agent2 = new TribeAgent("t2", Color.Yellow,  new RandomWalkerTribeBehaviour(rnd));

            // TODO: create overload params ITribeAgent[]
            platform.RegisterAgent(agent1);
            platform.RegisterAgent(agent2);

            var options = new EvGameOptions
            {
                //RenderEachTurn = true,
                //WaitAfterEachMove = true,
                DumpWinnerHistory = true,
            };

            await game.GameLoop(options);
        }
    }
}
