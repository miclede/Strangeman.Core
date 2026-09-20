using Strangeman.Utils.Extensions;
using UnityEngine;

namespace Strangeman.Utils.Bootstrap
{
    [DisallowMultipleComponent]
    public abstract class Bootstrapper<T> : MonoBehaviour, IInitializable where T : Component
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
        
        public void Initialize()
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
