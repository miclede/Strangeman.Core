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
        /// Implementing classes should include their initialization logic within this method.
        /// </summary>
        /// <param name="arg0">Optional parameter for passing additional data during initialization.</param>
        void Initialize(object arg0 = null);
    }
}
