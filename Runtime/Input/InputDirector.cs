using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Strangeman.Utils.Input
{
    public class InputDirector : ScriptableObject
    {
        private static readonly Dictionary<Type, IInputActionCollection2> InputActionMapping = new();

        public static T Get<T>() where T : class, IInputActionCollection2, new()
        {
            if (!InputActionMapping.TryGetValue(typeof(T), out var c))
                InputActionMapping[typeof(T)] = c = new T();
            return c as T;
        }

        [SerializeField] private List<InputMapHandler> inputMapHandlers;

        private bool _isSetup;
        public InputMapHandler ActiveHandler { get; private set; }

        public void SetupInputHandlers()
        {
            if (_isSetup) return;

            foreach (var handler in inputMapHandlers)
            {
                handler.Initialize(this);
            }

            _isSetup = true;
        }

        public void EnableHandler<T>() where T : InputMapHandler
        {
            foreach (var handler in inputMapHandlers)
            {
                handler.Get().Disable();
            }

            var toEnable = inputMapHandlers.OfType<T>().FirstOrDefault();

            if (toEnable is not null)
            {
                toEnable.Get().Enable();
                ActiveHandler = toEnable;
                Debug.Log($"Activating Input Handler: {ActiveHandler}");
            }
            else
                Debug.LogError(
                    "Attempted to enable a type of InputMapHandler that is not present in the HashSet for the InputDirector.");
        }

        public void DisableHandler<T>() where T : InputMapHandler
        {
            var toDisable = inputMapHandlers.OfType<T>().FirstOrDefault();

            if (toDisable is null) return;
            toDisable.Get().Disable();
            if (ActiveHandler == toDisable) ActiveHandler = null;
        }

        public const string k_inputDirectorName = "InputDirector";
        private static InputDirector _asset;

        public static InputDirector Asset
        {
            get
            {
                if (_asset is null)
                {
                    var assetAtPath = Resources.Load<InputDirector>(k_inputDirectorName);
                    _asset = assetAtPath ??
                             throw new NullReferenceException(
                                 "ApplicationInteraction.Asset: no asset in Resources folder, please create.");
                }

                return _asset;
            }
        }
    }
}