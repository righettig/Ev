using Ev.Domain.Server.Entities.Core;

namespace Ev.Domain.Server.Entities
{
    public class Water : BaseBlockingWorldEntity
    {
        public Water() : base(Common.Utils.Color.Blue)
        {
        }
    }
}