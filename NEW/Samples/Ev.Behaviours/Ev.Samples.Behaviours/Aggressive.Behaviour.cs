using Ev.Common.Utils;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class AggressiveTribeBehaviour : TribeBehaviour
    {
        public AggressiveTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var enemy = FindAnEnemy();

            if (NotFound(enemy))
            {
                return RandomWalk();
            }

            if (Close(enemy)) 
            {
                return Attack(state.GetEntity<ITribe>(enemy));
            }

            return MoveTowards(enemy);
        }
    }
}