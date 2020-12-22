[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ProviderAzure = {
    param($Context)

    $result = [TooltipDto]::new()
    $result.Value = az account show --query name --output tsv
    $result.Template = "`u{fd03} {value}"
    $result.ForegroundColor = "#3A96DD"
    $result.BackgroundColor = "#000000"
    return $result
}