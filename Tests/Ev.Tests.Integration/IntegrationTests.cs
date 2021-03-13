using Ev.Common.Utils;
using Ev.Domain.Client;
using Ev.Domain.Server;
using Ev.Domain.Server.World;
using Ev.Domain.Server.World.Core;
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

        static void CreateTribes(Game game, IRandom rnd)
        {
            var platform = game.GetPlatform();

            var agent1 = new TribeAgent("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd));
            var agent2 = new TribeAgent("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd));
            var agent3 = new TribeAgent("Aggr",     Color.Cyan,       new AggressiveTribeBehaviour(rnd));
            var agent4 = new TribeAgent("Gatherer", Color.Cyan,       new SmartAggressiveTribeBehaviour(rnd));

            platform.RegisterAgent(agent1, agent2, agent3, agent4);
        }

        [TestMethod]
        public async Task FourTribesOnRandomMap()
        {
            // Arrange
            var world = CreateWorld(new Random(1));

            var game = new Game("local", world, new Random(1));

            CreateTribes(game, new Random(1));

            // Act
            await game.GameLoop(new EvGameOptions
            {
                DumpWinnerHistory = true,
                WinnerHistoryFilename = "actual_FourTribesOnRandomMap.json"
            });

            // Assertions
            AssertSameFinalState("FourTribesOnRandomMap");
        }

        [TestMethod]
        public async Task FourTribesOnStaticMap()
        {
            // Arrange
            var world = CreateWorldFromMap(new Random(1));

            var game = new Game("local", world, new Random(1));

            CreateTribes(game, new Random(1));

            // Act
            await game.GameLoop(new EvGameOptions
            {
                DumpWinnerHistory = true,
                WinnerHistoryFilename = "actual_FourTribesOnStaticMap.json"
            });

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
