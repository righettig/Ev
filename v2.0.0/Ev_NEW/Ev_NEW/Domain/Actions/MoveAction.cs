namespace Ev_NEW
{
    internal class MoveAction : IGameAction
    {
        private Direction direction;

        public MoveAction(Direction direction)
        {
            this.direction = direction;
        }
    }
}