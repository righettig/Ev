namespace Ev.Serialization.Dto.Entities.Core
{
    public abstract class BaseWorldEntityDto : IWorldEntityDto
    {
        public string EntityType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public IWorldEntityDto WithPosition(int x, int y) 
        { 
            X = x; 
            Y = y; 
            
            return this; 
        }
    }
}
