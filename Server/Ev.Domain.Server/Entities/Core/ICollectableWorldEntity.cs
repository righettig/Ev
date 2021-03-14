using Ev.Common.Core;

namespace Ev.Domain.Server.Entities.Core
{
    public interface ICollectableWorldEntity : IWorldEntity 
    {
        int Value { get; }
    }
}
