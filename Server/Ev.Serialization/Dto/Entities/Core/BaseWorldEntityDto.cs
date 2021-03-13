namespace Ev.Serialization.Dto.Entities.Core
{
    public abstract class BaseWorldEntityDto : IWorldEntityDto
    {
        public string EntityType { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public IWorldEntityDto WithPosition(int x, int y) 
        { 
            X = x; 
            Y = y; 
            
            return this; 
        }
    }
}
