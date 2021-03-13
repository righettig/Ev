using Ev.Domain.Server.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Core;
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
                    //ForegroundColor = ColorMapper.MapColor(entity.Color);
                    Write("O ");
                    break;

                case Water:
                    Write("~ ");
                    //ForegroundColor = ColorMapper.MapColor(entity.Color);
                    break;

                case ICollectableWorldEntity e:
                    //ForegroundColor = ColorMapper.MapColor(e.Color);
                    Write($"{e.Value} ");
                    break;
            }

            ResetColor();
        }
    }
}
