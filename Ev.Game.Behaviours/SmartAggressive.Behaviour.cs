using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class SmartAggressiveTribeBehaviour : AggressiveTribeBehaviour
    {
        public SmartAggressiveTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var enemy = FindAnEnemy(state);

            if (NotFound(enemy))
            {
                var food = FindHighestValueFood(state);

                if (NotFound(food))
                {
                    return RandomWalk();
                }
                else
                {
                    return MoveTowards(food);
                }
            }
            else if (Close(enemy))
            {
                var enemyRef = state.GetEntity<ITribe>(enemy);

                if (tribe.StrongerThan(enemyRef))
                {
                    return new AttackAction(enemyRef);
                }
                else 
                {
                    return RandomWalk();
                } 
            }
            else
            {
                return MoveTowards(enemy);
            }
        }
    }
}