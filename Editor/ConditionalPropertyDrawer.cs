#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Strangeman.Utils.Attributes;

namespace Strangeman.Editor
{
    /// <summary>
    /// Custom property drawer for ConditionalAttribute, which conditionally displays properties in the Unity Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draws the property field in the Inspector if the condition specified by the ConditionalAttribute is met.
        /// </summary>
        /// <param name="position">The position of the property field in the Inspector.</param>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label to display next to the property field.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute conditionalAttribute = (ConditionalAttribute)attribute;
            bool shouldShow = ShouldShowProperty(property, conditionalAttribute);

            if (shouldShow)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        /// <summary>
        /// Returns the height of the property field in the Inspector based on the condition specified by the ConditionalAttribute.
        /// </summary>
        /// <param name="property">The serialized property to measure.</param>
        /// <param name="label">The label to display next to the property field.</param>
        /// <returns>The height of the property field, or 0 if it should not be displayed.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute conditionalAttribute = (ConditionalAttribute)attribute;
            bool shouldShow = ShouldShowProperty(property, conditionalAttribute);

            return shouldShow ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        /// <summary>
        /// Determines if the property should be displayed based on the result of the specified condition method.
        /// </summary>
        /// <param name="property">The serialized property being evaluated.</param>
        /// <param name="conditionalAttribute">The ConditionalAttribute used to determine the display condition.</param>
        /// <returns>True if the property should be displayed; otherwise, false.</returns>
        private bool ShouldShowProperty(SerializedProperty property, ConditionalAttribute conditionalAttribute)
        {
            MethodInfo methodInfo = property.serializedObject.targetObject.GetType().GetMethod(conditionalAttribute.ConditionFunctionName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (methodInfo != null && methodInfo.ReturnType == typeof(bool))
            {
                return (bool)methodInfo.Invoke(property.serializedObject.targetObject, null) == conditionalAttribute.Show;
            }

            return true; // Default to true if method is not found or invalid.
        }
    }
}
#endif
