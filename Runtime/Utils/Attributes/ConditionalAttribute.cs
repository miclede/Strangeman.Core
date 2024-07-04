using UnityEngine;
using System;

namespace Strangeman.Utils.Attributes
{
    /// <summary>
    /// Attribute to conditionally show or hide fields in the Unity inspector based on a boolean condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public bool Show = false;
        public string ConditionFunctionName;

        /// <summary>
        /// Initializes a new instance of the ConditionalAttribute with specified show flag and condition function name.
        /// </summary>
        /// <param name="show">Boolean flag indicating whether the field should be shown.</param>
        /// <param name="conditionFunctionName">Name of the function that returns a boolean determining the field's visibility.</param>
        public ConditionalAttribute(bool show, string conditionFunctionName)
        {
            this.Show = show;
            this.ConditionFunctionName = conditionFunctionName;
        }
    }
}