using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class RandomWalkerTribeBehaviour : TribeBehaviour
    {
        public RandomWalkerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var move = _rnd.Next(9);

            if (move == 8)
            {
                return Hold();
            }
            else
            {
                return Move((Direction) move);
            }
        }
    }
}