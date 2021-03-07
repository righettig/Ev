using Ev.Domain.World.Core;

namespace Ev.Agents.GameMaster.Proxies.Core
{
    internal interface ISpectatorProxy
    {
        void Start();
        void Update(IWorld world);
        void End();
        void Shutdown();
    }
}