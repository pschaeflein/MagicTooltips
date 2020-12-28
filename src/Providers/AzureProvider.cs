using MagicTooltips.Services;
using System;
using System.IO;
using System.Security.Cryptography;

namespace MagicTooltips.Providers
{
    public class AzureProvider : IProvider
    {
        public string DefaultCommands => "az,terraform,pulumi,terragrunt";
        public string DefaultFgColor => "#3A96DD";
        public string DefaultBgColor => "";
        public string DefaultTemplate => "\ufd03 {value}";

        private static string fileHash = null;
        private static string azureAccount = null;

        public string GetValue()
        {
            string currentFileHash = null;
            try
            {
                var azureFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".azure/azureProfile.json");
                currentFileHash = CalculateMd5(azureFilePath);
            }
            catch(Exception ex)
            {
                LoggingService.WriteLog(ex.ToString());
            }

            if (currentFileHash != fileHash)
            {
                // file has changed
                fileHash = currentFileHash;
                azureAccount = null;
            }

            if (string.IsNullOrWhiteSpace(azureAccount))
            {
                var script = "az account show --query name --output tsv";
                azureAccount = SettingsService.SessionState.InvokeCommand.InvokeScript(script)[0].ToString();
            }

            return azureAccount;
        }

        private static string CalculateMd5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash);
                }
            }
        }

    }
}
