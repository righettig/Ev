using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;

namespace Ev.Domain.Server.Actions.Core
{
    public interface IGameActionProcessor
    {
        void Update(IGameAction action, ITribe tribe, IWorld world, int iteration); // iteration is "turn"
    }

    public interface IGameActionProcessor<in T> where T : IGameAction
    {
        void Update(T action, ITribe tribe, IWorld world, int iteration); // iteration is "turn"
    }
}