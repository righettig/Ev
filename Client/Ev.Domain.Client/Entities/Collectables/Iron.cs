using Ev.Common.Utils;
using Ev.Domain.Client.Entities.Core;

namespace Ev.Domain.Client.Entities.Collectables
{
    public class Iron : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Iron(IRandom rnd) : base(rnd, MAX_VALUE)
        {
            Color = Color.DarkYellow;
        }
    }
}
