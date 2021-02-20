using Ev.Behaviours;
using Ev.Domain.Utils;
using Ev.Domain.World;

namespace Ev.Game
{
    class Program
    {
        static IWorld CreateWorld(IRandom rnd) => new World(
            size: 32,
            new WorldResources { FoodCount = 100, WoodCount = 30, IronCount = 100 },
            rnd);

        static void CreateTribes(IWorld world, IRandom rnd) =>
            world
                //.WithTribe("Engineer", Color.DarkYellow, new EngineerTribeBehaviour(rnd))
                .WithTribe("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd))
                .WithTribe("Explorer", Color.Red,        new ExplorerTribeBehaviour(rnd))
                .WithTribe("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd))
                .WithTribe("Lazy",     Color.White,      new LazyTribeBehaviour(rnd))
                .WithTribe("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd))
                .WithTribe("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd))
                .WithTribe("Flee",     Color.DarkCyan,   new FleeTribeBehaviour(rnd));

        // TODO: implement e2e test based on history and initial conditions
        static void Main()
        {
            var world = CreateWorld(new Random(1));

            CreateTribes(world, new Random(1));

            EvGame.GameLoop(world, new Random(1));
        }
    }
}
