using System.Runtime.InteropServices;

namespace MagicTooltips.Utilities
{
	internal static class OperatingSystem
	{
		internal static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		internal static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		internal static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

		internal static string GetOSString()
		{
			if (IsWindows())
			{
				return "Windows";
			}
			else if (IsMacOS())
			{
				return "MacOS";
			}
			else if (IsLinux())
			{
				return "Linux";
			}
			else
			{
				return "Unknown";
			}
		}
	}
}