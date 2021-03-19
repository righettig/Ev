using Ev.Common.Core.Interfaces;

namespace Ev.Infrastructure
{
    public interface IMapper
    {
        IWorldState Map(IWorldState worldState);
        Domain.Client.Core.ITribe Map(Domain.Server.Core.ITribe tribe);
        Domain.Server.Core.IGameAction Map(Domain.Client.Core.IGameAction action);
    }
}