using System;
using System.Collections.Generic;
using Strangeman.Utils.Evaluation;
using UnityEngine;

namespace Strangeman.Utils.State
{
    public partial class StateMachine : IDisposable
    {
        private static readonly IReadOnlyCollection<StateMachine.Transition> EmptyTransitions =
            Array.Empty<StateMachine.Transition>();

        private readonly Dictionary<Type, HashSet<StateMachine.Transition>> _transitions = new();
        private readonly HashSet<StateMachine.Transition> _anyTransitions = new();
        private readonly Queue<IState> _pendingStates = new();
        private IReadOnlyCollection<StateMachine.Transition> _currentTransitions = EmptyTransitions;

        private bool _isChangingState;
        private bool _isDisposed;

        public IState CurrentState { get; private set; }

        public event Action<IState, IState> OnStateChanged;

        public void PreTick()
        {
            if (_isDisposed)
                return;

            CurrentState?.PreTick();
        }

        public void Tick()
        {
            if (_isDisposed)
                return;

            CurrentState?.Tick();
        }

        public void PostTick()
        {
            if (_isDisposed)
                return;

            CurrentState?.PostTick();
        }
        
        //Good for, initial state machine, state entry.
        public void ForceState(IState state) => SetState(state);

        private void SetState(IState state)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateMachine));

            RequestState(state);
            ProcessPendingStates();
        }

        private StateMachine.Transition AddTransition(IState from, IState to, IEvaluate predicate)
        {
            var transition = new StateMachine.Transition(this, from, to, predicate);

            if (!_transitions.TryGetValue(from.GetType(), out var transitions))
            {
                transitions = new HashSet<StateMachine.Transition>();
                _transitions.Add(from.GetType(), transitions);
            }

            transitions.Add(transition);

            if (CurrentState == from)
                transition.Subscribe();

            return transition;
        }

        private StateMachine.Transition AddAnyTransition(IState to, IEvaluate predicate)
        {
            var transition = new StateMachine.Transition(this, null, to, predicate);

            _anyTransitions.Add(transition);
            transition.Subscribe();
            return transition;
        }

        public bool RemoveTransition(StateMachine.Transition transition)
        {
            if (transition == null || !ReferenceEquals(transition.Owner, this))
                return false;

            return transition.Remove();
        }

        public bool RemoveAnyTransition(IState to, IEvaluate predicate)
        {
            if (to == null || predicate == null)
                return false;

            foreach (var transition in _anyTransitions)
            {
                if (transition.To != to)
                    continue;
                
                if (transition.Predicate != predicate)
                    continue;

                transition.Dispose();
                _anyTransitions.Remove(transition);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            
            UnsubscribeTransitions(_currentTransitions);

            var disposedPredicates = new HashSet<IEvaluate>();
            foreach (var transitions in _transitions.Values)
            {
                foreach (var transition in transitions)
                {
                    transition.Unsubscribe();
                    if (disposedPredicates.Add(transition.Predicate))
                        DisposePredicate(transition.Predicate);
                }
            }

            foreach (var transition in _anyTransitions)
            {
                transition.Unsubscribe();
                if (disposedPredicates.Add(transition.Predicate))
                    DisposePredicate(transition.Predicate);
            }

            _pendingStates.Clear();
            _currentTransitions = EmptyTransitions;
            _transitions.Clear();
            _anyTransitions.Clear();
            StateMachineRegistry.Instance.RemoveMachine(this);
            _isDisposed = true;
        }

        private void RequestState(IState state)
        {
            if (CurrentState == state)
                return;

            _pendingStates.Enqueue(state);
        }

        private void ProcessPendingStates()
        {
            if (_isChangingState || _isDisposed)
                return;

            var stateChanges = 0;
            while (_pendingStates.Count > 0)
            {
                if (++stateChanges > 100)
                {
                    _pendingStates.Clear();
                    Debug.LogError("Excessive State Changes Detected: StateMachine stopped processing transitions after 100 consecutive state changes.",
                        RunnerObject);
                    return;
                }

                var nextState = _pendingStates.Dequeue();
                if (CurrentState == nextState)
                    continue;

                ChangeState(nextState);
            }
        }

        private void ChangeState(IState state)
        {
            _isChangingState = true;
            UnsubscribeTransitions(_currentTransitions);

            var previousState = CurrentState;
            previousState?.OnExit();
            CurrentState = state;
            _currentTransitions = GetTransitionsFor(state);

            SubscribeTransitions(_currentTransitions);
            CurrentState.OnEnter();

            _isChangingState = false;
            OnStateChanged?.Invoke(previousState, CurrentState);
        }

        private IReadOnlyCollection<StateMachine.Transition> GetTransitionsFor(IState state)
        {
            return _transitions.TryGetValue(state.GetType(), out var transitions)
                ? transitions
                : EmptyTransitions;
        }

        private static void SubscribeTransitions(IEnumerable<StateMachine.Transition> transitions)
        {
            foreach (var transition in transitions)
                transition.Subscribe();
        }

        private static void UnsubscribeTransitions(IEnumerable<StateMachine.Transition> transitions)
        {
            foreach (var transition in transitions)
                transition.Unsubscribe();
        }

        private static void DisposePredicate(IEvaluate predicate)
        {
            if (predicate is IDisposable disposable)
                disposable.Dispose();
        }

        private void RemoveTransitionData(StateMachine.Transition transition)
        {
            if (transition.From == null)
            {
                _anyTransitions.Remove(transition);
                return;
            }

            if (!_transitions.TryGetValue(transition.From.GetType(), out var transitions))
                return;

            transitions.Remove(transition);
            if (transitions.Count == 0)
                _transitions.Remove(transition.From.GetType());
        }
    }
}