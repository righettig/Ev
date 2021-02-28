using System.Linq;

namespace Ev_NEW
{
    class TestTribeBehaviour : TribeBehaviour
    {
        public TestTribeBehaviour(IRandom rnd) : base(rnd)
        {
        }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            var c = FindACollectable(state);
            var e = FindAnEnemy(state);
            var x = FindHighestValueFood(state);

            //var foo = state.Closest<ICollectable>();

            return Attack(e);
        }
    }
}
