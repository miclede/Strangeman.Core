#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Strangeman.Utils.Attributes;

namespace Strangeman.Editor
{
    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute conditionalAttribute = (ConditionalAttribute)attribute;
            bool shouldShow = ShouldShowProperty(property, conditionalAttribute);

            if (shouldShow)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute conditionalAttribute = (ConditionalAttribute)attribute;
            bool shouldShow = ShouldShowProperty(property, conditionalAttribute);

            return shouldShow ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        private bool ShouldShowProperty(SerializedProperty property, ConditionalAttribute conditionalAttribute)
        {
            MethodInfo methodInfo = property.serializedObject.targetObject.GetType().GetMethod(conditionalAttribute.ConditionFunctionName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (methodInfo != null && methodInfo.ReturnType == typeof(bool))
            {
                return (bool)methodInfo.Invoke(property.serializedObject.targetObject, null) == conditionalAttribute.Show;
            }

            return true;
        }
    }
}
#endif
