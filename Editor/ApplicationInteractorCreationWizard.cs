using Strangeman.Utils.Application;
using UnityEditor;
using UnityEngine;

namespace Strangeman.Editor
{
    /// <summary>
    /// Wizard for creating an ApplicationInteractor asset in a specified project directory.
    /// </summary>
    public class ApplicationInteractorCreationWizard : EditorWindow
    {
        private static string _projectFolderPath = "";

        private void OnGUI()
        {
            // Display folder path selection UI.
            GUILayout.Label("Select Project Directory", EditorStyles.boldLabel);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            _projectFolderPath = EditorGUILayout.TextField("Folder Path", _projectFolderPath);

            if (GUILayout.Button("Browse"))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        _projectFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        DisplayInvalidPathDialog(); // Notify user if the path is invalid.
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.TextArea("This will create an ApplicationInteractor asset in a Resources folder within the targeted Directory.");

            GUILayout.Space(5);

            // Button to initiate asset creation.
            if (GUILayout.Button("Create Asset", GUILayout.Height(position.height * 0.25f)))
            {
                CreateApplicationInteraction();
            }
        }

        /// <summary>
        /// Creates an ApplicationInteractor asset in the selected directory.
        /// </summary>
        private static void CreateApplicationInteraction()
        {
            if (AssetDatabase.IsValidFolder(_projectFolderPath))
            {
                string destinationPath = _projectFolderPath + "/Resources";

                if (!AssetDatabase.IsValidFolder(destinationPath))
                {
                    AssetDatabase.CreateFolder(_projectFolderPath, "Resources"); // Create Resources folder if it doesn't exist.
                }

                ApplicationInteractor asset = CreateInstance<ApplicationInteractor>();
                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(destinationPath + "/" + ApplicationInteractor.k_applicationInteractionConfigName + ".asset");

                try
                {
                    // Check if the asset already exists.
                    var assetExists = ApplicationInteractor.Asset;

                    if (assetExists)
                    {
                        Debug.Log("ApplicationInteraction asset already exists, halting creation.");
                    }
                }
                catch
                {
                    // Create the asset if it doesn't exist.
                    AssetDatabase.CreateAsset(asset, assetPathAndName);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = asset;

                    Debug.Log($"ApplicationInteraction ScriptableObject successfully created at: {assetPathAndName}.");
                }
            }
            else
            {
                DisplayInvalidPathDialog(); // Notify user if the path is invalid.
            }
        }

        /// <summary>
        /// Displays a dialog indicating the path is invalid.
        /// </summary>
        private static void DisplayInvalidPathDialog()
        {
            EditorUtility.DisplayDialog("Invalid Path", "The specified path is not a valid folder within the Assets directory or Assets itself.", "OK");
        }

        /// <summary>
        /// Opens the ApplicationInteractorCreationWizard window from the Tools menu.
        /// </summary>
        [MenuItem("Tools/Strangeman/Application Interactor Creation Wizard")]
        private static void OpenInstructionWindow()
        {
            ApplicationInteractorCreationWizard window = (ApplicationInteractorCreationWizard)GetWindow(typeof(ApplicationInteractorCreationWizard));
            window.titleContent = new GUIContent("Application Interaction Wizard");

            window.minSize = new Vector2(475, 138);
            window.maxSize = new Vector2(476, 139);
            window.ShowUtility();
        }
    }
}