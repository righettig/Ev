using Ev.Serialization.Dto.Entities;

namespace Ev.Serialization.Dto.Actions.Core
{
    public abstract class GameActionDto : IGameActionDto
    {
        public TribeDto Tribe { get; }
    }
}