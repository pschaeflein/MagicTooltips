using MagicTooltips.Dtos;
using System;
using System.Management.Automation.Host;
using System.Text.RegularExpressions;

namespace MagicTooltips.Services
{
  public class RenderService
  {
    public static int ShowTooltip(string providerKey, string value, PSHost host, int initialY, int horizontalOffset)
    {
      var providerConfiguraiton = SettingsService.Settings.Providers[providerKey];
      var template = Regex.Unescape(providerConfiguraiton.Template);
      var tooltipText = template.Replace("{value}", value);

      var coloredTooltip = GetColoredString(tooltipText, providerConfiguraiton.FgColor, providerConfiguraiton.BgColor);
      var drawX = SettingsService.Settings.HorizontalAlignment switch
      {
        HorizontalAlignmentEnum.Left => horizontalOffset,
        _ => Console.WindowWidth - tooltipText.Length - horizontalOffset,
      };
      var drawY = initialY + SettingsService.Settings.VerticalOffset;
      drawY = Math.Max(0, drawY);

      var saveX = Console.CursorLeft;
      var saveY = Console.CursorTop;
      Console.SetCursorPosition(drawX, drawY);
      host.UI.Write(coloredTooltip);
      Console.SetCursorPosition(saveX, saveY);

      return horizontalOffset + tooltipText.Length + 1;
    }

    internal static string GetColoredString(string text, string fgColor, string bgColor)
    {
      if (string.IsNullOrWhiteSpace(text))
      {
        return "";
      }

      var fgRgb = ConvertHexToRgb(fgColor);
      var bgRgb = ConvertHexToRgb(bgColor);

      if (fgRgb.HasValue && bgRgb.HasValue)
      {
        return $"\x1b[38;2;{fgRgb?.r};{fgRgb?.g};{fgRgb?.b};48;2;{bgRgb?.r};{bgRgb?.g};{bgRgb?.b}m{text}\x1b[0m";
      }
      if (fgRgb.HasValue)
      {
        return $"\x1b[38;2;{fgRgb?.r};{fgRgb?.g};{fgRgb?.b}m{text}\x1b[0m";
      }
      if (bgRgb.HasValue)
      {
        return $"\x1b[48;2;{bgRgb?.r};{bgRgb?.g};{bgRgb?.b}m{text}\x1b[0m";
      }
      return text;
    }

    internal static (int r, int g, int b)? ConvertHexToRgb(string hex)
    {
      if (string.IsNullOrEmpty(hex) || !Regex.IsMatch(hex, "^#[a-fA-F0-9]{6}$"))
      {
        return null;
      }
      var r = Convert.ToInt32(hex.Substring(1, 2), 16);
      var g = Convert.ToInt32(hex.Substring(3, 2), 16);
      var b = Convert.ToInt32(hex.Substring(5, 2), 16);
      return (r, g, b);
    }
  }
}
