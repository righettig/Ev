namespace Ev_NEW
{
    internal class AttackAction : IGameAction
    {
        private string target;

        public AttackAction(string target)
        {
            this.target = target;
        }
    }
}