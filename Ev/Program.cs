using Ev.Behaviours;
using Ev.Domain.Actions.Core;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using System.Collections.Generic;
using System.Linq;
using static Ev.Helpers.Debug;

namespace Ev.Game
{
    class Program
    {
        static IWorld CreateWorld() => new World(size: 32, food: 10, wood: 10, iron: 10, new Random(1234));

        static void CreateTribes(IWorld world, IRandom rnd) => 
            world
                .WithTribe("RandomW",  Color.DarkYellow, new RandomWalkerTribeBehaviour(rnd))
                .WithTribe("Explorer", Color.Red,        new ExplorerTribeBehaviour(rnd))
                .WithTribe("Gatherer", Color.Cyan,       new JackOfAllTradesTribeBehaviour(rnd))
                .WithTribe("Lazy",     Color.White,      new LazyTribeBehaviour(rnd))
                .WithTribe("Aggr",     Color.Yellow,     new AggressiveTribeBehaviour(rnd))
                .WithTribe("SmrtAggr", Color.Magenta,    new SmartAggressiveTribeBehaviour(rnd))
                .WithTribe("Flee",     Color.DarkCyan,   new FleeTribeBehaviour(rnd));

        static void GameLoop(IWorld world) 
        {
            var finished = false;
            var iteration = 0;

            var history = new List<(IGameAction, IWorldState)>();

            Dump(world, iteration);

            do
            {
                var alive = world.GetAliveTribes().ToArray();
                
                for (int i = 0; i < alive.Length; i++)
                {
                    var tribe = alive[i];
                    var next = alive[(i + 1) % alive.Length];

                    var state = world.GetWorldState(tribe);
                    var move = tribe.DoMove(state);
                    history.Add((move, state));

                    finished = world.Update(tribe, move, iteration);

                    //Dump(world, iteration, move, next);

                    world.GetAliveTribes().ToList().ForEach(t => t.IsAttacking = false);

                    //ReadLine();
                }

                iteration++;

            } while (!finished);

            Dump(world, iteration++);

            DumpHistory(
                history.Where(el => el.Item1.Tribe == world.Winner).Select(el => el.Item1).ToList());
        }

        static void Main()
        {
            var world = CreateWorld();

            // TODO: setup game by specifying how many tribes are requested
            // TODO: wait for tribes to join the game
            // TODO: upon tribe joining the game registers the tribe IP address, name and color 
            // when doing 
            //  tribe.DoMove(world.GetWorldState(tribe));
            // game sends the world state to each tribe which in turn replies by sending its move
            // DoMove channels the move into a wrapper that takes care sending the move to the server
            // the wrapper is also responsive to deserialise the worldstate coming from the server
            // server is also sending special term messages when winning or losing

            CreateTribes(world, new Random());

            GameLoop(world);
        }
    }
}
