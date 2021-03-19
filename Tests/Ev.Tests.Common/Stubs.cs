using Ev.Common.Core.Interfaces;
using Ev.Domain.Client.Behaviours.BehaviourTrees;
using Ev.Domain.Client.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.World.Core;
using Ev.Infrastructure;
using Ev.Infrastructure.Core;
using Moq;

namespace Ev.Tests.Common
{
    public static class Stubs
    {
        public static readonly IEnumeration IEnumeration = new Mock<IEnumeration>().Object;

        public static readonly IWorldState IIWorldState = new Mock<IWorldState>().Object;

        public static readonly ITribeAgent ITribeAgent = new Mock<ITribeAgent>().Object;

        public static readonly IRandom IRandom = new Mock<IRandom>().Object;

        public static readonly IGame IGame = new Mock<IGame>().Object;

        public static readonly IMapper IMapper = new Mock<IMapper>().Object;

        public static readonly ITribeBehaviour ITribeBehaviour = new Mock<ITribeBehaviour>().Object;

        public static readonly IBehaviourTreeContext IBehaviourTreeContext = new Mock<IBehaviourTreeContext>().Object;

        public static readonly IBehaviourTreeNode IBehaviourTreeNode = new Mock<IBehaviourTreeNode>().Object;

        public static readonly IPlatform IPlatform = new Mock<IPlatform>().Object;

        public static readonly IWorld IWorld = new Mock<IWorld>().Object;

        public static readonly IGameActionProcessor IGameActionProcessor = new Mock<IGameActionProcessor>().Object;

        public static readonly IAttackOutcomePredictor IAttackOutcomePredictor = new Mock<IAttackOutcomePredictor>().Object;

        public static readonly ServerStubs Server = new();

        public static readonly ClientStubs Client = new();
    }
}