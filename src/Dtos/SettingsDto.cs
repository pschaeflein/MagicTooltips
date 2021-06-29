using MagicTooltips.Services;
using System.Collections.Generic;

namespace MagicTooltips.Dtos
{
  public class SettingsDto
  {
    public bool Debug { get; set; }
    public HorizontalAlignmentEnum HorizontalAlignment { get; set; }
    public int HorizontalOffset { get; set; }
    public int VerticalOffset { get; set; }
    public Dictionary<string, ProviderSettingsDto> Providers { get; set; }

    public bool DisableTelemetry { get; set; }

    public SettingsDto()
    {
      Providers = new Dictionary<string, ProviderSettingsDto>();
      var allProviders = ProviderFactory.GetAllProviders();
      foreach (var provider in allProviders)
      {
        Providers.Add(provider.Key, new ProviderSettingsDto());
      }
    }
  }

  public class ProviderSettingsDto
  {
    public string Commands { get; set; }
    public string NounPrefixes { get; set; }
    public string FgColor { get; set; }
    public string BgColor { get; set; }
    public string Template { get; set; }
  }
}
