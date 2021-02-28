namespace Ev_NEW
{
    public interface ITribeBehaviour
    {
        IGameAction DoMove(IWorldState state, ITribe tribe);
    }
}
