using UnityEngine;

namespace Strangeman.Utils
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField] Object _sceneAsset;
        [SerializeField] string _sceneName = "";

        public string SceneName => _sceneName;

        public static implicit operator string(SceneField sceneField) => sceneField._sceneName;
    }
}
