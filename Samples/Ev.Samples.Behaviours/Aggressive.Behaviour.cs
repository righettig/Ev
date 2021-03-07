using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Samples.Behaviours
{
    public class AggressiveTribeBehaviour : TribeBehaviour
    {
        public AggressiveTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            var enemy = FindAnEnemy();

            if (NotFound(enemy))
            {
                return RandomWalk();
            }
            else if (Close(enemy)) 
            {
                return Attack(state.GetEntity<ITribeState>(enemy));
            }
            else
            {
                return MoveTowards(enemy);
            }
        }
    }
}