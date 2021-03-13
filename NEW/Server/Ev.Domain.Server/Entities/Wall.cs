using Ev.Domain.Server.Entities.Core;

namespace Ev.Domain.Server.Entities
{
    public class Wall : BaseBlockingWorldEntity
    {
        public Wall() : base(Common.Utils.Color.White)
        {
        }
    }
}