using Strangeman.Utils.Attributes;
using Strangeman.Utils.Extensions;
using System;
using UnityEngine;

namespace Strangeman.Utils.Service
{
    /// <summary>
    /// Enum representing the configuration options for the ServiceLocator.
    /// </summary>
    public enum LocatorConfiguration { Local, Global }

    /// <summary>
    /// A MonoBehaviour-based service locator that provides a mechanism to register and retrieve services.
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        [ClearOnLoad]
        static ServiceLocator _global;

        readonly ServiceManager _services = new ServiceManager();

        internal const string k_globalServiceLocatorName = "ServiceLocator [Global]";

        /// <summary>
        /// Configures the ServiceLocator as a global instance, optionally preventing it from being destroyed on load.
        /// </summary>
        /// <param name="dontDestroyOnLoad">If true, the ServiceLocator will not be destroyed on load.</param>
        internal void ConfigureGlobal(bool dontDestroyOnLoad)
        {
            if (_global == this)
            {
                Debug.LogWarning("ServiceLocator.ConfigureGlobal: Already configured for Global. ", this);
                return;
            }

            if (_global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureGlobal: There is already a global ServiceLocator.", this);
            }

            _global = this;

            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Retrieves the appropriate ServiceLocator based on the specified configuration.
        /// </summary>
        /// <param name="component">The component to search from.</param>
        /// <param name="config">The configuration specifying whether to use a global or local locator.</param>
        /// <returns>The located ServiceLocator instance, or null if not found.</returns>
        public static ServiceLocator For(Component component, LocatorConfiguration config = LocatorConfiguration.Local)
        {
            ServiceLocator toFind = null;

            try
            {
                toFind = config == LocatorConfiguration.Global ? Global : toFind.GetInParent(component.gameObject, out toFind) ? toFind : null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"ServiceLocator.For: Error locating ServiceLocator: {ex}");
            }

            return toFind;
        }

        /// <summary>
        /// Retrieves the global ServiceLocator instance, creating one if it doesn't exist.
        /// </summary>
        public static ServiceLocator Global
        {
            get
            {
                if (_global != null) return _global;
                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.InitializeIfNeeded();
                    return _global;
                }

                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().InitializeIfNeeded();

                return _global;
            }
        }

        /// <summary>
        /// Registers a service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to register.</typeparam>
        /// <param name="service">The instance of the service to register.</param>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        public ServiceLocator Register<T>(T service)
        {
            _services.Register(service);
            return this;
        }

        /// <summary>
        /// Retrieves a registered service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve.</typeparam>
        /// <param name="service">The output parameter to hold the retrieved service instance.</param>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        public ServiceLocator GetAny<T>(out T service) where T : class
        {
            try
            {
                service = _services.Get<T>();
                return this;
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"ServiceLocator.Get: {ex.Message}");
                service = null;
                return this;
            }
        }

        /// <summary>
        /// Retrieves a registered MonoBehaviour-based service of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to retrieve, inheriting from <see cref="GlobalMonoService{T}"/>.</typeparam>
        /// <param name="service">The output parameter to hold the retrieved service instance.</param>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if the service cannot be located or created.</exception>
        public ServiceLocator GetMonoService<T>(out T service) where T : GlobalMonoService<T>
        {
            try
            {
                service = _services.Get<T>();
                return this;
            }
            catch (ArgumentException)
            {
                GlobalMonoService<T>.LocateAndRegister();
                service = _services.Get<T>();

                if (service != null)
                {
                    return this;
                }

                else throw new ArgumentException($"ServiceLocator.GetMonoInstance: Failed to locate or create service for {typeof(T).FullName}");
            }
        }

        #region Service Registration event pass-throughs
        public void BindServiceRegistered(Action<Type, object> listener) => _services.ServiceRegistered += listener;
        public void UnbindServiceRegistered(Action<Type, object> listener) => _services.ServiceRegistered -= listener;
        public void BindServiceUnregistered(Action<Type> listener) => _services.ServiceUnregistered += listener;
        public void UnbindServiceUnregistered(Action<Type> listener) => _services.ServiceUnregistered -= listener;
        #endregion

        /// <summary>
        /// Handles the destruction of the ServiceLocator, clearing the global instance if necessary.
        /// </summary>
        private void OnDestroy()
        {
            if (this == _global)
            {
                _global = null;
            }
        }
    }
}
