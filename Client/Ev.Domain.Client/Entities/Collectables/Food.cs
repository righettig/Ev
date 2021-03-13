using Ev.Common.Utils;
using Ev.Domain.Client.Entities.Core;

namespace Ev.Domain.Client.Entities.Collectables
{
    public class Food : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Food(IRandom rnd) : base(rnd, MAX_VALUE)
        {
            Color = Color.Green;
        }
    }
}
