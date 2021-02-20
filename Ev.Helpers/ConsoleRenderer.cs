using Ev.Domain.Entities.Core;
using static System.Console;

namespace Ev.Helpers
{
    static class ConsoleRenderer
    {
        public static void Render(IWorldEntity entity) 
        {
            ForegroundColor = ColorMapper.MapColor(entity.Color);

            switch (entity) 
            {
                case ITribe:
                    Write("X ");
                    break;

                case Wall:
                    Write("O ");
                    break;

                case Water:
                    Write("~ ");
                    break;

                case ICollectableWorldEntity e:
                    Write($"{e.Value} ");
                    break;
            }

            ResetColor();
        }
    }
}
