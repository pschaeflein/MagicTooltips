[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ProviderKubernetes = {
    param($state)

    $tooltip = kubectl config current-context
    $tooltip = [char]0xfd31 + " " + $tooltip
    $foregroundColor = [System.ConsoleColor]::Magenta
    $backgroundColor = [System.ConsoleColor]::Black
    $result = [TooltipDto]::new()
    $result.Text = $tooltip
    $result.ForegroundColor = $foregroundColor
    $result.BackgroundColor = $backgroundColor
    return $result
}