[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ShowTooltip = {
    param
    (
        [string]$tooltip,
        [string]$foregroundColor,
        [string]$backgroundColor
    )
    $fgRgb = Convert-HexToRgb($foregroundColor)
    $bgRgb = Convert-HexToRgb($backgroundColor)
    $coloredTooltip = "`e[38;2;${fgRgb}m`e[48;2;${bgRgb}m$tooltip`e[0m"

    $saveX = [console]::CursorLeft
    $saveY = [console]::CursorTop

    $drawX = [Console]::WindowWidth - $tooltip.Length - 1
    [console]::CursorVisible = $false
    [console]::setcursorposition($drawX, $saveY)
    Write-Host -NoNewline $coloredTooltip
    [console]::setcursorposition($saveX, $saveY)
    [console]::CursorVisible = $true
}

function Convert-HexToRgb($hex) {
    $red = [convert]::ToInt32($hex.Substring(1,2), 16)
    $green = [convert]::ToInt32($hex.Substring(3,2), 16)
    $blue = [convert]::ToInt32($hex.Substring(5,2), 16)
    return "${red};${green};${blue}"
}