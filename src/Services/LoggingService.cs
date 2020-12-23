using System;
using System.IO;

namespace MagicTooltips.Services
{
    public class LoggingService
    {
        private static object _lock = new object();

        public static void WriteLog(string message)
        {
            if (!SettingsService.Settings.Debug)
            {
                return;
            }

            var path = "magictooltips.log";
            var formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lock(_lock)
            {
                File.AppendAllText(path, $"{formattedDate} {message}\n");
            }
        }
    }
}
