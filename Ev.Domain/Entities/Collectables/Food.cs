using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
{
    class Food : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Food(IRandom rnd) : base(rnd, MAX_VALUE)
        {
            Color = Color.Green;
        }
    }
}
