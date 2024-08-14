using UnityEngine;
using UnityEngine.Events;

namespace Strangeman.Utils
{
    /// <summary>
    /// Encapsulates a UnityEvent and an associated integer invoker.
    /// </summary>
    public struct UnityEventIsolator
    {
        [SerializeField]
        private UnityEvent _uEvent; // The UnityEvent to be isolated.
        public UnityEvent uEvent => _uEvent; // Provides access to the UnityEvent.

        [SerializeField]
        private int _uEventInvoker; // An integer associated with the UnityEvent.
        public int uEventInvoker => _uEventInvoker; // Provides access to the integer invoker.
    }
}
