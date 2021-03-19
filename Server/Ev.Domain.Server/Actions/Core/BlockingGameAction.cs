using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Actions.Core
{
    public abstract class BlockingGameAction : GameAction 
    {
        public bool Completed { get; set; }

        public abstract string Result();

        public abstract void OnComplete(ITribe tribe);
    }
}