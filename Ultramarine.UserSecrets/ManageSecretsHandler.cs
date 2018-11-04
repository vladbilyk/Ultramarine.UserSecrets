using System;
using System.IO;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace Ultramarine.UserSecrets
{
    public class ManageSecretsHandler : CommandHandler
    {
        private const string USER_SECRETS_KEY = "UserSecretsId";

        protected override async void Run()
        {
            var project = IdeApp.ProjectOperations.CurrentSelectedProject;
            if (project == null)
            {
                return;
            }

            if (!project.ProjectProperties.HasProperty(USER_SECRETS_KEY))
            {
                var answer = MessageService.AskQuestion("User Secrets are not initialized", 
                                                        $"Do you want to initialize user secrets for the current project {project.Name}?", 
                                                        0, 
                                                        new[] { AlertButton.No, AlertButton.Yes });
                if (answer == AlertButton.No)
                {
                    return;
                }

                var newSecretId = $"{project.Name}-{Guid.NewGuid().ToString()}";
                project.ProjectProperties.SetValue(USER_SECRETS_KEY, newSecretId);
                await project.SaveAsync(new ProgressMonitor());
            }

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

            await IdeApp.Workbench.OpenDocument(secretsPath, IdeApp.ProjectOperations.CurrentSelectedProject, true);
        }
    }
}
