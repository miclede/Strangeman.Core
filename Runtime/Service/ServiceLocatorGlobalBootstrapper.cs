using UnityEngine;

namespace Strangeman.Utils.Service
{
    /// <summary>
    /// Bootstrapper class for configuring the global ServiceLocator in the scene.
    /// Automatically configures the global ServiceLocator and optionally prevents it from being destroyed on scene load.
    /// </summary>
    [AddComponentMenu("ServiceLocator/Service Locator Global")]
    [RequireComponent(typeof(ServiceLocator))]
    public class ServiceLocatorGlobalBootstrapper : Bootstrapper<ServiceLocator>
    {
        [SerializeField] bool _dontDestroyOnLoad = true;

        string _serviceName;

        private void Awake()
        {
            _serviceName = ServiceLocator.k_globalServiceLocatorName;
        }

        private void Start()
        {
            gameObject.name = gameObject.name != _serviceName ? _serviceName : gameObject.name;
        }

        protected override void Bootstrap()
        {
            Component.ConfigureGlobal(_dontDestroyOnLoad);
        }
    }
}
