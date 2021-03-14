using Ev.Common.Core;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class JackOfAllTradesTribeBehaviour : TribeBehaviour
    {
        public JackOfAllTradesTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var collectable = FindACollectable();

            if (NotFound(collectable))
            {
                return RandomWalk();
            }

            return MoveTowards(collectable);
        }
    }
}