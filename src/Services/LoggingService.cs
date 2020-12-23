﻿using System;
using System.IO;

namespace MagicTooltips.Services
{
    public class LoggingService
    {
        public static void WriteLog(string message)
        {
            // todo, opt in via config

            var path = "magictooltips.log";
            var formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            File.AppendAllText(path, $"{formattedDate} {message}\n");
        }
    }
}
