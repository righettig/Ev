namespace Ev.Agents.Core
{
    public interface IAgent<T> where T : class
    {
        T Start();
        T Connect(string serverUrl);
        void Shutdown();
    }
}