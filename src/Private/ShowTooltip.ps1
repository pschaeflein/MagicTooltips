[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ShowTooltip = {
    param
    (
        [string]$tooltip,
        [string]$foregroundColor,
        [string]$backgroundColor
    )

    $saveX = [console]::CursorLeft
    $saveY = [console]::CursorTop

    $drawX = [Console]::WindowWidth - $tooltip.Length - 1
    [console]::CursorVisible = $false
    [console]::setcursorposition($drawX, $saveY)
    Write-Host -Object $tooltip -ForegroundColor $foregroundColor -BackgroundColor $backgroundColor -NoNewline
    [console]::setcursorposition($saveX, $saveY)
    [console]::CursorVisible = $true
}