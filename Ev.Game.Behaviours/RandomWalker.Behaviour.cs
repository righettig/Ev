using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;

namespace Ev.Behaviours
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
                return Move((Direction)move);
            }
        }
    }
}