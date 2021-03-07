using Ev.Domain.Entities;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using static System.Console;

namespace Ev.Helpers
{
    static class ConsoleRenderer
    {
        public static void Render(IWorldEntity entity) 
        {
            switch (entity) 
            {
                case ITribeState t:
                    ForegroundColor = ColorMapper.MapColor(t.Color);
                    Write("X ");
                    break;

                case Wall:
                    ForegroundColor = System.ConsoleColor.White;
                    Write("O ");
                    break;

                case Water:
                    ForegroundColor = System.ConsoleColor.Blue;
                    Write("~ ");
                    break;

                case NotReachable:
                    ForegroundColor = System.ConsoleColor.White;
                    Write("* ");
                    break;

                case Food e:
                    ForegroundColor = System.ConsoleColor.Green;
                    Write($"{e.Value} ");
                    break;

                case Wood e:
                    ForegroundColor = System.ConsoleColor.DarkRed;
                    Write($"{e.Value} ");
                    break;

                case Iron e:
                    ForegroundColor = System.ConsoleColor.DarkYellow;
                    Write($"{e.Value} ");
                    break;
            }

            ResetColor();
        }
    }
}
