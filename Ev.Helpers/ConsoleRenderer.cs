using Ev.Domain.Entities.Blocking;
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

                case IBlockingWorldEntity {Type: BlockingWorldEntityType.Wall}:
                    ForegroundColor = System.ConsoleColor.White;
                    Write("O ");
                    break;

                case IBlockingWorldEntity { Type: BlockingWorldEntityType.Water }:
                    ForegroundColor = System.ConsoleColor.Blue;
                    Write("~ ");
                    break;

                case IBlockingWorldEntity { Type: BlockingWorldEntityType.NotReachable }:
                    ForegroundColor = System.ConsoleColor.White;
                    Write("* ");
                    break;

                case ICollectableWorldEntity { Type: CollectableWorldEntityType.Food } e:
                    ForegroundColor = System.ConsoleColor.Green;
                    Write($"{e.Value} ");
                    break;

                case ICollectableWorldEntity { Type: CollectableWorldEntityType.Wood } e:
                    ForegroundColor = System.ConsoleColor.DarkRed;
                    Write($"{e.Value} ");
                    break;

                case ICollectableWorldEntity { Type: CollectableWorldEntityType.Iron } e:
                    ForegroundColor = System.ConsoleColor.DarkYellow;
                    Write($"{e.Value} ");
                    break;
            }

            ResetColor();
        }
    }
}
