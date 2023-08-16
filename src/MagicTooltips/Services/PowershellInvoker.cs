using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MagicTooltips.Services
{
  public static class PowershellInvoker
  {
    public static string InvokeScript(string script)
    {
      var runspace = RunspaceFactory.CreateRunspace();
      var powershell = PowerShell.Create();
      powershell.Runspace = runspace;
      runspace.Open();
      powershell.AddScript(script);

      try
      {
        var results = powershell.Invoke();

        if (results.Count > 0)
        {
          return results[0]?.ToString() ?? "";
        }
        else
        {
          return "";
        }
      }
      catch (Exception ex)
      {
        LoggingService.LogDebug($"InvokeScript: {ex}");
        return "";
      }
    }
  }
}
