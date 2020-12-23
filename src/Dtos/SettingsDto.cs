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

        public SettingsDto()
        {
            Providers = new Dictionary<string, ProviderSettingsDto>();
            Providers.Add(ProviderKeys.Kubernetes, new ProviderSettingsDto());
            Providers.Add(ProviderKeys.Azure, new ProviderSettingsDto());
        }
    }

    public class ProviderSettingsDto
    {
        public string Commands { get; set; }
        public string FgColor { get; set; }
        public string BgColor { get; set; }
        public string Template { get; set; }
    }
}
