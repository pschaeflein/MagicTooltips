[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ProviderAzure = {
    param($Context)

    $result = [TooltipDto]::new()
    $result.Text = az account show --query name --output tsv
    $result.Text = [char]0xfd03 + " " + $result.Text
    $result.ForegroundColor = [System.ConsoleColor]::DarkCyan
    $result.BackgroundColor = [System.ConsoleColor]::Black
    return $result
}