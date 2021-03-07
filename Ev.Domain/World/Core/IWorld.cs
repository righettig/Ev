using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;

namespace Ev.Domain.World.Core
{
    public interface IWorld
    {
        int Size { get; }
                
        IWorldEntity[,] State { get; }

        ITribe[] Tribes { get; }

        bool Finished { get; }

        ITribe Winner { get; }

        IWorld AddTribe(string tribeName, Color color);

        ITribe[] GetAliveTribes();

        IWorldState GetWorldState(ITribe tribe);

        bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor);

        void WipeTribe(ITribe target, int iteration);

        bool CanMove((int x, int y) position, Direction direction);
        void Move(ITribe tribe, Direction direction);
    }
}