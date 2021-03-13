using Ev.Domain.Server.Core;
using Force.DeepCloner;
using System.Collections.Generic;

namespace Ev.Game.Server
{
    public class EvGameHistory 
    {
        private readonly List<(IGameAction, IWorldState)> _states = new();

        public IEnumerable<(IGameAction, IWorldState)> States => _states;

        public void Add((IGameAction, IWorldState) state) 
        {
            _states.Add(state.DeepClone());
        }
    }
}
