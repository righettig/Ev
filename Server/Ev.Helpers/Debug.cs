using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static System.Math;

namespace Ev.Helpers
{
    public static class Debug
    {
        private static readonly int WORLD_STATE_SIZE = WorldState.WORLD_STATE_SIZE;

        public static void DumpHistory(IList<IGameAction> history)
        {
            for (var i = 0; i < history.Count; i++)
            {
                WriteLine($"{i} - {history[i]}");
            }
        }

        public static void DumpActions() 
        {
            WriteLine("Choose: (H)old (A)ttack (M)ove (S)uicide (U)pgrade Attack Upgrade (D)efenses");
        }

        public static Domain.Client.Core.IGameAction ReadAction(IWorldState state) 
        {
            var key = ReadKey(true);

            Domain.Client.Core.IGameAction action;
            do
            {
                switch (key.Key)
                {
                    case ConsoleKey.S: action = new Domain.Client.Actions.SuicideAction(); break;

                    case ConsoleKey.H: action = new Domain.Client.Actions.HoldAction(); break;

                    case ConsoleKey.U: action = new Domain.Client.Actions.UpgradeAttackAction(); break;

                    case ConsoleKey.D: action = new Domain.Client.Actions.UpgradeDefensesAction(); break;

                    case ConsoleKey.M:
                        DumpDirections();
                        action = new Domain.Client.Actions.MoveAction(ReadDirection()); break;

                    case ConsoleKey.A:
                        DumpDirections();
                        var loc = (-1, -1);
                        switch (ReadDirection())
                        {
                            case Direction.N:  loc = (2, 1); break;
                            case Direction.S:  loc = (2, 3); break;
                            case Direction.E:  loc = (3, 2); break;
                            case Direction.W:  loc = (1, 2); break;
                            case Direction.NE: loc = (3, 1); break;
                            case Direction.NW: loc = (1, 1); break;
                            case Direction.SE: loc = (3, 3); break;
                            case Direction.SW: loc = (1, 3); break;
                        }
                        var target = state.GetEntity<ITribe>(loc);
                        if (target == null) {
                            WriteLine("Invalid Target!" + Environment.NewLine);
                            DumpActions();
                            return ReadAction(state);
                        }
                        return new Domain.Client.Actions.AttackAction(target.Name);

                    default: action = null; break;
                }
            } while (action == null);
            
            return action;
        }

        private static void DumpDirections()
        {
            WriteLine("Choose: 0 -> N | 1 -> S | 2 -> E | 3 -> W | 4 -> NE | 5 -> NW | 6 -> SE | 7 -> SW");
        }

        private static Direction ReadDirection()
        {
            var key = ReadKey(true);

            Direction? dir;
            do
            {
                dir = key.Key switch
                {
                    ConsoleKey.D0 => Direction.N,
                    ConsoleKey.D1 => Direction.S,
                    ConsoleKey.D2 => Direction.E,
                    ConsoleKey.D3 => Direction.W,
                    ConsoleKey.D4 => Direction.NE,
                    ConsoleKey.D5 => Direction.NW,
                    ConsoleKey.D6 => Direction.SE,
                    ConsoleKey.D7 => Direction.SW,
                    _ => null,
                };
            } while (!dir.HasValue);

            return dir.Value;
        }

        public static void Dump(IWorld world, int iteration, IGameAction move = null, ITribe next = null) 
        {
            WriteLine($"iteration {iteration} {Environment.NewLine}");

            if (move != null) {
                Write("Current: ");
                ForegroundColor = ColorMapper.MapColor(move.Tribe.Color);
                Write($"{move.Tribe.Name}");
                ResetColor();
                WriteLine($" - {move} {(move is BlockingGameAction {Completed: true} a ? $"COMPLETED ({a.Result()})" : "")}");
                if (next != null) WriteLine($"Next:\t {next.Name}");
                WriteLine();
            }

            if (world.Finished)
            {
                AsWinner(world.Winner);

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

            var rowLength = world.Size;
            var colLength = world.Size;

            for (var i = 0; i < rowLength; i++)
            {
                for (var j = 0; j < colLength; j++)
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
                            if (Abs(next.Position.x - j) <= WORLD_STATE_SIZE && Abs(next.Position.y - i) <= WORLD_STATE_SIZE)
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

        public static void DumpWorldState(IWorldState worldState)
        {
            var rowLength = worldState.State.GetLength(0);
            var colLength = worldState.State.GetLength(1);

            for (var i = 0; i < rowLength; i++)
            {
                for (var j = 0; j < colLength; j++)
                {
                    if (worldState.State[j, i] != null)
                    {
                        ConsoleRenderer.Render(worldState.State[j, i]);
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkGray;
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

        public static void AsAlive(ITribe tribe, bool current)
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

            Write($":\t{tribe.Population} {delta}\t[Iron: {tribe.Iron} Wood: {tribe.Wood}] ");

            //Write(tribe.DebugBehaviour());

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
