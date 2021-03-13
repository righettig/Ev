using Ev.Common.Utils;
using Ev.Domain.Client.Entities.Core;

namespace Ev.Domain.Client.Entities.Collectables
{
    public class Wood : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Wood(IRandom rnd) : base(rnd, MAX_VALUE)
        {
            Color = Color.DarkRed;
        }
    }
}
