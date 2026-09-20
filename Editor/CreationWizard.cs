#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Strangeman.Utils.Editor
{
/// <summary>
/// Abstract base for editor wizards that create a ScriptableObject asset of type T
/// in a user-selected folder. Derive from this, override what you need, and add a
/// [MenuItem] that calls OpenWizard.
/// </summary>
/// <typeparam name="T">The ScriptableObject type to create.</typeparam>
public abstract class CreationWizard<T> : EditorWindow where T : ScriptableObject
{
    private static readonly Vector2 DefaultWindowSize = new Vector2(475f, 138f);

    // Instance field (not static) so it survives domain reloads and is per-window.
    [SerializeField] private string _projectFolderPath = "Assets";

    /// <summary>File name (without extension) of the created asset. Defaults to the type name.</summary>
    protected virtual string AssetName => typeof(T).Name;

    /// <summary>Subfolder created inside the target directory. Return null or empty to create the asset directly in the target.</summary>
    protected virtual string SubFolderName => "Resources";

    /// <summary>Help text shown in the window.</summary>
    protected virtual string Description =>
        string.IsNullOrEmpty(SubFolderName)
            ? $"This will create a {typeof(T).Name} asset in the targeted directory."
            : $"This will create a {typeof(T).Name} asset in a {SubFolderName} folder within the targeted directory.";

    /// <summary>
    /// Return true to block creation. The default treats T as a singleton and blocks if any asset
    /// of type T exists in the project. Override and return false to allow multiple assets, or
    /// override with a custom check (e.g. a static Asset accessor).
    /// </summary>
    protected virtual bool AssetExists() => AssetDatabase.FindAssets($"t:{typeof(T).Name}").Length > 0;

    /// <summary>Hook called after the asset has been created, saved, and selected.</summary>
    protected virtual void OnAssetCreated(T asset) { }

    private void OnGUI()
    {
        GUILayout.Label("Select Project Directory", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        _projectFolderPath = EditorGUILayout.TextField("Folder Path", _projectFolderPath);

        if (GUILayout.Button("Browse"))
        {
            BrowseForFolder();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.HelpBox(Description, MessageType.Info);
        GUILayout.Space(5);

        if (GUILayout.Button("Create Asset", GUILayout.Height(position.height * 0.25f)))
        {
            CreateAsset();
        }
    }

    private void BrowseForFolder()
    {
        string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");

        if (string.IsNullOrEmpty(selectedPath))
            return;

        if (selectedPath.StartsWith(UnityEngine.Application.dataPath))
        {
            _projectFolderPath = "Assets" + selectedPath.Substring(UnityEngine.Application.dataPath.Length);
            GUI.FocusControl(null); // Drop focus so the text field shows the new value.
        }
        else
        {
            DisplayInvalidPathDialog();
        }
    }

    private void CreateAsset()
    {
        if (!AssetDatabase.IsValidFolder(_projectFolderPath))
        {
            DisplayInvalidPathDialog();
            return;
        }

        if (AssetExists())
        {
            Debug.Log($"{typeof(T).Name} asset already exists, halting creation.");
            return;
        }

        string destinationPath = _projectFolderPath;

        if (!string.IsNullOrEmpty(SubFolderName))
        {
            destinationPath = $"{_projectFolderPath}/{SubFolderName}";

            if (!AssetDatabase.IsValidFolder(destinationPath))
            {
                AssetDatabase.CreateFolder(_projectFolderPath, SubFolderName);
            }
        }

        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{destinationPath}/{AssetName}.asset");

        T asset = CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log($"{typeof(T).Name} ScriptableObject successfully created at: {assetPath}.");

        OnAssetCreated(asset);
    }

    private static void DisplayInvalidPathDialog()
    {
        EditorUtility.DisplayDialog(
            "Invalid Path",
            "The specified path is not a valid folder within the Assets directory or Assets itself.",
            "OK");
    }

    /// <summary>
    /// Opens the wizard as a fixed-size utility window. Call this from a [MenuItem] method
    /// in the derived class (MenuItem can't live on a generic class).
    /// </summary>
    protected static TWizard OpenWizard<TWizard>(string windowTitle, Vector2? size = null)
        where TWizard : CreationWizard<T>
    {
        var window = GetWindow<TWizard>();
        window.titleContent = new GUIContent(windowTitle);

        Vector2 windowSize = size ?? DefaultWindowSize;
        window.minSize = windowSize;
        window.maxSize = windowSize + Vector2.one;
        window.ShowUtility();

        return window;
    }
}
}
#endif
