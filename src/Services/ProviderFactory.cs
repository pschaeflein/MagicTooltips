using MagicTooltips.Providers;
using System;

namespace MagicTooltips.Services
{
    class ProviderFactory
    {
        public static IProvider GetProvider(string providerKey)
        {
            switch (providerKey)
            {
                case "kubernetes":
                    return new KubernetesProvider();
                case "azure":
                    return new AzureProvider();
                case "aws":
                    return new AwsProvider();
                default:
                    throw new NotImplementedException($"Unexpected providerKey: `{providerKey}`");
            }
        }
    }
}
