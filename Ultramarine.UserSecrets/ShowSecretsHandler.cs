using System;
using System.IO;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace Ultramarine.UserSecrets
{
    public class ShowSecretsHandler : CommandHandler
    {
        private const string USER_SECRETS_KEY = "UserSecretsId";

        protected override void Run()
        {
            var project = IdeApp.ProjectOperations.CurrentSelectedProject;
            if (project == null)
            {
                return;
            }

            if (project.ProjectProperties.HasProperty(USER_SECRETS_KEY))
            {
                var secretId = project.ProjectProperties.GetValue(USER_SECRETS_KEY);

                // TODO: think about another platforms
                var secretsPath = new FilePath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                    .Combine($".microsoft/usersecrets/{secretId}/secrets.json");

                if (!File.Exists(secretsPath))
                {
                    if (!Directory.Exists(secretsPath.ParentDirectory))
                    {
                        Directory.CreateDirectory(secretsPath.ParentDirectory);
                    }
                    File.WriteAllText(secretsPath, "{\n\n}");
                }

                IdeApp.Workbench.OpenDocument(secretsPath, IdeApp.ProjectOperations.CurrentSelectedProject, true);
            }
            else
            {
                MessageService.ShowWarning("User Secrets are not initialized for this project.");

                // TODO: ask to initalize user secrets
            }
        }
    }
}
