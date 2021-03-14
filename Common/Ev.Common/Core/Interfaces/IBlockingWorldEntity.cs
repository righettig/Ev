namespace Ev.Common.Core.Interfaces
{
    public interface IBlockingWorldEntity : IWorldEntity
    {
        BlockingWorldEntityType Type { get; }
    }
}