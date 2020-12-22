[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$ShowTooltip = {
    param
    (
        $tooltip
    )
    $fgRgb = Convert-HexToRgb($tooltip.ForegroundColor)
    $bgRgb = Convert-HexToRgb($tooltip.BackgroundColor)
    $tooltipText = $tooltip.Template.Replace("{value}", $tooltip.Value)
    $coloredTooltip = "`e[38;2;${fgRgb}m`e[48;2;${bgRgb}m$tooltipText`e[0m"

    $saveX = [console]::CursorLeft
    $saveY = [console]::CursorTop
    $drawX = $saveX
    $drawY = $saveY

    switch($Context.Configuration.HorizontalAlignment) {
        "left" {
            $drawX = $Context.Configuration.HorizontalOffset
        }
        default {
            $drawX = [Console]::WindowWidth - $tooltipText.Length - $Context.Configuration.HorizontalOffset
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