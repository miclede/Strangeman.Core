using System;
using Strangeman.Utils.Evaluation;

namespace Strangeman.Utils.State
{
    public partial class StateMachine
    {
        public sealed class Transition
        {
            internal Strangeman.Utils.State.StateMachine Owner { get; }
            
            internal IState From { get; }
            public IState To { get; }
            public IEvaluate Predicate { get; }
            
            private bool _isSubscribed;
            private bool _isRemoved;

            internal Transition(Strangeman.Utils.State.StateMachine owner, IState from, IState to, IEvaluate predicate)
            {
                Owner = owner;
                From = from;
                To = to;
                Predicate = predicate;
            }

            internal void Subscribe()
            {
                if (_isSubscribed || _isRemoved)
                    return;

                Predicate.OnSuccess += OnPredicateSuccess;
                Predicate.Evaluate(); //Always evaluate predicate after subscribing to avoid missing initial state
                _isSubscribed = true;
            }

            internal void Unsubscribe()
            {
                if (!_isSubscribed)
                    return;

                Predicate.OnSuccess -= OnPredicateSuccess;
                _isSubscribed = false;
            }

            internal void Dispose()
            {
                if (_isRemoved)
                    return;

                Unsubscribe();
                _isRemoved = true;

                if (Predicate is IDisposable disposable)
                    disposable.Dispose();
            }

            internal bool Remove()
            {
                if (_isRemoved)
                    return false;

                Dispose();
                Owner.RemoveTransitionData(this);
                return true;
            }

            private void OnPredicateSuccess()
            {
                Owner.SetState(To);
            }
        }
    }
}