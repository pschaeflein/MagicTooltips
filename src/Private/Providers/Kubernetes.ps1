[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ProviderKubernetes = {
    param($Context)

    $result = [TooltipDto]::new()
    $result.Text = kubectl config current-context
    $result.Text = [char]0xfd31 + " " + $result.Text
    $result.ForegroundColor = "#AE5FD6"
    $result.BackgroundColor = "#000000"
    return $result
}