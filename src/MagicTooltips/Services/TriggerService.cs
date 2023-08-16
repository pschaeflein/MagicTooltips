using MagicTooltips.Providers;
using System.Collections.Generic;

namespace MagicTooltips.Services
{
  public class TriggerService
  {
    internal static readonly Dictionary<string, List<IProvider>> CommandList = new();
    internal static readonly Dictionary<string, List<IProvider>> NounPrefixList = new();

    public static void PopulateTriggers()
    {
      foreach (var provider in SettingsService.Settings.Providers)
      {
        AddTriggers(provider.Key, provider.Value.Commands);
        AddTriggers(provider.Key, provider.Value.NounPrefixes, true);
      }
    }

    private static void AddTriggers(string provider, string triggersCsv, bool isNounPrefix = false)
    {
      if (string.IsNullOrEmpty(triggersCsv))
      {
        return;
      }

      LoggingService.LogDebug($"AddTriggers {provider}");
      var triggers = triggersCsv.ToLowerInvariant().Split(',');
      foreach (var trigger in triggers)
      {
        AddTrigger(trigger, provider, isNounPrefix ? NounPrefixList : CommandList);
      }
    }

    private static void AddTrigger(string trigger, string providerKey, Dictionary<string, List<IProvider>> triggerList)
    {
      LoggingService.LogDebug($"  {trigger} -> {providerKey}");
      if (!triggerList.ContainsKey(trigger))
      {
        triggerList.Add(trigger, new List<IProvider>());
      }
      var provider = ProviderFactory.GetProvider(providerKey);
      triggerList[trigger].Add(provider);
    }

    internal static IEnumerable<string> ParseLine(string line)
    {
      List<string> results = new();

      var workingSet = line;

      var dashPos = line.IndexOf('-');
      while (dashPos > -1)
      {
        // if dash is first character, move on...
        if (dashPos == 0)
        {
          workingSet = workingSet.Substring(dashPos + 1);
        }
        else
        {
          // the character before the dash cannot be blank
          var lastVerbChar = workingSet.Substring(dashPos - 1, 1);
          if (lastVerbChar == " ")
          {
            // toss this dash and continue
            workingSet = workingSet.Substring(dashPos + 1);
          }
          else
          {
            workingSet = workingSet.Substring(dashPos + 1);
            var spacePos = workingSet.IndexOf(" ");
            string noun;
            if (spacePos > 0)
            {
              noun = workingSet.Substring(0, spacePos);

              if (spacePos == workingSet.Length)
              {
                workingSet = workingSet.Substring(spacePos);
              }
              else
              {
                workingSet = workingSet.Substring(spacePos + 1);
              }
            }
            else
            {
              noun = workingSet;
            }
            results.Add(noun.ToLowerInvariant());
          }

        }

        dashPos = workingSet.IndexOf('-');
      };

      return results;

    }
  }
}
