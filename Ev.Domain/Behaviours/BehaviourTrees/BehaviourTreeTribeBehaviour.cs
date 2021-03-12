using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.BehaviourTrees.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using System;

namespace Ev.Domain.Behaviours.BehaviourTrees
{
    public abstract class BehaviourTreeTribeBehaviour : TribeBehaviour
    {
        private readonly IBehaviourTreeNode _root;

        // ReSharper disable once VirtualMemberCallInConstructor
        protected BehaviourTreeTribeBehaviour(IRandom rnd) : base(rnd) => _root = CreateRoot();

        protected abstract IBehaviourTreeNode CreateRoot();

        public override IGameAction DoMove(IWorldState state, ITribeState tribe)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (tribe == null) throw new ArgumentNullException(nameof(tribe));

            var context = new BehaviourTreeContext(state, tribe);

            _root.Tick(context);
            
            return context.Move ?? Hold();
        }
    }
}
