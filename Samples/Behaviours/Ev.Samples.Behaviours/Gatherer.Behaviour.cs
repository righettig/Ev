using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class GathererTribeBehaviour : TribeBehaviour
    {
        public GathererTribeBehaviour(IRandom rnd) : base(rnd) { }
        
        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var food = FindHighestValueFood();

            if (NotFound(food))
            {
                return RandomWalk();
            }

            return MoveTowards(food);
        }
    }
}