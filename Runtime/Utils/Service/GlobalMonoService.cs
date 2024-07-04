using UnityEngine;

namespace Strangeman.Utils.Service
{
    /// <summary>
    /// Base class for MonoBehaviour-based global services that automatically register with the ServiceLocator on Awake.
    /// </summary>
    /// <typeparam name="T">The type of MonoBehaviour component that represents the service.</typeparam>
    public abstract class GlobalMonoService<T> : MonoBehaviour where T : Component
    {
        protected virtual void Awake()
        {
            ServiceLocator.Global.Register(this);
        }

        /// <summary>
        /// Locates an existing instance of the service in the scene and registers it with the global ServiceLocator,
        /// or creates a new instance if none is found, and registers it.
        /// </summary>
        public static void LocateAndRegister()
        {
            if (FindFirstObjectByType<T>() is { } found)
            {
                ServiceLocator.Global.Register(found);
            }
            else
            {
                Create(out T instance);
                ServiceLocator.Global.Register(instance);
            }
        }

        /// <summary>
        /// Creates a new GameObject instance of the service type and retrieves its component as an instance.
        /// </summary>
        /// <param name="instance">Output parameter to hold the created instance of the service.</param>
        static void Create(out T instance)
        {
            var gameObject = new GameObject(typeof(T).FullName, typeof(T));
            instance = gameObject.GetComponent<T>();
        }
    }
}
