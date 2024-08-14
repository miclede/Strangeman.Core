#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Strangeman.Editor
{
    /// <summary>
    /// Provides an editor utility for clearing all player preferences from the Unity Editor.
    /// </summary>
    public static class ClearPlayerPrefs
    {
        /// <summary>
        /// Clears all player preferences and saves the changes. This method is accessible from the Unity Editor's Tools menu.
        /// </summary>
        [MenuItem("Tools/Strangeman/Clear Player Prefs")]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll(); // Remove all player preferences.
            PlayerPrefs.Save(); // Save changes to player preferences.
            Debug.Log("ClearPlayerPrefs.ClearAllPlayerPrefs: All Playerprefs have been cleared.");
        }
    }
}
#endif
