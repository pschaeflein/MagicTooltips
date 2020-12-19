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
    param($command, $Context, $tooltipDtoType)

    try {
        $tooltipData = $null;

        if ($command -eq "kubernetes") {
            $tooltipData = .$Context.Providers.Kubernetes $state
        }
        elseif ($command -eq "azure") {
            $tooltipData = .$Context.Providers.Azure $Context
        }
        else {
            .$Context.WriteLog "Unknown command: '$command'"
        }

        if ($null -ne $tooltipData) {
            .$Context.ShowTooltip $tooltipData.Text $tooltipData.ForegroundColor $tooltipData.BackgroundColor
        }
    }
    catch {
        .$Context.WriteLog "[ThreadJobSb] ERROR: $_"
        .$Context.WriteLog $_.ScriptStackTrace
    }
}