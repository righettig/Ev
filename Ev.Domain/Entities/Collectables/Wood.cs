using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
{
    class Wood : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Wood(IRandom rnd) : base(rnd, MAX_VALUE)
        {
            Color = Color.DarkRed;
        }
    }
}
