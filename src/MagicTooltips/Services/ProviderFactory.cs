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
      AllProviders ??= new Dictionary<string, IProvider>
          {
          { ProviderKeys.Kubernetes, new KubernetesProvider() },
          { ProviderKeys.AzCLI, new AzCLIProvider() },
          { ProviderKeys.AzPwsh, new AzPowerShellProvider() },
          { ProviderKeys.Aws, new AwsProvider() },
          { ProviderKeys.M365, new M365Provider() },
          { ProviderKeys.MicrosoftGraph, new MicrosoftGraphPowerShellProvider() },
          { ProviderKeys.MGCLI, new MicrosoftGraphCLIProvider() }
        };
      return AllProviders;
    }
  }
}
