using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World;
using Ev.Domain.World.Core;
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

        public static void DumpActions() 
        {
            WriteLine("Choose: (H)old (A)attack (M)ove (S)uicide");
        }

        public static IGameAction ReadAction(IWorldState state) 
        {
            var key = ReadKey(true);

            IGameAction action = null;
            do
            {
                switch (key.Key)
                {
                    case ConsoleKey.S: action = new SuicideGameAction(); break;

                    case ConsoleKey.H: action = new HoldAction(); break;

                    case ConsoleKey.M:
                        DumpDirections();
                        action = new MoveAction(ReadDirection()); break;

                    case ConsoleKey.A:
                        DumpDirections();
                        var loc = (-1, -1);
                        switch (ReadDirection())
                        {
                            case Directions.N: loc = (2, 1); break;
                            case Directions.S: loc = (2, 3); break;
                            case Directions.E: loc = (3, 2); break;
                            case Directions.W: loc = (1, 2); break;
                            case Directions.NE: loc = (3, 1); break;
                            case Directions.NW: loc = (1, 1); break;
                            case Directions.SE: loc = (3, 3); break;
                            case Directions.SW: loc = (1, 3); break;
                        }
                        var target = state.GetEntity<ITribe>(loc);
                        if (target == null) {
                            WriteLine("Invalid Target!" + Environment.NewLine);
                            DumpActions();
                            return ReadAction(state);
                        }
                        return new AttackAction(target);

                    default: action = null; break;
                }
            } while (action == null);
            
            return action;
        }

        private static void DumpDirections()
        {
            WriteLine("Choose: 0 -> N | 1 -> S | 2 -> E | 3 -> W | 4 -> NE | 5 -> NW | 6 -> SE | 7 -> SW");
        }

        private static Directions ReadDirection()
        {
            var key = ReadKey(true);

            Directions? dir;
            do
            {
                switch (key.Key)
                {
                    case ConsoleKey.D0: dir = Directions.N; break;
                    case ConsoleKey.D1: dir = Directions.S; break;
                    case ConsoleKey.D2: dir = Directions.E; break;
                    case ConsoleKey.D3: dir = Directions.W; break;
                    case ConsoleKey.D4: dir = Directions.NE; break;
                    case ConsoleKey.D5: dir = Directions.NW; break;
                    case ConsoleKey.D6: dir = Directions.SE; break;
                    case ConsoleKey.D7: dir = Directions.SW; break;
                    default: dir = null; break;
                }

            } while (!dir.HasValue);

            return dir.Value;
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
                            if (Abs(next.Position.x - j) <= 2 && Abs(next.Position.y - i) <= 2)
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
