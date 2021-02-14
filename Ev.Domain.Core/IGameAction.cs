namespace Ev.Domain.Core
{
    public interface IGameAction 
    {
        ITribe Tribe { get; set; }
    }
}