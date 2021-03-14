using Ev.Common.Core;
using Ev.Domain.Client.Behaviours.Core;
using Ev.Domain.Client.Core;

namespace Ev.Samples.Behaviours
{
    public class ExplorerTribeBehaviour : TribeBehaviour 
    {
        private Direction _currentDir;

        public ExplorerTribeBehaviour(IRandom rnd) : base(rnd) { }

        public override IGameAction DoMove(IWorldState state, ITribe tribe)
        {
            // initial move OR can no longer move in the same direction
            if (tribe.PrevPosition.x == tribe.Position.x && tribe.PrevPosition.y == tribe.Position.y)
            {
                PickDirection();
            }

            return Move(_currentDir);
        }

        private void PickDirection()
        {
            _currentDir = (Direction)_rnd.Next(8);
        }
    }
}