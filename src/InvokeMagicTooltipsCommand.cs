using System;
using System.Management.Automation;

namespace MagicTooltips
{
    [Cmdlet(VerbsLifecycle.Invoke, "MagicTooltips")]
    public class InvokeMagicTooltipsCommand : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var tooltip = "Process MagicTooltips!";
            var saveX = Console.CursorLeft;
            var saveY = Console.CursorTop;
            var drawX = Console.WindowWidth - tooltip.Length;
            var drawY = saveY;
            Console.SetCursorPosition(drawX, drawY);
            Host.UI.Write(tooltip);
            Console.SetCursorPosition(saveX, saveY);
        }
    }
}
