using Ev.Domain.Actions.Core;
using Ev.Domain.Actions.Core.Processors;
using Ev.Domain.Actions.Processors;
using Ev.Domain.Utils;
using Ev.Domain.World;
using Ev.Domain.World.Core;
using System.Collections.Generic;
using System.Linq;
using static Ev.Helpers.Debug;

namespace Ev.Game
{
    // TODO: Dump, etc. => Renderer?

    public static class EvGame
    {
        public static void GameLoop(IWorld world, IRandom rnd)
        {
            var finished = false;
            var iteration = 0;

            var history = new List<(IGameAction, IWorldState)>();
            var actionProcessor = new GameActionProcessor(new AttackOutcomePredictor(rnd));

            Dump(world, iteration);

            do
            {
                var alive = world.GetAliveTribes();

                for (int i = 0; i < alive.Length; i++)
                {
                    var tribe = alive[i];
                    var next = alive[(i + 1) % alive.Length];

                    var state = world.GetWorldState(tribe);
                    var move = tribe.DoMove(state);

                    if (move is PlayerControlledGameAction)
                    {
                        DumpActions();
                        move = ReadAction(state);
                        move.Tribe = tribe;
                    }

                    history.Add((move, state));

                    finished = world.Update(tribe, move, iteration, actionProcessor);

                    // TODO: pass as config option
                    //Dump(world, iteration, move, next);

                    // TODO: pass as config option
                    //ReadLine();
                }

                iteration++;

            } while (!finished);

            Dump(world, iteration++);

            // TODO: pass as config option
            DumpHistory(
                history.Where(el => el.Item1.Tribe == world.Winner).Select(el => el.Item1).ToList());
        }
    }
}
