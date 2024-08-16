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
        void Initialize();
    }

    /// <summary>
    /// Generic interface for objects that require initialization with a specific type of data.
    /// </summary>
    /// <typeparam name="T">The type of data required for initialization.</typeparam>
    public interface IInitializeWith<T>
    {
        /// <summary>
        /// Initializes the object with the provided data of type <typeparamref name="T"/>.
        /// Implementing classes should use this method to set up any necessary state or 
        /// configuration using the supplied data.
        /// </summary>
        /// <param name="arg0">
        /// The data of type <typeparamref name="T"/> that is required for initialization. 
        /// This data is typically a dependency or configuration that the object needs to function correctly.
        /// </param>
        void InitializeWith(T arg0);
    }
}
