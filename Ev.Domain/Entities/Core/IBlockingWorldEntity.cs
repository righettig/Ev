namespace Ev.Domain.Entities.Core
{
    public interface IBlockingWorldEntity : IWorldEntity
    {
    }

    public abstract class BaseBlockingWorldEntity : IBlockingWorldEntity
    {
        public IWorldEntity ToImmutable() 
        {
            return this;
        }
    }
}