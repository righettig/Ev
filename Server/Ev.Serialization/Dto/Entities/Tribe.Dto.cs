using Ev.Serialization.Dto.Entities.Core;

namespace Ev.Serialization.Dto.Entities
{
    public class TribeDto : IWorldEntityDto
    {
        public string Name { get; set; }
        public int Population { get; set; }
    }
}
