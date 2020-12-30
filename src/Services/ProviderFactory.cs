using MagicTooltips.Dtos;
using MagicTooltips.Providers;
using System;
using System.Collections.Generic;

namespace MagicTooltips.Services
{
    class ProviderFactory
    {
        private static Dictionary<string, IProvider> AllProviders = null;

        public static IProvider GetProvider(string providerKey)
        {

            if (AllProviders.ContainsKey(providerKey))
            {
                return AllProviders[providerKey];
            }

            throw new NotImplementedException($"Unexpected providerKey: `{providerKey}`");
        }

        public static Dictionary<string, IProvider> GetAllProviders()
        {
            if (AllProviders == null)
            {
                AllProviders = new Dictionary<string, IProvider>
                {
                    { ProviderKeys.Kubernetes, new KubernetesProvider() },
                    { ProviderKeys.Azure, new AzureProvider() },
                    { ProviderKeys.Aws, new AwsProvider() },
                };
            }
            return AllProviders;
        }
    }
}
