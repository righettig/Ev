using Ev.Common.Utils;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Entities.Core
{
    public interface IBlockingWorldEntity : IWorldEntity { }

    public abstract class BaseBlockingWorldEntity : IBlockingWorldEntity
    {
        public Color Color { get; }

        public BaseBlockingWorldEntity(Color color) => Color = color;

        //public IWorldEntity ToImmutable() 
        //{
        //    return this;
        //}
    }
}