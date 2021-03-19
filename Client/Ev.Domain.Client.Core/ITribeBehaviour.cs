using Ev.Common.Core.Interfaces;

namespace Ev.Domain.Client.Core
{
    public interface ITribeBehaviour 
    {
        IGameAction DoMove(IWorldState state, ITribe tribe);
    }
}