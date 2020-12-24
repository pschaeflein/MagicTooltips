using System;
using System.Management.Automation.Host;
using System.Text.RegularExpressions;

namespace MagicTooltips.Services
{
    public class ShowTooltipService
    {
        private const char esc = '\u001B';

        public static void ShowTooltip(string providerKey, string value, PSHost host, int initialY)
        {
            var providerConfiguraiton = SettingsService.Settings.Providers[providerKey];
            var template = Regex.Unescape(providerConfiguraiton.Template);
            var tooltipText = template.Replace("{value}", value);
            var fgRgb = ConvertHexToRgb(providerConfiguraiton.FgColor);
            var bgRgb = ConvertHexToRgb(providerConfiguraiton.BgColor);

            var coloredTooltip = $"{esc}[38;2;{fgRgb}m{esc}[48;2;{bgRgb}m{tooltipText}{esc}[0m";


            int drawX;

            switch (SettingsService.Settings.HorizontalAlignment)
            {
                case Dtos.HorizontalAlignmentEnum.Left:
                    drawX = SettingsService.Settings.HorizontalOffset;
                    break;
                case Dtos.HorizontalAlignmentEnum.Right:
                default:
                    drawX = Console.WindowWidth - tooltipText.Length - SettingsService.Settings.HorizontalOffset;
                    break;
            }

            var drawY = initialY + SettingsService.Settings.VerticalOffset;
            drawY = Math.Max(0, drawY);

            var saveX = Console.CursorLeft;
            var saveY = Console.CursorTop;
            Console.SetCursorPosition(drawX, drawY);
            host.UI.Write(coloredTooltip);
            Console.SetCursorPosition(saveX, saveY);
        }

        private static string ConvertHexToRgb(string hex)
        {
            var r = Convert.ToInt32(hex.Substring(1, 2), 16);
            var g = Convert.ToInt32(hex.Substring(3, 2), 16);
            var b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return $"{r};{g};{b}";
        }
    }
}
