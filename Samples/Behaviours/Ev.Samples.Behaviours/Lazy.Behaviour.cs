using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class LazyTribeBehaviour : TribeBehaviour 
    {
        public LazyTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe) => Hold();
    }
}