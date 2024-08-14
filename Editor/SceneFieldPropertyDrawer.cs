#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Strangeman.Utils;

namespace Strangeman.Editor
{
    /// <summary>
    /// Custom property drawer for SceneField, ensuring scene selection and name synchronization in the Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            // Retrieve the serialized properties.
            SerializedProperty sceneAsset = property.FindPropertyRelative("_sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("_sceneName");

            // Draw the label and the ObjectField for selecting the scene asset.
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

            // Automatically update the scene name when a new scene asset is selected.
            if (sceneAsset.objectReferenceValue != null)
            {
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif