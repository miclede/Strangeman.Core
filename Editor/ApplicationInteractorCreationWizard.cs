using Strangeman.Utils.Application;

namespace Strangeman.Utils.Editor
{
    using UnityEditor;

    public class ApplicationInteractorCreationWizard : CreationWizard<ApplicationInteractor>
    {
        protected override string AssetName => ApplicationInteractor.k_applicationInteractionConfigName;

        // Keeps your original "does the singleton already resolve?" check.
        protected override bool AssetExists()
        {
            try
            {
                return ApplicationInteractor.Asset != null;
            }
            catch
            {
                return false;
            }
        }

        [MenuItem("Tools/Strangeman/Application Interactor Creation Wizard")]
        private static void OpenWizardWindow()
        {
            OpenWizard<ApplicationInteractorCreationWizard>("Application Interaction Wizard");
        }
    }
}