using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class AggressiveTribeBehaviour : TribeBehaviour
    {
        public AggressiveTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var enemy = FindAnEnemy(state);

            if (NotFound(enemy))
            {
                return RandomWalk();
            }
            else if (Close(enemy)) 
            {
                return new AttackAction(state.GetEntity<ITribe>(enemy));
            }
            else
            {
                return MoveTowards(enemy);
            }
        }
    }
}