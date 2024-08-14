using System;

namespace Strangeman.Utils.Attributes
{
    /// <summary>
    /// Attribute that marks a field to be cleared to its default value upon subsystem registration in Unity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ClearOnLoadAttribute : Attribute
    {
        // Optional message associated with the attribute.
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearOnLoadAttribute"/> class.
        /// </summary>
        /// <param name="message">An optional message to associate with the attribute.</param>
        public ClearOnLoadAttribute(string message = "")
        {
            Message = message;
        }
    }
}
