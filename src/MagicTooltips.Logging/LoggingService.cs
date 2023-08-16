using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MagicTooltips.Services
{
  public class LoggingService
	{
		private static string logPath = "";
		private static readonly object _lock = new object();

		private static bool settingsDebug;
		private static bool settingsDisabled;

		private static TelemetryClient telemetryClient;
		private static Dictionary<string, string> telemetryProperties;

		public static void Initialize(string appVersion, bool debug = false, bool disableTelemetry = false)
		{
#if DEBUG
			debug = true;
#endif

			settingsDebug = debug;
			settingsDisabled = disableTelemetry;

			if (disableTelemetry)
			{
				return;
			}

			if (debug)
			{
				if (string.IsNullOrEmpty(logPath))
				{
					var logDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					if (logDir.EndsWith("lib"))
					{
						logDir = Directory.GetParent(logDir).ToString();
					}
					logPath = Path.Combine(logDir, "magictooltips.log");
				}
			}

			if (telemetryClient == null)
			{
				TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
        config.ConnectionString = "InstrumentationKey=bf4fb923-8051-426b-a657-7255b766deb2;IngestionEndpoint=https://northcentralus-0.in.applicationinsights.azure.com/;LiveEndpoint=https://northcentralus.livediagnostics.monitor.azure.com/";
        telemetryClient = new TelemetryClient(config);
				telemetryClient.Context.Cloud.RoleInstance = "MagicTooltips";
				telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

				var operatingSystem = Utilities.OperatingSystem.GetOSString();

				telemetryProperties = new Dictionary<string, string>
				{
					{ "Version", appVersion },
					{ "OperatingSystem", operatingSystem}
				};
			}
		}

		public static void LogOperation(string operation, string providerKey)
		{
			if (!settingsDisabled)
			{
				telemetryProperties["operation"] = operation.ToLower();
				telemetryProperties["providerKey"] = providerKey;
				telemetryClient.TrackEvent(operation, telemetryProperties);
				telemetryClient.Flush();
			}
		}

		public static void LogDebug(string message)
		{
			if (settingsDebug)
			{
				var formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				lock (_lock)
				{
					File.AppendAllText(logPath, $"{formattedDate} {message}\n");
				}
			}
		}

		public static void LogError(Exception exception)
		{
			if (!settingsDisabled)
			{
				telemetryProperties.Remove("operation");
				telemetryProperties.Remove("providerKey");
				telemetryClient.TrackException(exception, telemetryProperties);
			}

			if (settingsDebug)
			{
				var formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				lock (_lock)
				{
					File.AppendAllText(logPath, $"{formattedDate} {exception.Message}\n");
				}
			}
		}
	}
}
