using Karambolo.Extensions.Logging.File;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MagicTooltips.Services
{
	public class LoggingService
	{
		private static string logPath = "";
		private static object _lock = new object();

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
				config.InstrumentationKey = "bf4fb923-8051-426b-a657-7255b766deb2";
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

			//			if (logger == null)
			//			{
			//				// Create the DI container.
			//				IServiceCollection services = new ServiceCollection();

			//				var logDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			//				if (logDir.EndsWith("lib"))
			//				{
			//					logDir = Directory.GetParent(logDir).ToString();
			//				}

			//				services.AddLogging(loggingBuilder =>
			//				{
			//					loggingBuilder.AddApplicationInsights("bf4fb923-8051-426b-a657-7255b766deb2");
			//					loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);

			//					loggingBuilder.AddFile(o =>
			//					{
			//						o.RootPath = logDir;
			//						o.Files = new LogFileOptions[] {
			//							new LogFileOptions{ Path=Path.Combine(logDir,"magictooltips.log")}
			//						};
			//					});

			//#if DEBUG
			//					debug = true;
			//#endif

			//					if (debug)
			//					{
			//						loggingBuilder.AddFilter<FileLoggerProvider>("", LogLevel.Debug);
			//					}
			//					else
			//					{
			//						loggingBuilder.AddFilter<FileLoggerProvider>("", LogLevel.Error);
			//					}
			//				});

			//				// Build ServiceProvider.
			//				IServiceProvider serviceProvider = services.BuildServiceProvider();

			//				// Obtain logger instance from DI.
			//				var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
			//				logger = loggerFactory.CreateLogger("MagicTooltips");

			//				logger.LogInformation("Initialize completed");
			//			}
		}

		//internal static void Init2()
		//{
		//  TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
		//  telemetryClient = new TelemetryClient(config);
		//  config.InstrumentationKey = "bf4fb923-8051-426b-a657-7255b766deb2";
		//  telemetryClient.Context.Cloud.RoleInstance = "MagicTooltips";
		//  telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

		/*
		 * 

		* 
		 * 
		 */

		//}

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
