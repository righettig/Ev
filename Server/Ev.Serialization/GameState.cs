using Ev.Serialization.Dto.Actions.Core;
using Ev.Serialization.Dto.Entities.Core;
using System.Collections.Generic;

namespace Ev.Serialization
{
    class GameState 
    { 
        public IGameActionDto Action { get; set; }
        public List<IWorldEntityDto> State { get; set; }

        public GameState() => State = new List<IWorldEntityDto>();
    }
}
