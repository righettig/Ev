using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
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
