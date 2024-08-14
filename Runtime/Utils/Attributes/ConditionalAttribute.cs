using UnityEngine;
using System;

namespace Strangeman.Utils.Attributes
{
    /// <summary>
    /// Attribute that conditionally shows or hides fields in the Unity Inspector based on a boolean condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public bool Show = false;
        public string ConditionFunctionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalAttribute"/> class.
        /// </summary>
        /// <param name="show">Whether the field should be shown (true) or hidden (false).</param>
        /// <param name="conditionFunctionName">Name of the method that returns the visibility condition.</param>
        public ConditionalAttribute(bool show, string conditionFunctionName)
        {
            Show = show;
            ConditionFunctionName = conditionFunctionName;
        }
    }
}
