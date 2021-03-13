using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.Actions.Core
{
    public interface IAttackOutcomePredictor 
    {
        bool CanWin(ITribe attacker, ITribe defender);
    }
}