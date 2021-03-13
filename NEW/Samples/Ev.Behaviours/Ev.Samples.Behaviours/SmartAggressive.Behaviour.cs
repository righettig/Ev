using Ev.Common.Utils;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class SmartAggressiveTribeBehaviour : AggressiveTribeBehaviour
    {
        public SmartAggressiveTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var enemy = FindAnEnemy();

            if (NotFound(enemy))
            {
                var food = FindHighestValueFood();

                if (NotFound(food))
                {
                    return RandomWalk();
                }

                return MoveTowards(food);
            }

            if (Close(enemy))
            {
                var enemyRef = state.GetEntity<ITribe>(enemy);

                if (tribe.StrongerThan(enemyRef))
                {
                    return Attack(enemyRef);
                }

                return RandomWalk();
            }

            return MoveTowards(enemy);
        }
    }
}