using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class FleeTribeBehaviour : TribeBehaviour
    {
        public FleeTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var enemy = FindAnEnemy(state);

            if (NotFound(enemy))
            {
                var food = FindHighestValueFood(state);

                if (Found(food))
                {
                    return MoveTowards(food);
                }
                else
                {
                    return RandomWalk();
                }
            }
            else
            {
                return MoveAwayFrom(enemy);
            }
        }
    }
}