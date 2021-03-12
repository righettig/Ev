using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using System.Collections.Generic;

namespace Ev.Domain.Behaviours.BehaviourTrees
{
    public interface IBehaviourTreeContext
    {
        IWorldState WorldState { get; }

        ITribeState TribeState { get; }

        IGameAction Move { get; set; }

        //IDictionary<string, object> Context { get; }

        //bool Valid { get; }
    }
}