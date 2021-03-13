namespace Ev.Domain.Client.Core
{
    public interface ITribeAgent
    {
        string Name { get; }
        string Color { get; }

        ITribeBehaviour Behaviour { get; }
    }
}