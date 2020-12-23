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

            var command = CommandService.CommandList[Line];
            LoggingService.WriteLog($"command: {command}");

            Task.Run(() =>
            {
                var tooltip = "Process MagicTooltips!";
                var saveX = Console.CursorLeft;
                var saveY = Console.CursorTop;
                var drawX = Console.WindowWidth - tooltip.Length;
                var drawY = saveY;
                Console.SetCursorPosition(drawX, drawY);
                Host.UI.Write(tooltip);
                Console.SetCursorPosition(saveX, saveY);
            });
        }
    }
}
