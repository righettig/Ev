using Ev.Common;

namespace Ev.Domain.Client.Core
{
    public interface ITribeBehaviour
    {
        IGameAction DoMove(IWorldState worldState, ITribe tribe);
    }
}