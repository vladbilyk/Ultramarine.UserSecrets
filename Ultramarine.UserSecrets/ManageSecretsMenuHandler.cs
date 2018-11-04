using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace Ultramarine.UserSecrets
{
    public class ManageSecretsMenuHandler : ManageSecretsHandler
    {
        protected override void Update(CommandInfo info)
        {
            // TODO: check that selected project is c# project
            info.Enabled = IdeApp.ProjectOperations.CurrentSelectedProject?.FileName != null;
        }
    }

}
