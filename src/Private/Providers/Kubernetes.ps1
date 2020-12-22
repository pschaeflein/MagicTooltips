[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ProviderKubernetes = {
    param($Context)

    $result = [TooltipDto]::new()
    $result.Value = kubectl config current-context
    $result.Template = "`u{fd31} {value}"
    $result.ForegroundColor = "#AE5FD6"
    $result.BackgroundColor = "#000000"
    return $result
}