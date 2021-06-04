using MagicTooltips.Dtos;
using System;
using System.Collections;
using System.Management.Automation;

namespace MagicTooltips.Services
{
  public static class SettingsService
  {
    public static SessionState SessionState { get; private set; }
    public static SettingsDto Settings { get; private set; }

    public static void Populate(SessionState sessionState)
    {
      SessionState = sessionState;
      Settings = new SettingsDto();
      var settingsObj = sessionState.PSVariable.GetValue("MagicTooltipsSettings");
      if (!(settingsObj is Hashtable))
      {
        settingsObj = new Hashtable();
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

      if (!(providerSettingsObj is Hashtable))
      {
        providerSettingsObj = new Hashtable();
      }

      var providerSettingsHash = (Hashtable)providerSettingsObj;

      foreach (var provider in Settings.Providers)
      {
        PopulateProviderSettings(provider.Key, providerSettingsHash);
      }
    }

    private static void PopulateProviderSettings(string providerKey, Hashtable providerSettingsHash)
    {
      var provider = ProviderFactory.GetProvider(providerKey);
      var providerSettings = (Hashtable)providerSettingsHash[providerKey];
      Settings.Providers[providerKey].Commands = GetSetting(providerSettings, "Commands", provider.DefaultCommands);
      Settings.Providers[providerKey].FgColor = GetSetting(providerSettings, "FgColor", provider.DefaultFgColor);
      Settings.Providers[providerKey].BgColor = GetSetting(providerSettings, "BgColor", provider.DefaultBgColor);
      Settings.Providers[providerKey].Template = GetSetting(providerSettings, "Template", provider.DefaultTemplate);
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
        throw new NotImplementedException($"Something went wrong parsing the value: '{value}' as '{targetType}'");
      }
    }
  }
}
