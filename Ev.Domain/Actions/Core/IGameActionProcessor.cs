using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;

namespace Ev.Domain.Actions.Core
{
    public interface IGameActionProcessor
    {
        void Update(IGameAction action, ITribe tribe, IWorld world, int iteration); // iteration is "turn"
    }

    public interface IGameActionProcessor<T> where T : IGameAction
    {
        void Update(T action, ITribe tribe, IWorld world, int iteration); // iteration is "turn"
    }
}