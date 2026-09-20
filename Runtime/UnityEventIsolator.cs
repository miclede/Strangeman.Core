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
        private UnityEvent _uEvent;
        public UnityEvent uEvent => _uEvent;

        [SerializeField]
        private int _uEventInvoker;
        public int uEventInvoker => _uEventInvoker;
    }
}
