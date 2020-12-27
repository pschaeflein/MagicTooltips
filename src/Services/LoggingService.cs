using System;
using System.IO;
using System.Reflection;

namespace MagicTooltips.Services
{
    public class LoggingService
    {
        private static string logPath = "";
        private static object _lock = new object();

        public static void WriteLog(string message)
        {
            if (!SettingsService.Settings.Debug)
            {
                return;
            }

            if (string.IsNullOrEmpty(logPath))
            {
                var logDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (logDir.EndsWith("lib"))
                {
                    logDir = Directory.GetParent(logDir).ToString();
                }
                logPath = Path.Combine(logDir, "magictooltips.log");
            }

            var formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lock(_lock)
            {
                File.AppendAllText(logPath, $"{formattedDate} {message}\n");
            }
        }
    }
}
