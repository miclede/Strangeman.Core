using Strangeman.Utils.Extensions;
using UnityEngine;

namespace Strangeman.Utils
{
    /// <summary>
    /// Abstract base class for bootstrapping a specific type of Component in Unity.
    /// Provides thread-safe mechanisms to ensure the Bootstrap process is executed exactly once.
    /// </summary>
    /// <typeparam name="T">The type of Component to Bootstrap</typeparam>
    [DisallowMultipleComponent]
    public abstract class Bootstrapper<T> :  MonoBehaviour, IInitializable where T : Component
    {
        T _component;
        bool _bootstrapped;
        readonly object _lock = new();

        internal T Component
        {
            get
            {
                _component.Get(gameObject, out _component);
                return _component;
            }
        }

        /// <summary>
        /// Ensures that the Bootstrap process for the specified type T has been executed once.
        /// If not already bootstrapped, locks the operation to prevent concurrent execution,
        /// logs the start of the bootstrap process, and calls the Bootstrap method.
        /// Handles exceptions during bootstrapping by logging errors and rethrowing exceptions.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe and ensures that the Bootstrap method is called exactly once,
        /// even if accessed concurrently by multiple threads.
        /// </remarks>
        public void InitializeIfNeeded()
        {
            if (_bootstrapped) return;

            lock (_lock)
            {
                if (_bootstrapped) return;

                try
                {
                    Debug.Log($"Bootstrapper: Bootstrapping {typeof(T)} for {gameObject.name}");
                    _bootstrapped = true;
                    Bootstrap();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Bootstrapper: Error during bootstrapping {typeof(T)} on {gameObject.name}: {ex}");
                    throw;
                }
            }
        }

        protected abstract void Bootstrap();
    }
}
