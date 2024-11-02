using UnityEngine;

namespace Strangeman.Utils
{
    /// <summary>
    /// Encapsulates a reference to a Unity scene, storing both the scene asset and its name.
    /// </summary>
    [System.Serializable]
    public class SceneField
    {
        // Reference to the scene asset, used only in the Unity Editor.
        [SerializeField] private Object _sceneAsset;

        // Name of the scene, automatically populated from the scene asset.
        [SerializeField] private string _sceneName = "";

        // Provides read-only access to the scene's name.
        public string SceneName => _sceneName;

        // Implicit conversion to string, returning the scene's name.
        public static implicit operator string(SceneField sceneField) => sceneField._sceneName;
    }
}