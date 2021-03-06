using MagicTooltips.Services;
using System;
using System.IO;
using System.Security.Cryptography;

namespace MagicTooltips.Providers
{
    public class AzureProvider : IProvider
    {
        public string ProviderKey => "azure";
        public string DefaultCommands => "az,terraform,pulumi,terragrunt";
        public string DefaultFgColor => "#3A96DD";
        public string DefaultBgColor => "";
        public string DefaultTemplate => "\ufd03 {value}";

        private static string fileHash = null;
        private static string azureAccount = null;
        private static string azProfilePath = null;

        public string GetValue()
        {
            string currentFileHash = null;
            try
            {
                if (string.IsNullOrWhiteSpace(azProfilePath))
                {
                    var azConfigDir = Environment.GetEnvironmentVariable("AZURE_CONFIG_DIR");
                    if (string.IsNullOrWhiteSpace(azConfigDir))
                    {
                        azConfigDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".azure");
                    }
                    azProfilePath = Path.Combine(azConfigDir, "azureProfile.json");
                }
                currentFileHash = CalculateMd5(azProfilePath);
            }
            catch (Exception ex)
            {
                LoggingService.WriteLog(ex.ToString());
            }

            if (currentFileHash != fileHash)
            {
                LoggingService.WriteLog("azureProfile.json has changed, clearing cache");
                fileHash = currentFileHash;
                azureAccount = null;
            }

            if (string.IsNullOrWhiteSpace(azureAccount))
            {
                var script = "az account show --query name --output tsv";

                azureAccount = PowershellInvoker.InvokeScript(script);
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
