﻿using MagicTooltips.Services;
using System;
using System.IO;
using System.Linq;

namespace MagicTooltips.Providers
{
  public class MicrosoftGraphPowerShellProvider : IProvider
  {
    public string ProviderKey => "mg";
    public string DefaultCommands => "";
    public string DefaultNounPrefixes => "mg";
    public string DefaultFgColor => "#32A5E6";
    public string DefaultBgColor => "";
    public string DefaultTemplate => "\uf871 {value}";

    private static string fileHash = null;
    private static string mgAccount = null;
    private static string mgCacheFilePath = null;

    public string GetValue()
    {
      string currentFileHash = null;
      try
      {
        if (string.IsNullOrWhiteSpace(mgCacheFilePath))
        {
          var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".graph");
          mgCacheFilePath = Directory.EnumerateFiles(folderPath, "*cache.bin*").FirstOrDefault();
          LoggingService.LogDebug($"mgCacheFilePath: {mgCacheFilePath}");
        }

        if (!string.IsNullOrEmpty(mgCacheFilePath))
        {
          currentFileHash = Md5Utility.CalculateMd5(mgCacheFilePath);
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
        LoggingService.LogDebug("No mgTokenCache found");
        mgAccount = null;
      }
      if (currentFileHash != fileHash)
      {
        LoggingService.LogDebug("mgTokenCache has changed, clearing cache");
        fileHash = currentFileHash;
        mgAccount = null;
      }

      if (string.IsNullOrWhiteSpace(mgAccount))
      {
        var script = "(Get-MgContext).Account";
        mgAccount = PowershellInvoker.InvokeScript(script);
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
