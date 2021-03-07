using Ev.Tribe;

namespace Ev.Agents.GameMaster.Proxies.Core
{
    internal interface ITribeProxy
    {
        void Start();
        DoMoveReply DoMove(DoMoveRequest request);
        void End();
        void Shutdown();
    }
}