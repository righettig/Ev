using Ev.Common.Core;
using Ev.Domain.Client;
using Ev.Infrastructure;
using Ev.Samples.Behaviours;
using Random = Ev.Common.Core.Random;

namespace Ev.Game.Client.Console
{
    class Program
    {
        static void Main()
        {
            var game = GameFactory.Client("localhost", 30051, 30052);

            var rnd = new Random(1);

            var agent1 = new TribeAgent("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd));
            var agent2 = new TribeAgent("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd));
            var agent3 = new TribeAgent("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd));
            var agent4 = new TribeAgent("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd));

            game.RegisterAgent(agent1, agent2, agent3, agent4);

            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            //tribe2.Shutdown();
        }
    }
}