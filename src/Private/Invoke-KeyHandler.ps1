function Invoke-KeyHandler() {
    try {
        $line = $null
        $cursor = $null
        [Microsoft.PowerShell.PSConsoleReadLine]::GetBufferState([ref]$line, [ref]$cursor)
        .$Context.WriteLog "line: '$line'"

        # insert the space that was swallowed by the KeyHandler
        [Microsoft.PowerShell.PSConsoleReadLine]::Insert(" ")

        $command = $CommandList[$line]
        if ($null -eq $command) {
            return
        }
        .$Context.WriteLog "command: $command"

        Start-ThreadJob -Name "MagicTooltips-KeyHandler" -StreamingHost $Host `
            -ScriptBlock $ThreadJob `
            -ArgumentList ($command, $Context, [ToolTipDto])
    }
    catch {
        .$Context.WriteLog "[Invoke-KeyHandler] ERROR: $_"
        .$Context.WriteLog $_.ScriptStackTrace
    }
}

$ThreadJob = {
    param($provider, $Context, $tooltipDtoType)
    function Set-Overrides($provider, $tooltipData) {
        $fgColor = $Context.Configuration.Providers[$provider].FgColor
        if ($null -ne $fgColor) {
            $tooltipData.ForegroundColor = $fgColor
        }

        $bgColor = $Context.Configuration.Providers[$provider].BgColor
        if ($null -ne $bgColor) {
            $tooltipData.BackgroundColor = $bgColor
        }

        $template = $Context.Configuration.Providers[$provider].Template
        if ($null -ne $template) {
            $tooltipData.Template = $template
        }
    }

    try {
        $tooltipData = $null

        switch ($provider) {
            "kubernetes" {
                $tooltipData = .$Context.Providers.Kubernetes $Context
            }
            "azure" {
                $tooltipData = .$Context.Providers.Azure $Context
            }
            default {
                .$Context.WriteLog "Unknown provider: '$provider'"
            }
        }

        if ($null -ne $tooltipData) {
            Set-Overrides $provider $tooltipData
            .$Context.ShowTooltip $tooltipData
        }
    }
    catch {
        .$Context.WriteLog "[ThreadJobSb] ERROR: $_"
        .$Context.WriteLog $_.ScriptStackTrace
    }
}