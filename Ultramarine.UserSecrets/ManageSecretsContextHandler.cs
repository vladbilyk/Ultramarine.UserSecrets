using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace Ultramarine.UserSecrets
{
    public class ManageSecretsContextHandler : ManageSecretsHandler
    {
        protected override void Update(CommandInfo info)
        {
            // TODO: check that selected project is c# project
            info.Enabled = IdeApp.ProjectOperations.CurrentSelectedProject?.FileName != null
                && (IdeApp.ProjectOperations.CurrentSelectedItem as Project) != null;
        }
    }

}
