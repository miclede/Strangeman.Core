using System;
using UnityEngine;

namespace Strangeman.Utils.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Checks if the component is null.
        /// </summary>
        /// <returns>True if the component is null; otherwise, false.</returns>
        public static bool IsNull<T>(this T component) where T : Component
        {
            return component is null;
        }

        /// <summary>
        /// Returns the component if it is not null; otherwise, returns null.
        /// This allows for null propagation in Unity.
        /// </summary>
        /// <returns>The component if it is not null; otherwise, null.</returns>
        public static T OrNull<T>(this T component) where T : Component
        {
            return component ? component : null;
        }

        /// <summary>
        /// Retrieves a component of type <typeparamref name="T"/> from the specified <paramref name="targetObject"/>.
        /// If the <paramref name="component"/> is not null, it returns the <paramref name="component"/>. Otherwise, it attempts to retrieve the component from the <paramref name="targetObject"/>.
        /// </summary>
        /// <returns>The retrieved component or null if no component is found.</returns>
        public static T GetOrNull<T>(this T component, GameObject targetObject) where T : Component
        {
            if (targetObject is null)
            {
                throw new ArgumentNullException(nameof(targetObject), "Target object cannot be null.");
            }

            var result = component ? component : targetObject.GetComponent<T>();
            return result ?? null;
        }

        /// <summary>
        /// Checks if the component is null and initializes it using GetComponent<T>() if it is null.
        /// </summary>
        /// <returns>True if the component was successfully retrieved or initialized; otherwise, false.</returns>
        public static bool Get<T>(this T component, GameObject targetObject, out T result) where T : Component
        {
            result = component ? component : targetObject.GetComponent<T>();

            if (result is null)
            {
                Debug.LogError($"Component.GetOrReturn: Component of type {typeof(T)} not found in parent hierarchy of {targetObject.gameObject.name}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves or initializes a component of type T by checking if it is null and using GetComponentInParent<T>() if needed.
        /// </summary>
        /// <returns>True if the component was successfully retrieved or initialized; otherwise, false.</returns>
        public static bool GetInParent<T>(this T component, GameObject childObject, out T result) where T : Component
        {
            result = component ? component : childObject.GetComponentInParent<T>();

            if (result is null)
            {
                Debug.LogError($"Component.GetOrReturnInParent: Component of type {typeof(T)} not found in parent hierarchy of {childObject.gameObject.name}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or adds a component of type T to the GameObject associated with the target Component.
        /// </summary>
        /// <returns>True if the component was retrieved or added successfully; false if failed to retrieve or add.</returns>
        public static bool GetOrAdd<T>(this T component, GameObject targetObject, out T addedComponent) where T : Component
        {
            addedComponent = component ?? targetObject.GetComponent<T>();

            if (addedComponent is null)
            {
                addedComponent = targetObject.gameObject.AddComponent<T>();

                if (addedComponent is null)
                {
                    Debug.LogError($"Component.GetOrAdd: Failed to add component of type {typeof(T)} to {targetObject.gameObject.name}");
                    return false;
                }
                return true;
            }

            return true;
        }

        /// <summary>
        /// Checks if the component exists on the GameObject or its parents.
        /// </summary>
        /// <returns>True if the component exists on the GameObject or its parents; otherwise, false.</returns>
        public static bool HasComponentInParent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null || gameObject.GetComponentInParent<T>() != null;
        }
    }
}
