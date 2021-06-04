using MagicTooltips.Providers;
using System.Collections.Generic;

namespace MagicTooltips.Services
{
  public class CommandService
  {
    public static Dictionary<string, List<IProvider>> CommandList = new Dictionary<string, List<IProvider>>();

    public static void PopulateCommands()
    {
      foreach (var provider in SettingsService.Settings.Providers)
      {
        AddCommands(provider.Key, provider.Value.Commands);
      }
    }

    private static void AddCommands(string provider, string commandsCsv)
    {
      LoggingService.WriteLog($"AddCommands {provider}");
      var commands = commandsCsv.Split(',');
      foreach (var command in commands)
      {
        AddCommand(command, provider);
      }
    }

    private static void AddCommand(string command, string providerKey)
    {
      LoggingService.WriteLog($"  {command} -> {providerKey}");
      if (!CommandList.ContainsKey(command))
      {
        CommandList.Add(command, new List<IProvider>());
      }
      var provider = ProviderFactory.GetProvider(providerKey);
      CommandList[command].Add(provider);
    }
  }
}
