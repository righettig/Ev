using Ev.Common;
using Ev.Common.Utils;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;

namespace Ev.Domain.Server.World.Core
{
    public interface IWorld
    {
        int Size { get; }
                
        IWorldEntity[,] State { get; }

        ITribe[] Tribes { get; }

        bool Finished { get; }

        ITribe Winner { get; }

        void AddTribe(string tribeName, Color color);

        ITribe[] GetAliveTribes();

        IWorldState GetWorldState(ITribe tribe);

        bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor);

        void WipeTribe(ITribe target, int iteration);

        bool CanMove((int x, int y) position, Direction direction);

        void Move(ITribe tribe, Direction direction);
    }
}