using MagicTooltips.Dtos;
using System;
using System.Collections;
using System.Management.Automation;

namespace MagicTooltips.Services
{
    public static class SettingsService
    {
        public static SettingsDto Settings { get; private set; }

        public static void Populate(SessionState sessionState)
        {
            Settings = new SettingsDto();
            var settingsObj = sessionState.PSVariable.GetValue("MagicTooltipsSettings");
            if (!(settingsObj is Hashtable))
            {
                return;
            }

            var settingsHash = (Hashtable)settingsObj;
            Populate(settingsHash);
        }

        public static void Populate(Hashtable settingsHash)
        {
            Settings = new SettingsDto();
            Settings.Debug = GetSetting(settingsHash, "Debug", false);
            Settings.HorizontalAlignment = GetSetting(settingsHash, "HorizontalAlignment", HorizontalAlignmentEnum.Right);
            Settings.HorizontalOffset = GetSetting(settingsHash, "HorizontalOffset", 0);
            Settings.VerticalOffset = GetSetting(settingsHash, "VerticalOffset", -1);

            var providerSettingsObj = settingsHash["Providers"];

            if (providerSettingsObj == null || !(providerSettingsObj is Hashtable))
            {
                return;
            }

            var providerSettingsHash = (Hashtable)providerSettingsObj;

            var providerSettings = (Hashtable)providerSettingsHash[ProviderKeys.Kubernetes];
            Settings.Providers[ProviderKeys.Kubernetes].Commands = GetSetting(providerSettings, "Commands", "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmlist");
            Settings.Providers[ProviderKeys.Kubernetes].FgColor = GetSetting(providerSettings, "FgColor", "#AE5FD6");
            Settings.Providers[ProviderKeys.Kubernetes].BgColor = GetSetting(providerSettings, "BgColor", "#000000");
            Settings.Providers[ProviderKeys.Kubernetes].Template = GetSetting(providerSettings, "Template", "`u{fd31} {value}");

            providerSettings = (Hashtable)providerSettingsHash[ProviderKeys.Azure];
            Settings.Providers[ProviderKeys.Azure].Commands = GetSetting(providerSettings, "Commands", "az,terraform,pulumi,terragrunt");
            Settings.Providers[ProviderKeys.Azure].FgColor = GetSetting(providerSettings, "FgColor", "#3A96DD");
            Settings.Providers[ProviderKeys.Azure].BgColor = GetSetting(providerSettings, "BgColor", "#000000");
            Settings.Providers[ProviderKeys.Azure].Template = GetSetting(providerSettings, "Template", "`u{fd03} {value}");
        }

        internal static T GetSetting<T>(Hashtable settings, string settingKey, T defaultValue)
        {
            if (settings == null)
            {
                return defaultValue;
            }

            var targetType = typeof(T);
            var settingObj = settings[settingKey];
            if (settingObj == null)
            {
                return defaultValue;
            }

            var typedValue = (T)ConvertValue(settingObj, targetType, defaultValue);

            return typedValue;
        }

        internal static object ConvertValue(object value, Type targetType, object defaultValue)
        {
            try
            {
                if (targetType == typeof(string))
                {
                    return value.ToString();
                }

                if (targetType.IsEnum)
                {
                    if (value == null)
                    {
                        return defaultValue;
                    }

                    return Enum.Parse(targetType, value.ToString(), true);
                }

                if (targetType.IsValueType)
                {
                    return Convert.ChangeType(value, targetType);
                }

                throw new NotImplementedException($"Setting has an invalid type: {targetType}");
            }
            catch
            {
                // todo: write to host?
                throw new NotImplementedException($"Something went wrong parsing the value: '{value}' as '{targetType}'");
                //return defaultValue;
            }
        }
    }
}
