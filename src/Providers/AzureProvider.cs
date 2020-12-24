using MagicTooltips.Services;

namespace MagicTooltips.Providers
{
    public class AzureProvider : IProvider
    {
        public string DefaultCommands => "az,terraform,pulumi,terragrunt";
        public string DefaultFgColor => "#3A96DD";
        public string DefaultBgColor => "#000000";
        public string DefaultTemplate => "`u{fd03} {value}";

        public string GetValue()
        {
            var script = "az account show --query name --output tsv";
            return SettingsService.SessionState.InvokeCommand.InvokeScript(script)[0].ToString();
        }
    }
}
