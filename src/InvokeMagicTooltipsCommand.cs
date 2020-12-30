using MagicTooltips.Services;
using System;
using System.Linq;
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

            var providers = CommandService.CommandList[Line];
            var providerKeys = string.Join(", ", providers.Select(x => x.ProviderKey));
            LoggingService.WriteLog($"providerKey: {providerKeys}");

            Task.Run(() =>
            {
                var initialY = Console.CursorTop;
                var horizontalOffset = SettingsService.Settings.HorizontalOffset;

                foreach (var provider in providers)
                {
                    var val = provider.GetValue();
                    horizontalOffset = RenderService.ShowTooltip(provider.ProviderKey, val, Host, initialY, horizontalOffset);
                }
            });
        }
    }
}
