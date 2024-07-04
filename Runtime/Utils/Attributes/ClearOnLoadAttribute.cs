using System;

namespace Strangeman.Utils.Attributes
{
    /// <summary>
    /// Marks a field to be cleared to its default value on subsystem registration in Unity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ClearOnLoadAttribute : Attribute
    {
        public string Message { get; private set; }

        /// <param name="message">Optional message associated with the attribute.</param>
        public ClearOnLoadAttribute(string message = "")
        {
            Message = message;
        }
    }
}