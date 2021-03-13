using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Game;
using Ev.Samples.Behaviours;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Ev.Tests.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        static IWorld CreateWorldFromMap(IRandom rnd)
        {
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
            size: 24,
            new WorldResources { FoodCount = 100, WoodCount = 40, IronCount = 10 },
            rnd);

        static void CreateTribes(IWorld world, IRandom rnd) =>
            world
                .WithTribe("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd))
                .WithTribe("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd))
                .WithTribe("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd))
                .WithTribe("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd));

        [TestMethod]
        public async Task FourTribesOnRandomMap()
        {
            // Arrange
            var world = CreateWorld(new Random(1));

            CreateTribes(world, new Random(1));

            var options = new EvGameOptions { DumpWinnerHistory = true, WinnerHistoryFilename = "actual_FourTribesOnRandomMap.json" };

            // Act
            await EvGame.GameLoop(options, world, new Random(1));

            // Assertions
            AssertSameFinalState("FourTribesOnRandomMap");
        }

        [TestMethod]
        public async Task FourTribesOnStaticMap()
        {
            // Arrange
            var world = CreateWorldFromMap(new Random(1));

            CreateTribes(world, new Random(1));

            var options = new EvGameOptions { DumpWinnerHistory = true, WinnerHistoryFilename = "actual_FourTribesOnStaticMap.json" };

            // Act
            await EvGame.GameLoop(options, world, new Random(1));

            // Assertions
            AssertSameFinalState("FourTribesOnStaticMap");
        }

        private static void AssertSameFinalState(string testName) 
        {
            var expected = File.ReadAllText($"Expected/expected_{testName}.json");
            var actual   = File.ReadAllText($"actual_{testName}.json");

            Assert.AreEqual(expected, actual);
        }
    }
}
