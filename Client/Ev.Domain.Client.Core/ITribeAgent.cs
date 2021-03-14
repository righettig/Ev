using Ev.Common.Core;

namespace Ev.Domain.Client.Core
{
    public interface ITribeAgent
    {
        string Name { get; }

        Color Color { get; }

        ITribeBehaviour Behaviour { get; }
    }
}