using Ev.Domain.Actions.Core;
using Ev.Domain.World.Core;
using Force.DeepCloner;
using System.Collections.Generic;

namespace Ev.Game
{
    public class GameHistory 
    {
        private readonly List<(IGameAction, IWorldState)> _states = new();

        public IEnumerable<(IGameAction, IWorldState)> States => _states;

        public void Add((IGameAction, IWorldState) state) 
        {
            _states.Add(state.DeepClone());
        }
    }
}
