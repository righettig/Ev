using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Core
{
    public class Wall : IBlockingWorldEntity
    {
        public Color Color => Color.White;
    }
}