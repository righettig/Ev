using Ev.Common.Utils;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Entities.Core
{
    public abstract class BaseBlockingWorldEntity : IBlockingWorldEntity
    {
        public Color Color { get; }

        public BaseBlockingWorldEntity(Color color) => Color = color;
    }
}