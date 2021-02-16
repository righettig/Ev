using Ev.Domain.Actions.Core;
using Ev.Domain.Actions.Core.Processors;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System.Collections.Generic;

namespace Ev.Domain.World
{
    public interface IWorld
    {
        int Size { get; }
                
        IWorldEntity[,] State { get; }
        
        IEnumerable<ITribe> Tribes { get; }

        bool Finished { get; }

        ITribe Winner { get; }

        IWorld WithTribe(string tribeName, Color darkYellow, ITribeBehaviour behaviour);

        IEnumerable<ITribe> GetAliveTribes();

        IWorldState GetWorldState(ITribe tribe);

        bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor);

        void WipeTribe(ITribe target, int iteration);
    }
}