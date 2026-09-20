using System;
using Strangeman.Utils.Evaluation;

namespace Strangeman.Utils.State
{
    public partial class StateMachine
    {
        public Strangeman.Utils.State.StateMachine.Transition At(IState from, IState to, IEvaluate predicate) => AddTransition(from, to, predicate);

        public Transition At(IState from, IState to, Func<bool> predicate) => AddTransition(from, to, Predicate.Pull(predicate));

        public Transition At(IState to, IEvaluate predicate) => AddAnyTransition(to, predicate);

        public Transition At(IState to, Func<bool> predicate) => AddAnyTransition(to, Predicate.Pull(predicate));
    }
}
