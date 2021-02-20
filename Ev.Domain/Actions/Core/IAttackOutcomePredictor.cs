using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions.Core
{
    public interface IAttackOutcomePredictor 
    {
        bool CanWin(ITribe attacker, ITribe defender);
    }
}