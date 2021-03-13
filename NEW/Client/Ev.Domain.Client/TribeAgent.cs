using Ev.Domain.Client.Core;

namespace Ev.Domain.Client
{
    public class TribeAgent : ITribeAgent
    {
        public string Name { get; }
        public string Color { get; }
        public ITribeBehaviour Behaviour { get; }

        public TribeAgent(string name, string color, ITribeBehaviour behaviour)
        {
            Name = name;
            Color = color;
            Behaviour = behaviour;
        }
    }
}
