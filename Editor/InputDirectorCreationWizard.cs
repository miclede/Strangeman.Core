using Strangeman.Utils.Input;
using UnityEditor;

namespace Strangeman.Utils.Editor
{
    public class InputDirectorCreationWizard : CreationWizard<InputDirector>
    {
        protected override string AssetName => InputDirector.k_inputDirectorName;

        protected override bool AssetExists()
        {
            try { return InputDirector.Asset != null; }
            catch { return false; }
        }

        [MenuItem("Tools/Strangeman/Input Director Creation Wizard")]
        private static void OpenWizardWindow()
        {
            OpenWizard<InputDirectorCreationWizard>("Input Director Wizard");
        }
    }
}
