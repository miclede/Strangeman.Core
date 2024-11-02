using System.Collections;
using UnityEngine;

namespace Strangeman.Utils.Extensions
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Instantiates a MonoBehaviour and retrieves the instantiated object.
        /// </summary>
        /// <typeparam name="T">The type of MonoBehaviour to instantiate.</typeparam>
        /// <param name="obj">The original MonoBehaviour to instantiate.</param>
        /// <param name="result">The instantiated MonoBehaviour.</param>
        /// <returns>True if the MonoBehaviour was successfully instantiated and retrieved; otherwise, false.</returns>
        public static bool InstantiateMono<T>(this T obj, out T result) where T : MonoBehaviour
        {
            T sobj = default;

            // Check if the original MonoBehaviour is null
            if (obj is null)
            {
                Debug.LogError("InstantiateMono: The original MonoBehaviour is null");
                result = null;
                return false;
            }

            // Instantiate the GameObject associated with the MonoBehaviour
            var iobj = Object.Instantiate(obj.gameObject);

            // Check if the GameObject was successfully instantiated
            if (iobj is null)
            {
                Debug.LogError("InstantiateMono: Failed to instantiate the GameObject.");
                result = null;
                return false;
            }

            // Retrieve the MonoBehaviour from the instantiated GameObject
            if (!sobj.Get(iobj, out result))
            {
                Debug.LogError($"InstantiateMono: The instantiated GameObject does not have component of type {typeof(T)}");
                Object.Destroy(iobj);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Finds a component of type T in the scene.
        /// </summary>
        /// <typeparam name="T">The type of component to find.</typeparam>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <returns>The component of type T if found; otherwise, null.</returns>
        public static T FindComponentInScene<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return Object.FindFirstObjectByType<T>();
        }

        /// <summary>
        /// Executes an action after a specified delay.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <param name="delay">The delay in seconds.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>An IEnumerator that can be used with StartCoroutine.</returns>
        public static IEnumerator ExecuteAfterDelay(this MonoBehaviour monoBehaviour, float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        /// <summary>
        /// Checks if a GameObject is within a certain distance from another GameObject.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <param name="target">The target GameObject to check the distance from.</param>
        /// <param name="distance">The distance threshold.</param>
        /// <returns>True if the GameObject is within the specified distance; otherwise, false.</returns>
        public static bool IsWithinDistance(this MonoBehaviour monoBehaviour, GameObject target, float distance)
        {
            return Vector3.Distance(monoBehaviour.transform.position, target.transform.position) <= distance;
        }

        /// <summary>
        /// Destroys a GameObject after a specified delay.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <param name="gameObject">The GameObject to destroy.</param>
        /// <param name="delay">The delay in seconds.</param>
        public static void DestroyAfterDelay(this MonoBehaviour monoBehaviour, GameObject gameObject, float delay)
        {
            monoBehaviour.StartCoroutine(DestroyAfterDelayCoroutine(gameObject, delay));
        }

        private static IEnumerator DestroyAfterDelayCoroutine(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            Object.Destroy(gameObject);
        }

        /// <summary>
        /// Moves the GameObject towards a target position over time.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <param name="targetPosition">The target position to move towards.</param>
        /// <param name="speed">The speed of the movement.</param>
        public static void MoveTowards(this MonoBehaviour monoBehaviour, Vector3 targetPosition, float speed)
        {
            monoBehaviour.transform.position = Vector3.MoveTowards(monoBehaviour.transform.position, targetPosition, speed * Time.deltaTime);
        }

        /// <summary>
        /// Makes the GameObject face a target position in 2D space.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance.</param>
        /// <param name="targetPosition">The target position to face.</param>
        public static void LookAt2D(this MonoBehaviour monoBehaviour, Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - monoBehaviour.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            monoBehaviour.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
