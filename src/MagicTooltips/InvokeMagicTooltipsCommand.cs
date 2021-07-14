using MagicTooltips.Providers;
using MagicTooltips.Services;
using System;
using System.Collections.Generic;
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
      Line = Line.TrimEnd(' ').ToLowerInvariant();
      LoggingService.LogDebug($"line: '{Line}'");

      if (TriggerService.CommandList.ContainsKey(Line))
      {
        Invoke(TriggerService.CommandList[Line]);
      }
      else
      {
        if (TriggerService.NounPrefixList.Count == 0)
        {
          return;
        }

        var nounsInLine = TriggerService.ParseLine(Line);
        var nounList = string.Join(", ", nounsInLine);
        LoggingService.LogDebug($"nounsInLine: {nounList}");

        foreach (var noun in nounsInLine)
        {
          foreach (var prefix in TriggerService.NounPrefixList)
          {
            if (noun.StartsWith(prefix.Key))
            {
              Invoke(prefix.Value);
            }
          }
        }
        return;
      }
    }

    private void Invoke(List<IProvider> providers)
    {     
      var providerKeys = string.Join(", ", providers.Select(x => x.ProviderKey));

      Task.Run(() =>
      {
        var initialY = Console.CursorTop;
        var horizontalOffset = SettingsService.Settings.HorizontalOffset;

        foreach (var provider in providers)
        {
          LoggingService.LogOperation("Invoke", provider.ProviderKey);
          var val = provider.GetValue();
          horizontalOffset = RenderService.ShowTooltip(provider.ProviderKey, val, Host, initialY, horizontalOffset);
        }
      });
    }
  }
}
