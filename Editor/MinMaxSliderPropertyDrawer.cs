#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Strangeman.Utils.Attributes;

namespace Strangeman.Editor
{
    /// <summary>
    /// Custom property drawer for MinMaxSliderAttribute, providing a slider with min and max value fields in the Unity Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderPropertyDrawer : PropertyDrawer
    {
        private const float LabelWidth = 50f; // Width of the min and max value labels.
        private const float FieldSpacing = 5f; // Spacing between the slider and the labels.

        /// <summary>
        /// Draws the min-max slider and associated fields in the Inspector.
        /// </summary>
        /// <param name="position">The position and size of the property field in the Inspector.</param>
        /// <param name="property">The serialized property to draw.</param>
        /// <param name="label">The label to display next to the property field.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute range = (MinMaxSliderAttribute)attribute;

            // Ensure the property is of the correct type.
            if (property.type != nameof(MinMaxSliderValue))
            {
                EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with MinMaxSliderValue.");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Retrieve the min and max value properties.
            SerializedProperty minValueProperty = property.FindPropertyRelative(nameof(MinMaxSliderValue.MinSliderValue));
            SerializedProperty maxValueProperty = property.FindPropertyRelative(nameof(MinMaxSliderValue.MaxSliderValue));

            if (minValueProperty == null || maxValueProperty == null)
            {
                EditorGUI.LabelField(position, label.text, "Could not find MinValue or MaxValue properties.");
                Debug.LogError($"MinMaxSlider: Could not find minValue or maxValue properties on {property.displayName}");
                return;
            }

            float minValue = minValueProperty.floatValue;
            float maxValue = maxValueProperty.floatValue;

            // Define rects for the slider and value fields.
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var minLabelRect = new Rect(position.x, position.y, LabelWidth, position.height);
            var maxLabelRect = new Rect(position.x + position.width - LabelWidth, position.y, LabelWidth, position.height);
            var sliderRect = new Rect(position.x + LabelWidth + FieldSpacing, position.y, position.width - 2 * (LabelWidth + FieldSpacing), position.height);

            // Draw the slider and value fields.
            EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.MinSliderValue, range.MaxSliderValue);
            minValue = EditorGUI.FloatField(minLabelRect, GUIContent.none, minValue);
            maxValue = EditorGUI.FloatField(maxLabelRect, GUIContent.none, maxValue);

            // Clamp values to ensure minValue is not greater than maxValue and within the specified range.
            if (minValue > maxValue) minValue = maxValue;
            minValue = Mathf.Clamp(minValue, range.MinSliderValue, range.MaxSliderValue);
            maxValue = Mathf.Clamp(maxValue, range.MinSliderValue, range.MaxSliderValue);

            // Update the serialized properties with the new values.
            minValueProperty.floatValue = minValue;
            maxValueProperty.floatValue = maxValue;

            EditorGUI.EndProperty();
        }
    }
}
#endif
