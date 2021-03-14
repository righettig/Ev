using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.Entities.Core;

namespace Ev.Domain.Server.Entities.Collectables
{
    public class Iron : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Iron(IRandom rnd) : base(rnd, MAX_VALUE)
        {
        }
    }
}
