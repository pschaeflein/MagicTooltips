﻿using MagicTooltips.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MagicTooltips.Providers
{
	public class M365Provider: IProvider
	{
    public string ProviderKey => "m365";
    public string DefaultCommands => "m365";
    public string DefaultFgColor => "#EF5350";
    public string DefaultBgColor => "";
    public string DefaultTemplate => "\uf8c5 {value}";

    private static string fileHash = null;
    private static string m365Account = null;
    private static string m365ConnectionInfoFilePath = null;

    public string GetValue()
    {
      string currentFileHash = null;
      try
      {
        if (string.IsNullOrWhiteSpace(m365ConnectionInfoFilePath))
        {
          m365ConnectionInfoFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cli-m365-tokens.json");
        }
        currentFileHash = CalculateMd5(m365ConnectionInfoFilePath);
      }
      catch (Exception ex)
      {
        LoggingService.WriteLog(ex.ToString());
      }

      if (currentFileHash != fileHash)
      {
        LoggingService.WriteLog(".cli-m365-tokens.json has changed, clearing cache");
        fileHash = currentFileHash;
        m365Account = null;
      }

      if (string.IsNullOrWhiteSpace(m365Account))
      {
        var script = "m365 status --query connectedAs -o json";

        m365Account = PowershellInvoker.InvokeScript(script);
      }

      return m365Account;
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
