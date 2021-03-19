using Ev.Common.Core;
using Ev.Common.Core.Interfaces;

namespace Ev.Domain.Client.Core
{
    public interface ITribeAgent
    {
        string Name { get; }

        Color Color { get; }

        ITribeBehaviour Behaviour { get; }

        #region Game event handlers

        void OnGameStart();
        
        void OnTurnStart();

        void OnBeforeMove(IWorldState worldState, ITribe tribe);

        void OnTurnEnd();

        void OnGameEnd();

        #endregion
    }
}