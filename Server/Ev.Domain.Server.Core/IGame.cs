using Ev.Common.Core;

namespace Ev.Domain.Server.Core
{
    public interface IGame
    {
        void RegisterAgent(string agentName, Color agentColor);
    }
}