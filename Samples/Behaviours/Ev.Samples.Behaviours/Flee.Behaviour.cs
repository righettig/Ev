using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
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

                return RandomWalk();
            }

            return MoveAwayFrom(enemy);
        }
    }
}