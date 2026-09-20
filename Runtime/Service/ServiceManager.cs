using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strangeman.Utils.Service
{
    /// <summary>
    /// Manages registration and retrieval of services using a dictionary-based storage.
    /// </summary>
    public class ServiceManager
    {
        readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        readonly object _lock = new();

        public event Action<Type, object> ServiceRegistered;
        public event Action<Type> ServiceUnregistered;

        private void OnServiceRegistered(Type serviceType, object service) => ServiceRegistered?.Invoke(serviceType, service);
        private void OnServiceUnregistered(Type serviceType) => ServiceUnregistered?.Invoke(serviceType);

        /// <summary>
        /// Retrieves a registered service of type T.
        /// </summary>
        /// <returns>The registered service.</returns>
        public T Get<T>() where T : class
        {
            Type serviceType = typeof(T);

            if (_services.TryGetValue(serviceType, out object service))
            {
                return service as T;
            }

            throw new ArgumentException($"ServiceManager.Get: Service of type {serviceType.FullName} not registered");
        }

        /// <summary>
        /// Retrieves a registered service of type T in a thread-safe manner.
        /// </summary>
        /// <returns>The registered service.</returns>
        public T GetThreadSafe<T>() where T : class
        {
            lock (_lock)
            {
                Type serviceType = typeof(T);

                if (_services.TryGetValue(serviceType, out object service))
                {
                    return service as T;
                }

                throw new ArgumentException($"ServiceManager.GetThreadSafe: Service of type {serviceType.FullName} not registered");
            }
        }

        /// <summary>
        /// Registers a service of type T.
        /// </summary>
        /// <returns>The ServiceManager instance for method chaining.</returns>
        public ServiceManager Register<T>(T service)
        {
            Type serviceType = typeof(T);

            if (!_services.TryAdd(serviceType, service))
            {
                Debug.LogWarning($"ServiceManager.Register: Service type of {serviceType.FullName} already registered.");
            }
            else
            {
                OnServiceRegistered(serviceType, service);
            }

            return this;
        }

        /// <summary>
        /// Registers a service of type T in a thread-safe manner.
        /// </summary>
        /// <returns>The ServiceManager instance for method chaining.</returns>
        public ServiceManager RegisterThreadSafe<T>(T service)
        {
            lock (_lock)
            {
                Type serviceType = typeof(T);

                if (!_services.TryAdd(serviceType, service))
                {
                    Debug.LogWarning($"ServiceManager.RegisterThreadSafe: Service type of {serviceType.FullName} already registered.");
                }
                else
                {
                    OnServiceRegistered(serviceType, service);
                }

                return this;
            }
        }

        /// <summary>
        /// Unregisters a service of type T.
        /// </summary>
        /// <returns>The ServiceManager instance for method chaining.</returns>
        public ServiceManager Unregister<T>()
        {
            Type serviceType = typeof(T);

            if (_services.ContainsKey(serviceType))
            {
                _services.Remove(serviceType);
                OnServiceUnregistered(serviceType);
            }

            return this;
        }

        /// <summary>
        /// Checks if a service of type T is registered.
        /// </summary>
        /// <returns>True if a service of type T is registered; otherwise, false.</returns>
        public bool ContainsService<T>()
        {
            Type serviceType = typeof(T);
            return _services.ContainsKey(serviceType);
        }

        public void Clear() => _services.Clear();
        public int Count => _services.Count;
    }

}
