using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.World;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure;
using Random = Ev.Common.Core.Random;

namespace Ev.Game.Server.Console
{
    class Program
    {
        static IWorld CreateWorld(IRandom rnd) => new RandomWorld(
            size: 24,
            new WorldResources { FoodCount = 100, WoodCount = 40, IronCount = 10 },
            rnd);

        static void Main()
        {
            var world = CreateWorld(new Random(1));

            var options = new EvGameOptions(players: 4)
            {
                RenderEachTurn = true,
                // WaitAfterEachMove = true,
                DumpWinnerHistory = true,
                Random = new Random(1)
            };

            var game = GameFactory.Server(options, world, 30051);
            
            //System.Console.WriteLine("Press any key to exit..." + Environment.NewLine);
            //System.Console.ReadKey();

            //game.Shutdown();
        }
    }
}