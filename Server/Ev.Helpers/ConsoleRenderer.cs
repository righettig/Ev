using Ev.Common.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Collectables;
using static System.Console;

namespace Ev.Helpers
{
    static class ConsoleRenderer
    {
        public static void Render(IWorldEntity entity) 
        {
            switch (entity) 
            {
                case ITribe t:
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
