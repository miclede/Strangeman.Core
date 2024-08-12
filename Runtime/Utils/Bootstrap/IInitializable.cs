using UnityEngine;

namespace Strangeman.Utils
{
    /// <summary>
    /// Interface for objects that require bootstrapping functionality.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Ensures that the object is initialized or configured as needed.
        /// Implementing classes should perform any necessary initialization logic inside this method.
        /// </summary>
        void Initialize(object arg0 = null);
    }
}