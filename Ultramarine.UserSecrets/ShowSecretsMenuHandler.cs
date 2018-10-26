using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace Ultramarine.UserSecrets
{
    public class ShowSecretsMenuHandler : ShowSecretsHandler
    {
        protected override void Update(CommandInfo info)
        {
            // TODO: check that selected project is c# project
            info.Enabled = IdeApp.ProjectOperations.CurrentSelectedProject?.FileName != null;
        }
    }

}
