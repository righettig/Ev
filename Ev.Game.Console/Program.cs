using Ev.Behaviours;
using Ev.Domain.Utils;
using Ev.Domain.World;

namespace Ev.Game
{
    class Program
    {
        static IWorld CreateWorldFromMap(IRandom rnd) 
        {
            //var map = // 2 tribes
            //    @"
            //        S _ _ _ _ _
            //        _ _ X X I _
            //        _ _ F X _ _
            //        _ _ _ X _ _
            //        _ _ _ _ X _
            //        W _ _ _ _ S
            //    ";

            var map = // 4 tribes
                @"
                    S _ _ _ _ _ _ _ F _ _ _ _ _ _ S
                    _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _
                    _ _ _ _ X X X X X _ _ _ _ _ _ _
                    _ _ _ _ X I F _ W X _ _ _ I _ _
                    _ W _ _ X _ W _ _ I X _ _ _ _ _
                    _ W _ _ X F _ _ F _ X _ _ _ _ _
                    _ _ _ _ ~ X _ F _ _ X _ _ _ _ _
                    _ _ _ ~ ~ ~ X _ _ F _ X _ _ _ _
                    _ _ _ ~ ~ ~ _ _ _ _ _ X _ F _ _
                    _ F _ ~ X _ F _ _ X X _ _ _ _ _
                    _ _ _ ~ X _ _ _ X _ _ _ _ _ _ _
                    _ _ _ ~ ~ _ _ _ _ _ _ W _ _ _ _
                    _ W _ _ ~ _ _ _ _ _ ~ ~ ~ _ _ _
                    _ _ _ _ _ _ F _ _ F ~ ~ ~ _ _ _
                    _ _ _ _ _ _ _ _ _ _ ~ ~ ~ _ _ _
                    S _ _ _ _ _ _ _ _ _ _ _ _ _ _ S
                ";

            return new MapWorld(16, map, rnd);
        }

        static IWorld CreateWorld(IRandom rnd) => new RandomWorld(
            size: 6,
            new WorldResources { FoodCount = 1, WoodCount = 1, IronCount = 1 },
            rnd);

        static void CreateTribes(IWorld world, IRandom rnd) =>
            world
                //.WithTribe("Player1",  Color.White,      new PlayerControlledTribeBehaviour(rnd))
                .WithTribe("Engineer", Color.DarkYellow, new EngineerTribeBehaviour(rnd))
                //.WithTribe("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd))
                //.WithTribe("Explorer", Color.Red,        new ExplorerTribeBehaviour(rnd))
                .WithTribe("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd))
                //.WithTribe("Lazy",     Color.White,      new LazyTribeBehaviour(rnd))
                //.WithTribe("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd))
                .WithTribe("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd))
                .WithTribe("Flee",     Color.DarkCyan,   new FleeTribeBehaviour(rnd));

        // TODO: implement e2e test based on history and initial conditions
        static void Main()
        {
            // TODO: it should be possible to specify parameters like how much population you lose by standing still, win gain etc.

            //var world = CreateWorld(new Random(1));
            var world = CreateWorldFromMap(new Random(1234));

            CreateTribes(world, new Random(1));

            var options = new EvGameOptions
            {
                //RenderEachTurn = true,
                //WaitAfterEachMove = true,
                DumpWinnerHistory = true,
            };

            EvGame.GameLoop(options, world, new Random(1));
        }
    }
}
