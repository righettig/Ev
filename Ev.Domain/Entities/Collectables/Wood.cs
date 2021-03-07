using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.Entities.Collectables
{
    public class Wood : CollectableWorldEntity
    {
        private const int MAX_VALUE = 9;

        public Wood(IRandom rnd) : base(rnd, MAX_VALUE)
        {
        }

        public Wood(int value)
        {
            Value = value;
        }
    }
}
