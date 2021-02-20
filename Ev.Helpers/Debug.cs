using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.Math;

namespace Ev.Helpers
{
    public static class Debug 
    {
        public static void DumpHistory(IList<IGameAction> history) 
        {
            for (int i = 0; i < history.Count; i++)
            {
                WriteLine($"{i} - {history[i]}");
            }
        }

        public static void Dump(IWorld world, int iteration, IGameAction move = null, ITribe next = null) 
        {
            WriteLine($"iteration {iteration} {Environment.NewLine}");

            if (move != null) {
                Write($"Current: ");
                ForegroundColor = ColorMapper.MapColor(move.Tribe.Color);
                Write($"{move.Tribe.Name}");
                ResetColor();
                WriteLine($" - {move}");
                WriteLine($"Next:\t {next.Name}");
                WriteLine();
            }

            if (world.Finished)
            {
                var winner = world.Tribes.First(t => t.Population > 0);

                AsWinner(winner);

                foreach (var t in world.Tribes
                    .Where(t => t.Population <= 0)
                    .OrderByDescending(t => t.DeadAtIteration))
                {
                    AsDead(t);
                }
            }
            else 
            {
                foreach (var t in world.Tribes.OrderByDescending(t => t.Population))
                {
                    if (t.Population <= 0)
                    {
                        AsDead(t);
                    }
                    else
                    {
                        AsAlive(t, t == move?.Tribe);
                    }
                }
            }

            WriteLine();

            int rowLength = world.Size;
            int colLength = world.Size;

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    if (world.State[j, i] != null)
                    {
                        ConsoleRenderer.Render(world.State[j, i]);
                    }
                    else 
                    {
                        ForegroundColor = ConsoleColor.DarkGray;
                        if (next != null)
                        {
                            if (Abs(next.Position.x - j) <= WorldState.WORLD_STATE_SIZE && Abs(next.Position.y - i) <= WorldState.WORLD_STATE_SIZE)
                            {
                                ForegroundColor = ConsoleColor.White;
                            }
                        }
                        Write("0 ");
                        ResetColor();
                    }
                }

                Write(Environment.NewLine);
            }

            WriteLine();
        }

        private static void AsWinner(ITribe tribe) 
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine($"Tribe {tribe.Name}: {tribe.Population}");
        }

        private static void AsDead(ITribe tribe) 
        {
            ForegroundColor = ConsoleColor.DarkGray;
            WriteLine($"Tribe {tribe.Name}:\tDEAD {tribe.DeadAtIteration}");
            ResetColor();
        }

        private static void AsAlive(ITribe tribe, bool current)
        {
            Write("Tribe ");

            ForegroundColor = ColorMapper.MapColor(tribe.Color);
            Write(tribe.Name);
            ResetColor();

            var delta = "";
            if (current) 
            {
                var sign = tribe.Population > tribe.PrevPopulation ? "+" : "-";
                
                delta = $"({sign}{Abs(tribe.PrevPopulation - tribe.Population)})";
            }

            Write($":\t{tribe.Population} {delta}");

            if (tribe.IsAttacking)
            {
                WriteLine("*");
            }
            else 
            {
                WriteLine();
            }
        }
    }
}
