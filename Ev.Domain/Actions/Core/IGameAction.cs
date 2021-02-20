using Ev.Domain.Entities.Core;

namespace Ev.Domain.Actions.Core
{
    public interface IGameAction 
    {
        ITribe Tribe { get; internal set; }
    }
}