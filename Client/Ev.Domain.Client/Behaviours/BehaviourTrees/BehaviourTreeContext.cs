using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Core;
using System;
using System.Collections.Generic;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees
{
    public class BehaviourTreeContext : IBehaviourTreeContext
    {
        public IWorldState WorldState => _context[WORLD_STATE] as IWorldState;

        public ITribe TribeState => _context[TRIBE_STATE] as ITribe;

        public IGameAction Move
        {
            get => _context[MOVE] as IGameAction;
            set => _context[MOVE] = value;
        }

        public object this[string key]
        {
            get => _context[key];
            set => Set(key, value);
        }

        private const string WORLD_STATE = "worldState";
        private const string TRIBE_STATE = "tribeState";
        private const string MOVE        = "move";

        private readonly Dictionary<string, object> _context = new();

        public BehaviourTreeContext(IWorldState worldState, ITribe tribeState)
        {
            _context[WORLD_STATE] = worldState ?? throw new ArgumentNullException(nameof(worldState));
            _context[TRIBE_STATE] = tribeState ?? throw new ArgumentNullException(nameof(tribeState));
        }

        public void Set(string key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (key == WORLD_STATE || key == TRIBE_STATE)
            {
                throw new ArgumentException("Cannot set read-only entries: worldState or tribeState.", nameof(key));
            }

            _context.Add(key, value);
        }

        public object Get(string key) => _context[key];
    }
}