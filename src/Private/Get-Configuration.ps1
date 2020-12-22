function Get-Configuration() {
    $configuration = @{
        Providers = @{
            "kubernetes" = @{}
            "azure" = @{}
        }
    }

    function Add-Configuration($variableName) {
        $value = Get-Variable "MagicTooltips_$($variableName)" -ValueOnly -ErrorAction SilentlyContinue
        if ($null -ne $value) {
            $configuration.Add($variableName, $value)
        }
    }

    function Add-ProviderConfiguration($provider, $variableName) {
        $value = Get-Variable "MagicTooltips_$($provider)$($variableName)" -ValueOnly -ErrorAction SilentlyContinue
        if ($null -ne $value) {
            $configuration.Providers[$provider].Add($variableName, $value)
        }
    }

    Add-Configuration "Debug"
    Add-Configuration "HorizontalAlignment"
    Add-Configuration "HorizontalOffset"
    Add-Configuration "VerticalOffset"

    Add-ProviderConfiguration "kubernetes" "Commands"
    Add-ProviderConfiguration "kubernetes" "FgColor"
    Add-ProviderConfiguration "kubernetes" "BgColor"

    Add-ProviderConfiguration "azure" "Commands"
    Add-ProviderConfiguration "azure" "FgColor"
    Add-ProviderConfiguration "azure" "BgColor"
    return $configuration
}