using Ev.Common.Core;
using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Behaviours.Fsm;
using Ev.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ev.Tests.Common.TestHelpers;

namespace Ev.Domain.Client.Tests.Fsm
{
    class MyFsmStates : Enumeration
    {
        public static MyFsmStates Foo = new(1, nameof(Foo));
        public static MyFsmStates Bar = new(2, nameof(Bar));

        private MyFsmStates(int id, string name) : base(id, name)
        {
        }
    }

    [TestClass]
    public class FiniteStateMachineTests
    {
        private readonly FiniteStateMachine _uat = new();

        [TestMethod]
        public void WithState_Should_Throw_ArgumentNullException_If_FsmState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.WithState(null));
        }

        [TestMethod]
        public void WithInitialState_Should_Throw_ArgumentNullException_If_Enumeration_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.WithInitialState(null));
        }

        [TestMethod]
        public void WithInitialState_Should_Throw_FsmStateNotFoundException_If_State_Is_Unknown()
        {
            Assert.ThrowsException<FsmStateNotFoundException>(() => _uat.WithInitialState(MyFsmStates.Foo));
        }

        [TestMethod]
        public void WithState_Should_Throw_InvalidFsmStateNotFoundException_If_State_Is_Missing_ActionFn()
        {
            Assert.ThrowsException<InvalidFsmStateNotFoundException>(
                () =>
                {
                    var fsmState = new FsmState(MyFsmStates.Foo) {TransitionFn = (state, tribe) => MyFsmStates.Bar};

                    return _uat.WithState(fsmState);
                });
        }

        [TestMethod]
        public void WithState_Should_Throw_InvalidFsmStateNotFoundException_If_State_Is_Missing_TransitionFn()
        {
            Assert.ThrowsException<InvalidFsmStateNotFoundException>(
                () =>
                {
                    var fsmState = new FsmState(MyFsmStates.Foo) { ActionFn = (state, tribe) => new HoldAction() };

                    return _uat.WithState(fsmState);
                });
        }

        [TestMethod]
        public void WithInitialState_Should_Not_Throw_FsmStateNotFoundException_If_State_Is_Known()
        {
            // Arrange
            var fsmState = new FsmState(MyFsmStates.Foo)
            {
                ActionFn     = (state, tribe) => new HoldAction(),
                TransitionFn = (state, tribe) => MyFsmStates.Bar
            };

            _uat.WithState(fsmState);

            // Act
            _uat.WithInitialState(MyFsmStates.Foo);
        }

        [TestMethod]
        public void DoMove_Should_Throw_ArgumentNullException_If_IWorldState_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.DoMove(null, Stubs.Client.ITribe));
        }

        [TestMethod]
        public void DoMove_Should_Throw_ArgumentNullException_If_ITribe_Is_Null()
        {
            ShouldThrowArgumentNullException(() => _uat.DoMove(Stubs.IIWorldState, null));
        }

        [TestMethod]
        public void DoMove_Should_Return_Action_Based_On_Current_State()
        {
            // Arrange
            _uat.WithState(new FsmState(MyFsmStates.Foo)
                {
                    ActionFn     = (state, tribe) => new HoldAction(),
                    TransitionFn = (state, tribe) => MyFsmStates.Bar
                })
                .WithState(new FsmState(MyFsmStates.Bar)
                {
                    ActionFn     = (state, tribe) => new AttackAction("target"),
                    TransitionFn = (state, tribe) => MyFsmStates.Foo
                })
                .WithInitialState(MyFsmStates.Foo);

            // Act
            var action = _uat.DoMove(Stubs.IIWorldState, Stubs.Client.ITribe);

            // Assert
            Assert.IsInstanceOfType(action, typeof(HoldAction));
        }

        [TestMethod]
        public void DoMove_Should_Transition_To_New_State_According_To_Transition_Fn()
        {
            // Arrange
            _uat.WithState(new FsmState(MyFsmStates.Foo)
                {
                    ActionFn     = (state, tribe) => new HoldAction(),
                    TransitionFn = (state, tribe) => MyFsmStates.Bar
                })
                .WithState(new FsmState(MyFsmStates.Bar)
                {
                    ActionFn     = (state, tribe) => new AttackAction("target"),
                    TransitionFn = (state, tribe) => MyFsmStates.Foo
                })
                .WithInitialState(MyFsmStates.Foo);

            // Act
            _uat.DoMove(Stubs.IIWorldState, Stubs.Client.ITribe);

            // Assert
            Assert.AreSame(MyFsmStates.Bar, _uat.Current.Id);
        }

        [TestMethod]
        public void Debug_Should_Return_Current_State()
        {
            // Arrange
            _uat.WithState(new FsmState(MyFsmStates.Foo)
                {
                    ActionFn = (state, tribe) => new HoldAction(),
                    TransitionFn = (state, tribe) => MyFsmStates.Bar
                })
                .WithState(new FsmState(MyFsmStates.Bar)
                {
                    ActionFn = (state, tribe) => new AttackAction("target"),
                    TransitionFn = (state, tribe) => MyFsmStates.Foo
                })
                .WithInitialState(MyFsmStates.Foo);

            // Act
            _uat.DoMove(Stubs.IIWorldState, Stubs.Client.ITribe);

            // Assert
            Assert.AreEqual("Foo -> Bar", _uat.ToString());
        }
    }
}
