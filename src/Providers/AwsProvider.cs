using MagicTooltips.Services;
using System;

namespace MagicTooltips.Providers
{
    public class AwsProvider : IProvider
    {
        public string ProviderKey => "aws";
        public string DefaultCommands => "aws,awless,terraform,pulumi,terragrunt";
        public string DefaultFgColor => "#EC7211";
        public string DefaultBgColor => "";
        public string DefaultTemplate => "\uf270 {value}";

        public string GetValue()
        {
            return Environment.GetEnvironmentVariable("AWS_Profile");
        }
    }
}
