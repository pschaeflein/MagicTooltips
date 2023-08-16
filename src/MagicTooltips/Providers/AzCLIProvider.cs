using MagicTooltips.Services;
using System;
using System.IO;

namespace MagicTooltips.Providers
{
  public class AzCLIProvider : IProvider
  {
    public string ProviderKey => "azcli";
    public string DefaultCommands => "az,terraform,pulumi,terragrunt";
    public string DefaultNounPrefixes => null;
    public string DefaultFgColor => "#3A96DD";
    public string DefaultBgColor => "";
    public string DefaultTemplate => "\ufd03 {value}";

    private static string fileHash = null;
    private static string azCliAccount = null;
    private static string azProfilePath = null;

    public string GetValue()
    {
      string currentFileHash = null;
      try
      {
        if (string.IsNullOrWhiteSpace(azProfilePath))
        {
          azProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".azure", "azureProfile.json");
        }
        currentFileHash = Md5Utility.CalculateMd5(azProfilePath);
      }
      catch (Exception ex)
      {
        LoggingService.LogDebug(ex.ToString());
      }

      if (currentFileHash != fileHash)
      {
        LoggingService.LogDebug("azureProfile.json has changed, clearing cache");
        fileHash = currentFileHash;
        azCliAccount = null;
      }

      if (string.IsNullOrWhiteSpace(azCliAccount))
      {
        var script = "az account show --query name --output tsv";

        azCliAccount = PowershellInvoker.InvokeScript(script);
      }

      return azCliAccount;
    }
  }
}
