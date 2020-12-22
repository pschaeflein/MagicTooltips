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
    $drawX = $saveX
    $drawY = $saveY

    switch($Context.Configuration.HorizontalAlignment) {
        "left" {
            $drawX = $Context.Configuration.HorizontalOffset
        }
        default {
            $drawX = [Console]::WindowWidth - $tooltip.Length - $Context.Configuration.HorizontalOffset
        }
    }

    $drawY += $Context.Configuration.VerticalOffset

    [console]::CursorVisible = $false
    [console]::setcursorposition($drawX, $drawY)

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