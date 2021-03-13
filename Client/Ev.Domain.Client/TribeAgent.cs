using Ev.Common.Utils;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client
{
    public class TribeAgent : ITribeAgent
    {
        public string Name { get; }
        public Color Color { get; }
        public ITribeBehaviour Behaviour { get; }

        public TribeAgent(string name, Color color, ITribeBehaviour behaviour)
        {
            Name      = name;
            Color     = color;
            Behaviour = behaviour;
        }
    }
}
