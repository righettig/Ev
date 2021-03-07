using Ev.Domain.Behaviours.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using Ev.Game;
using Ev.Samples.Behaviours;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ev.Tests.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        private readonly IRandom _rnd = new Random(1);

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

        static void CreateTribes(IWorld world)
        {
            world
                .AddTribe("RandomW",  Color.DarkYellow)
                .AddTribe("Gatherer", Color.Cyan)
                .AddTribe("Aggr",     Color.Yellow)
                .AddTribe("SmrtAggr", Color.Magenta);
        }

        [TestMethod]
        public async Task FourTribesOnRandomMap()
        {
            // Arrange
            var world = CreateWorld(new Random(1));

            CreateTribes(world);

            var tribeBehaviourRnd = new Random(1);

            var behaviours = new Dictionary<string, ITribeBehaviour>
            {
                { "RandomW",  new RandomWalkerTribeBehaviour    (tribeBehaviourRnd) },
                { "Gatherer", new JackOfAllTradesTribeBehaviour (tribeBehaviourRnd) },
                { "Aggr",     new AggressiveTribeBehaviour      (tribeBehaviourRnd) },
                { "SmrtAggr", new SmartAggressiveTribeBehaviour (tribeBehaviourRnd) }
            };

            var options = new GameOptions { DumpWinnerHistory = true, WinnerHistoryFilename = "actual_FourTribesOnRandomMap.json" };

            // Act
            await new EvGame(behaviours, options, world, new Random(1)).GameLoop();

            // Assertions
            AssertSameFinalState("FourTribesOnRandomMap");
        }

        [TestMethod]
        public async Task FourTribesOnStaticMap()
        {
            // Arrange
            var world = CreateWorldFromMap(new Random(1));

            CreateTribes(world);

            var behaviours = new Dictionary<string, ITribeBehaviour>
            {
                { "RandomW",  new RandomWalkerTribeBehaviour   (_rnd) },
                { "Gatherer", new JackOfAllTradesTribeBehaviour(_rnd) },
                { "Aggr",     new AggressiveTribeBehaviour     (_rnd) },
                { "SmrtAggr", new SmartAggressiveTribeBehaviour(_rnd) }
            };

            var options = new GameOptions { DumpWinnerHistory = true, WinnerHistoryFilename = "actual_FourTribesOnStaticMap.json" };

            // Act
            await new EvGame(behaviours, options, world, _rnd).GameLoop();

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
