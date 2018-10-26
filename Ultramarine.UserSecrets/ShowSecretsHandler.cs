using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects.Text;

namespace Ultramarine.UserSecrets
{
    public class ShowSecretsHandler : CommandHandler
    {
        protected override void Run()
        {
            var filePath = IdeApp.ProjectOperations.CurrentSelectedProject?.FileName;
            if (filePath.HasValue)
            {
                var file = TextFile.ReadFile(filePath.Value);
                var xdoc = XDocument.Parse(TextFile.ReadFile(filePath.Value)?.Text);
                var secretIds = xdoc.Descendants("PropertyGroup").Elements("UserSecretsId").Select(i => (string)i).ToList();

                if (secretIds.Count > 0)
                {
                    // TODO: think about another platforms
                    var secretsPath = new FilePath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                        .Combine($".microsoft/usersecrets/{secretIds[0]}/secrets.json");

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

        protected override void Update(CommandInfo info)
        {
            // TODO: check that selected project is c# project
            info.Enabled = IdeApp.ProjectOperations.CurrentSelectedProject?.FileName != null;
        }
    }
}
