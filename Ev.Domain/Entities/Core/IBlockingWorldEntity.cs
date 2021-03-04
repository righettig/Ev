using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Core
{
    public interface IBlockingWorldEntity : IWorldEntity { }

    public abstract class BaseBlockingWorldEntity : IBlockingWorldEntity
    {
        public Color Color { get; }

        public BaseBlockingWorldEntity(Color color) => Color = color;

        public IWorldEntity ToImmutable() 
        {
            return this;
        }
    }
}