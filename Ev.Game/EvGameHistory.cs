using Ev.Domain.Actions.Core;
using Ev.Domain.World.Core;
using Force.DeepCloner;
using System.Collections.Generic;

namespace Ev.Game
{
    public class EvGameHistory 
    {
        private readonly List<(IGameAction, IWorldState)> _states = new List<(IGameAction, IWorldState)>();

        public List<(IGameAction, IWorldState)> States { get => _states; }

        public void Add((IGameAction, IWorldState) state) 
        {
            _states.Add(state.DeepClone());
        }
    }
}
