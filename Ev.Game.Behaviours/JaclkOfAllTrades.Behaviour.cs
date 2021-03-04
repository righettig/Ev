using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
{
    public class JackOfAllTradesTribeBehaviour : TribeBehaviour
    {
        public JackOfAllTradesTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            var collectable = FindACollectable();

            if (NotFound(collectable))
            {
                return RandomWalk();
            }
            else
            {
                return MoveTowards(collectable);
            }
        }
    }
}