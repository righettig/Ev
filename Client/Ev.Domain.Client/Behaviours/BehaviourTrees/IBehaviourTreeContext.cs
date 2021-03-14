using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Core;

namespace Ev.Domain.Client.Behaviours.BehaviourTrees
{
    public interface IBehaviourTreeContext
    {
        IWorldState WorldState { get; }

        ITribe TribeState { get; }

        IGameAction Move { get; set; }
    }
}