using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Core
{
    public class Wall : IBlockingEntity
    {
        public Color Color => Color.White;
    }
}