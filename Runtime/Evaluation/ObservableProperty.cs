using System;
using System.Collections.Generic;

namespace Strangeman.Utils.Evaluation
{
    /// <summary>
    /// Represents a property that notifies observers when its value has changed.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value stored in the property.
    /// </typeparam>
    /// 
    /// <remarks>
    /// The <see cref="ObservableProperty{T}"/> class provides a mechanism to observe and react to
    /// changes in a property value. It supports notification of value changes through the
    /// <see cref="OnValueChanged"/> event and maintains the previous value for reference.
    /// </remarks>
    public sealed class ObservableProperty<T> : IObservable
    {
        public T OldValue { get; private set; }
        private T _value;

        public event Action OnValueChanged;

        /// <summary>
        /// Gets or sets the current value of the property.
        /// When the value changes, the <see cref="OnValueChanged"/> event is triggered.
        /// </summary>
        /// <remarks>
        /// Changing the value will automatically compare it with the current value using
        /// <see cref="IEquatable{T}"/> or the default equality comparer. If the value is different,
        /// it updates the old value, assigns the new value, and notifies observers through the event.
        /// </remarks>
        /// <value>
        /// The current value of the property.
        /// </value>
        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    OldValue = _value;
                    _value = value;
                    OnValueChanged?.Invoke();
                }
            }
        }

        public ObservableProperty(T initialValue = default)
        {
            OldValue = default;
            _value = initialValue;
        }
    }

    public interface IObservable
    {
        event Action OnValueChanged;
    }
}