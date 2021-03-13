using Ev.Common.Utils;
using Ev.Domain.Client.Core;
using System;

namespace Ev.Domain.Client
{
    public class TribeAgent : ITribeAgent
    {
        public string Name { get; }
        public Color Color { get; }
        public ITribeBehaviour Behaviour { get; }

        public TribeAgent(string name, Color color, ITribeBehaviour behaviour)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            Name      = name;
            Color     = color;
            Behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour));
        }
    }
}
