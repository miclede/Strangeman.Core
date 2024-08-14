namespace Strangeman.Utils.Builder
{
    /// <summary>
    /// Interface defining the contract for building instances of type B.
    /// Provides methods for registering values, retrieving values, and constructing the final object.
    /// </summary>
    /// <typeparam name="B">The type of object to build.</typeparam>
    public interface IBuild<B>
    {
        /// <summary>
        /// Registers a value for a member with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The type of the member value.</typeparam>
        /// <param name="memberName">The name of the member to register.</param>
        /// <param name="value">The value to associate with the member.</param>
        /// <returns>Returns the current builder instance for method chaining.</returns>
        IBuild<B> With<T>(string memberName, T value);

        /// <summary>
        /// Retrieves the value of a member with the specified name and type.
        /// </summary>
        /// <typeparam name="T">The type of the member value.</typeparam>
        /// <param name="memberName">The name of the member to retrieve.</param>
        /// <returns>The value associated with the member, or the default value if not found.</returns>
        T Get<T>(string memberName);

        /// <summary>
        /// Constructs and returns an instance of type B.
        /// </summary>
        /// <returns>An instance of type B.</returns>
        B Build();
    }
}
