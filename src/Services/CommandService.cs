using System.Collections.Generic;

namespace MagicTooltips.Services
{
    public class CommandService
    {
        public static Dictionary<string, string> CommandList = new Dictionary<string, string>();

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

        private static void AddCommand(string command, string provider)
        {
            LoggingService.WriteLog($"  {command} -> {provider}");
            if (!CommandList.ContainsKey(command))
            {
                CommandList.Add(command, provider);
            }
        }
    }
}
