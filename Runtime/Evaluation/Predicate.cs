using System;

namespace Strangeman.Utils.Evaluation
{
    //Push-based Predicates with Events, Evaluation is meant to call OnSuccess or OnFailure.
    //This becomes very powerful when used with ObservableProperty.
    public abstract class ObservablePredicate<T> : IEvaluate, IDisposable
    {
        protected readonly T ConditionSource;

        protected ObservablePredicate(T conditionSource) => ConditionSource = conditionSource;
        
        protected void TriggerSuccess() => OnSuccess?.Invoke();
        public event Action OnSuccess;
        protected void TriggerFailure() => OnFailure?.Invoke();
        public event Action OnFailure;
        
        //Make sure to call Evaluate when constructing a predicate to avoid missing initial state.
        public abstract void Evaluate();
        public abstract void Dispose();
    }

    public sealed class Predicate : IEvaluate, IDisposable
    {
        private readonly IObservable _conditionSource;
        private readonly Func<bool> _condition;
        
        private Predicate(Func<bool> condition)
        {
            _condition = condition;
            _conditionSource = null;
        }

        private Predicate(IObservable observable, Func<bool> condition)
        {
            _conditionSource = observable;
            _condition = condition;
        }
        
        //Pull = I want to be responsible for when to Evaluate.
        public static Predicate Pull(Func<bool> condition)
        {
            return new Predicate(condition);
        }
        
        //Push = I want the observable to be responsible for when to Evaluate.
        public static Predicate Push(IObservable observable, Func<bool> condition)
        {
            var predicate = new Predicate(observable, condition);
            observable.OnValueChanged += predicate.Evaluate;
            return predicate;
        }

        public event Action OnSuccess;
        public event Action OnFailure;
        
        //Call Evaluate first after subscribing to predicate events to avoid missing initial state.
        public void Evaluate()
        {
            if (_condition())
                OnSuccess?.Invoke();
            else OnFailure?.Invoke();
        }

        public void Dispose()
        {
            if (_conditionSource is not null)
            {
                _conditionSource.OnValueChanged -= Evaluate;
            }
        }
    }

    /* Example
    public sealed class DeathPredicate : ObservablePredicate<TestHealth>
    {
        private ObservableProperty<int> _healthProperty;
        
        public DeathPredicate(TestHealth conditionSource) : base(conditionSource)
        {
            _healthProperty = conditionSource.Health;
            _healthProperty.OnValueChanged += Evaluate;
        }
        
        public override void Evaluate()
        {
            if (_healthProperty.Value > 0)
                TriggerFailure();
            else TriggerSuccess();
        }

        public override void Dispose()
        {
            _healthProperty.OnValueChanged -= Evaluate;
        }
    }

    public class TestHealth : IDisposable
    {
        public TestHealth ()
        {
            Health = new ObservableProperty<int>(100);
            _deathPredicate = new DeathPredicate(this);
            _deathPredicate.OnSuccess += () => Debug.Log("Death");
        }

        public readonly ObservableProperty<int> Health;
        private readonly DeathPredicate _deathPredicate;
        
        public void Dispose()
        {
            _deathPredicate.Dispose();
        }
    }
    */
}