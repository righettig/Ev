using Ev.Common.Core.Interfaces;

namespace Ev.Domain.Client.Core
{
    public interface ITribeBehaviour 
    {
        IWorldState State { get; set; }

        IGameAction DoMove(IWorldState state, ITribe tribe);
    }
}