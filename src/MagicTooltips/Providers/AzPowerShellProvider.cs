using MagicTooltips.Services;
using System;
using System.IO;

namespace MagicTooltips.Providers
{
  public class AzPowerShellProvider : IProvider
  {
    public string ProviderKey => "azpwsh";
    public string DefaultCommands => null;
    public string DefaultNounPrefixes => "az";
    public string DefaultFgColor => "#3A96DD";
    public string DefaultBgColor => "";
    public string DefaultTemplate => "\ufd03 {value}";

    private static string fileHash = null;
    private static string azPwshAccount = null;
    private static string azpwshProfilePath = null;

    public string GetValue()
    {
      string currentFileHash = null;
      try
      {
        if (string.IsNullOrWhiteSpace(azpwshProfilePath))
        {
          azpwshProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".azure", "AzureRmContext.json");
        }
        currentFileHash = Md5Utility.CalculateMd5(azpwshProfilePath);
      }
      catch (Exception ex)
      {
        LoggingService.LogDebug(ex.ToString());
      }

      if (currentFileHash != fileHash)
      {
        LoggingService.LogDebug("AzureRmContext.json has changed, clearing cache");
        fileHash = currentFileHash;
        azPwshAccount = null;
      }

      if (string.IsNullOrWhiteSpace(azPwshAccount))
      {
        var script = "((Get-AzContext).Subscription | Get-AzSubscription).Name";
        azPwshAccount = PowershellInvoker.InvokeScript(script);
      }

      return azPwshAccount;
    }
  }
}
