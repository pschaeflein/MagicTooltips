using MagicTooltips.Services;
using System;
using System.Management.Automation;
using System.Threading.Tasks;

namespace MagicTooltips
{
    [Cmdlet(VerbsLifecycle.Invoke, "MagicTooltips")]
    public class InvokeMagicTooltipsCommand : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Line { get; set; }

        protected override void ProcessRecord()
        {
            Line = Line.TrimEnd(' ');
            LoggingService.WriteLog($"line: '{Line}'");

            if (!CommandService.CommandList.ContainsKey(Line))
            {
                return;
            }

            var providerKey = CommandService.CommandList[Line];
            LoggingService.WriteLog($"providerKey: {providerKey}");

            Task.Run(() =>
            {
                var initialY = Console.CursorTop;
                var provider = ProviderFactory.GetProvider(providerKey);
                var val = provider.GetValue();
                RenderService.ShowTooltip(providerKey, val, Host, initialY);
            });
        }
    }
}
