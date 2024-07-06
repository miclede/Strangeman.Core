using System;
using UnityEngine;

namespace Strangeman.Utils.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Checks if the component is null.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The component instance to check.</param>
        /// <returns>True if the component is null; otherwise, false.</returns>
        public static bool IsNull<T>(this T component) where T : Component
        {
            return component is null;
        }

        /// <summary>
        /// Extension method that returns the given component or null if the component is null.
        /// In Unity this allows for null propagation ie. Component.OrNull?.variable ?? default
        /// </summary>
        /// <typeparam name="T">The type of the component, must be a subtype of UnityEngine.Component.</typeparam>
        /// <param name="component">The component to check.</param>
        /// <returns>The component if it is not null; otherwise, null.</returns>
        public static T OrNull<T> (this T component) where T : Component
        {
            return component ? component : null;
        }

        /// <summary>
        /// Retrieves a component of type <typeparamref name="T"/> from the specified <paramref name="targetObject"/>.
        /// If the specified <paramref name="component"/> is not null, it returns the <paramref name="component"/>.
        /// If the <paramref name="component"/> is null, it attempts to retrieve a component of type <typeparamref name="T"/> from the <paramref name="targetObject"/>.
        /// If no component of type <typeparamref name="T"/> is found on the <paramref name="targetObject"/>, it returns null.
        /// In Unity this allows for null propagation ie. Component.OrNull?.variable ?? default
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <param name="component">The initial component to check.</param>
        /// <param name="targetObject">The GameObject from which to retrieve the component if the initial component is null.</param>
        /// <returns>The retrieved component of type <typeparamref name="T"/>, or null if no component of type <typeparamref name="T"/> is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetObject"/> is null.</exception>
        public static T GetOrNull<T> (this T component, GameObject targetObject) where T : Component
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
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The component instance to check and initialize.</param>
        /// <param name="targetObject">The Component instance whose GameObject is used to find the component.</param>
        /// <param name="result">The initialized or retrieved component.</param>
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
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The component instance to check and initialize.</param>
        /// <param name="childObject">A child component used to search for the component in its parent hierarchy.</param>
        /// <param name="result">The initialized or retrieved component.</param>
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
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The component instance to check and initialize.</param>
        /// <param name="targetObject">The Component instance whose GameObject is used to find or add the component.</param>
        /// <param name="addedComponent">The initialized or retrieved component of type T.</param>
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
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="gameObject">The GameObject to which the component is attached.</param>
        /// <returns>True if the component exists on the GameObject or its parents; otherwise, false.</returns>
        public static bool HasComponentInParent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() != null || gameObject.GetComponentInParent<T>() != null;
    }
    }
}