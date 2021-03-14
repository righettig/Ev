using Ev.Common.Core;
using Ev.Domain.Server.Entities.Core;

namespace Ev.Domain.Server.Entities.Collectables
{
    public class Food : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Food(IRandom rnd) : base(rnd, MAX_VALUE)
        {
        }
    }
}
