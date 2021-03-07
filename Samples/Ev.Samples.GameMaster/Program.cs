using Ev.Agents.GameMaster;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using Ev.Game;
using System;
using Random = Ev.Domain.Utils.Random;

namespace Ev.Samples.GameMaster
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
            size: 24,
            new WorldResources { FoodCount = 100, WoodCount = 40, IronCount = 10 },
            rnd);

        static void Main()
        {
            var world = CreateWorld(new Random(1));
            //var world = CreateWorldFromMap(new Random(1));

            var options = new GameOptions
            {
                //RenderEachTurn = true,
                //WaitAfterEachMove = true,
                DumpWinnerHistory = false,
            };

            var master = new GameMasterAgent(options, world, new Random(1), 2).Start();

            Console.WriteLine("Press any key to exit..." + Environment.NewLine);
            Console.ReadKey();

            master.Shutdown();
        }
    }
}
