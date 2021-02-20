namespace Ev.Domain.Actions.Core
{
    public abstract class BlockingGameAction : GameAction 
    {
        public bool Completed { get; set; }

        public abstract string Result();
    }
}