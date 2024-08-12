#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Strangeman.Editor
{
    //https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
    public static class ClearPlayerPrefs
    {
        [MenuItem("Tools/Strangeman/Clear Player Prefs")]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("ClearPlayerPrefs.ClearAllPlayerPrefs: All Playerprefs have been cleared.");
        }
    }
}
#endif