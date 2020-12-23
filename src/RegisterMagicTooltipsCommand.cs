using MagicTooltips.Services;
using System.Management.Automation;

namespace MagicTooltips
{
    [Cmdlet(VerbsLifecycle.Register, "MagicTooltips")]
    public class TestSampleCmdletCommand : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            LoggingService.WriteLog("------------------------");
            LoggingService.WriteLog("Initializing");

            PowerShell.Create().AddCommand("Remove-PSReadlineKeyHandler")
                   .AddParameter("Key", "SpaceBar")
                   .Invoke();

            PowerShell.Create().AddCommand("Set-PSReadlineKeyHandler")
                   .AddParameter("Key", "SpaceBar")
                   .AddParameter("ScriptBlock", ScriptBlock.Create(@"
                        [Microsoft.PowerShell.PSConsoleReadLine]::Insert(' '); 
                        Invoke-MagicTooltips;
                    "))
                   .Invoke();
        }
    }
}
