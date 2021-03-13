namespace Ev.Domain.Server.Core
{
    public interface IGameAction
    {
        ITribe Tribe { get; set; }
    }
}