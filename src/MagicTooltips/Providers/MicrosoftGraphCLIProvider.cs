using MagicTooltips.Services;
using System;
using System.IO;
using System.Text.Json;

namespace MagicTooltips.Providers
{
  public class MicrosoftGraphCLIProvider : IProvider
  {
    public string ProviderKey => "mgcli";
    public string DefaultCommands => "mg";  // macOS will get a new name...
    public string DefaultNounPrefixes => "";
    public string DefaultFgColor => "#32A5E6";
    public string DefaultBgColor => "";
    public string DefaultTemplate => "\uf871 {value}";

    private static string fileHash = null;
    private static string mgAccount = null;
    private static string mgRecordFilePath = null;

    public string GetValue()
    {
      string currentFileHash = null;
      try
      {
        if (string.IsNullOrWhiteSpace(mgRecordFilePath))
        {
          var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".mg");
          mgRecordFilePath = Path.Combine(folderPath, "record.txt");
          LoggingService.LogDebug($"mgRecordFilePath: {mgRecordFilePath}");
        }

        if (!string.IsNullOrEmpty(mgRecordFilePath))
        {
          currentFileHash = Md5Utility.CalculateMd5(mgRecordFilePath) ?? "null";
        }
        else
        {
          currentFileHash = "null";
        }
      }
      catch (Exception ex)
      {
        LoggingService.LogDebug(ex.ToString());
      }

      if (currentFileHash == "null")
      {
        LoggingService.LogDebug("No mgRecordFile found");
        mgAccount = null;
      }
      if (currentFileHash != fileHash)
      {
        LoggingService.LogDebug("mgRecordFile has changed, clearing cache");
        fileHash = currentFileHash;
        mgAccount = null;
      }

      /*
       *  For now, there is no command to get the current user
       * 
       *  So I'm going to read & parse the record.txt file
       */
      if (string.IsNullOrWhiteSpace(mgAccount))
      {
        //var script = "(Get-MgContext).Account";
        //mgAccount = PowershellInvoker.InvokeScript(script);

        if (File.Exists(mgRecordFilePath))
        {
          var content = File.ReadAllText(mgRecordFilePath);
          using var contentDoc = JsonDocument.Parse(content);
          mgAccount = contentDoc.RootElement.GetProperty("username").GetString();
        }
      }

      if (string.IsNullOrEmpty(mgAccount))
      {
        return "Not connected";
      }
      else
      {
        return mgAccount.Trim('"');
      }
    }

  }
}
