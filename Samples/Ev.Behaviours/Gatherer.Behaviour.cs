using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Samples.Behaviours
{
    public class GathererTribeBehaviour : TribeBehaviour
    {
        public GathererTribeBehaviour(IRandom rnd) : base(rnd) { }
        
        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            var food = FindHighestValueFood();

            if (NotFound(food))
            {
                return RandomWalk();
            }
            else 
            {
                return MoveTowards(food);
            }            
        }
    }
}