#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Strangeman.Utils.Attributes;

namespace Strangeman.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        private const float LabelWidth = 50f;
        private const float FieldSpacing = 5f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute range = (MinMaxSliderAttribute)attribute;

            // Ensure the property is of the expected type
            if (property.type != nameof(MinMaxSliderValue))
            {
                EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with MinMaxSliderValue.");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Get the min and max values from the property
            SerializedProperty minValueProperty = property.FindPropertyRelative (nameof(MinMaxSliderValue.MinSliderValue));
            SerializedProperty maxValueProperty = property.FindPropertyRelative(nameof(MinMaxSliderValue.MaxSliderValue));

            if (minValueProperty == null || maxValueProperty == null)
            {
                EditorGUI.LabelField(position, label.text, "Could not find MinValue or MaxValue properties.");
                Debug.LogError($"MinMaxSlider: Could not find minValue or maxValue properties on {property.displayName}");
                return;
            }

            float minValue = minValueProperty.floatValue;
            float maxValue = maxValueProperty.floatValue;

            // Draw the label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate rects
            var minLabelRect = new Rect(position.x, position.y, LabelWidth, position.height);
            var maxLabelRect = new Rect(position.x + position.width - LabelWidth, position.y, LabelWidth, position.height);
            var sliderRect = new Rect(position.x + LabelWidth + FieldSpacing, position.y, position.width - 2 * (LabelWidth + FieldSpacing), position.height);

            // Draw the slider
            EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.MinSliderValue, range.MaxSliderValue);

            // Draw the min and max value fields
            minValue = EditorGUI.FloatField(minLabelRect, GUIContent.none, minValue);
            maxValue = EditorGUI.FloatField(maxLabelRect, GUIContent.none, maxValue);

            // Ensure minValue is not greater than maxValue
            if (minValue > maxValue) minValue = maxValue;

            // Ensure values are within range
            minValue = Mathf.Clamp(minValue, range.MinSliderValue, range.MaxSliderValue);
            maxValue = Mathf.Clamp(maxValue, range.MinSliderValue, range.MaxSliderValue);

            // Set the new values
            minValueProperty.floatValue = minValue;
            maxValueProperty.floatValue = maxValue;

            EditorGUI.EndProperty();
        }
    }
}
#endif