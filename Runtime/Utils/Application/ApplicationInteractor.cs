using UnityEngine;
using System;

namespace Strangeman.Utils.Application
{
    /// <summary>
    /// Manages application lifecycle events, specifically handling the application quit event.
    /// </summary>
    public class ApplicationInteractor : ScriptableObject
    {
        // Event triggered when the application begins quitting.
        public Action OnBeginApplicationQuit;

        /// <summary>
        /// Initiates the application quit process and triggers the OnBeginApplicationQuit event.
        /// </summary>
        public void StartApplicationQuit()
        {
            OnBeginApplicationQuit?.Invoke(); // Invoke event if there are any subscribers.
            ApplicationQuit(); // Handle the actual application quit.
        }

        /// <summary>
        /// Executes the application quit operation.
        /// </summary>
        private void ApplicationQuit()
        {
            UnityEngine.Application.Quit();
        }

        public const string k_applicationInteractionConfigName = "ApplicationInteractor";

        private static ApplicationInteractor _asset;

        /// <summary>
        /// Loads the ApplicationInteractor asset from the Resources folder.
        /// Throws an exception if the asset is not found.
        /// </summary>
        public static ApplicationInteractor Asset
        {
            get
            {
                if (_asset is null)
                {
                    var assetAtPath = Resources.Load<ApplicationInteractor>(k_applicationInteractionConfigName);
                    _asset = assetAtPath ?? throw new NullReferenceException("ApplicationInteraction.Asset: no asset in Resources folder, please create.");
                }
                return _asset;
            }
        }
    }
}