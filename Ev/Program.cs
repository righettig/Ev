using Ev.Behaviours;
using Ev.Domain.Actions.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using System.Collections.Generic;
using System.Linq;
using static Ev.Helpers.Debug;

namespace Ev.Game
{
    class Program
    {
        static IWorld CreateWorld() => new World(size: 16, food: 0, wood: 0, iron: 0, new Random(1234));

        static void CreateTribes(IWorld world, IRandom rnd) => 
            world
                .WithTribe("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd))
                .WithTribe("Explorer", Color.Red,        new ExplorerTribeBehaviour(rnd))
                .WithTribe("Gatherer", Color.Cyan,       new JaclkOfAllTradesTribeBehaviour(rnd))
                .WithTribe("Lazy",     Color.White,      new LazyTribeBehaviour(rnd))
                .WithTribe("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd))
                .WithTribe("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd))
                .WithTribe("Flee",     Color.DarkCyan,   new FleeTribeBehaviour(rnd));

        static void GameLoop(IWorld world) 
        {
            var finished = false;
            var iteration = 0;

            var history = new List<IGameAction>();

            Dump(world, iteration);

            do
            {
                var alive = world.GetAliveTribes().ToArray();
                
                for (int i = 0; i < alive.Length; i++)
                {
                    var tribe = alive[i];
                    var next = alive[(i + 1) % alive.Length];

                    var move = tribe.DoMove(world.GetWorldState(tribe));
                    history.Add(move);

                    finished = world.Update(tribe, move, iteration);

                    //Dump(world, iteration, move, next);

                    world.GetAliveTribes().ToList().ForEach(t => t.IsAttacking = false);

                    //ReadLine();
                }

                iteration++;

            } while (!finished);

            Dump(world, iteration++);

            DumpHistory(history.Where(el => el.Tribe == world.Winner).ToList());
        }

        static void Main()
        {
            var world = CreateWorld();
            
            CreateTribes(world, new Random());
            GameLoop(world);
        }
    }
}
