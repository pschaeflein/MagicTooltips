using MagicTooltips.Services;
using System.Management.Automation;

namespace MagicTooltips
{
  [Cmdlet(VerbsLifecycle.Register, "MagicTooltips")]
  public class RegisterMagicTooltipsCommand : PSCmdlet
  {
    protected override void ProcessRecord()
    {
      SettingsService.Populate(SessionState);

      try
      {
        var AppVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(RegisterMagicTooltipsCommand).Assembly.Location).FileVersion;

        LoggingService.Initialize(AppVersion, SettingsService.Settings.Debug, SettingsService.Settings.DisableTelemetry);
        LoggingService.LogOperation("Register", "");
      }
      catch (System.Exception ex)
      {
        LoggingService.LogError(ex);
        base.WriteError(new ErrorRecord(ex, "ErrorOccurred", ErrorCategory.NotSpecified, null));
      }

      TriggerService.PopulateTriggers();

      PowerShell.Create().AddCommand("Remove-PSReadlineKeyHandler")
             .AddParameter("Key", "SpaceBar")
             .Invoke();

      PowerShell.Create().AddCommand("Set-PSReadlineKeyHandler")
             .AddParameter("Key", "SpaceBar")
             .AddParameter("ScriptBlock", ScriptBlock.Create(@"
                        [Microsoft.PowerShell.PSConsoleReadLine]::Insert(' ')
                        $line = $cursor = $null
                        [Microsoft.PowerShell.PSConsoleReadLine]::GetBufferState([ref]$line, [ref]$cursor)
                        Invoke-MagicTooltips $line
                    "))
             .Invoke();

      PowerShell.Create().AddCommand("Set-PSReadlineKeyHandler")
             .AddParameter("Key", "Tab")
             .AddParameter("ScriptBlock", ScriptBlock.Create(@"
                        $line = $cursor = $null
                        [Microsoft.PowerShell.PSConsoleReadLine]::GetBufferState([ref]$line, [ref]$cursor)
                        Invoke-MagicTooltips $line
                        [Microsoft.PowerShell.PSConsoleReadLine]::TabCompleteNext()
                    "))
             .Invoke();
    }
  }
}
