using MagicTooltips.Services;
using System.Management.Automation;

namespace MagicTooltips
{
  [Cmdlet(VerbsLifecycle.Register, "MagicTooltips")]
  public class TestSampleCmdletCommand : PSCmdlet
  {
    protected override void ProcessRecord()
    {
      SettingsService.Populate(SessionState);
      LoggingService.WriteLog("------------------------");
      LoggingService.WriteLog("Initializing");

      CommandService.PopulateCommands();

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
    }
  }
}
