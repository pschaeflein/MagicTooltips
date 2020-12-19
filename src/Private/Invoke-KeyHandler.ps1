function Invoke-KeyHandler() {
    try {
        $line = $null
        $cursor = $null
        [Microsoft.PowerShell.PSConsoleReadLine]::GetBufferState([ref]$line, [ref]$cursor)
        .$WriteLog "line: '$line'"
    
        # insert the space that was swallowed by the KeyHandler
        [Microsoft.PowerShell.PSConsoleReadLine]::Insert(" ")
    
        $command = $CommandList[$line]
        if ($null -eq $command) {
            return
        }
        .$WriteLog "command: $command"
    
        Start-ThreadJob -Name "MagicTooltips-KeyHandler" -StreamingHost $Host `
            -ScriptBlock $ThreadJob `
            -ArgumentList ($command, $WriteLog, $ShowTooltip, [ToolTipDto])
    }
    catch {
        .$WriteLog "[Invoke-KeyHandler] ERROR: $_"
        .$WriteLog $_.ScriptStackTrace
    }
}

$ThreadJob = {
    param($command, $WriteLog, $ShowTooltip, $tooltipDtoType)

    try {
        $tooltipData = $null;

        # placeholder until providers are created
        $tooltipData = $tooltipDtoType::new()
        $tooltipData.Text = "test"
        $tooltipData.ForegroundColor = [System.ConsoleColor]::White
        $tooltipData.BackgroundColor = [System.ConsoleColor]::Red
        
        if ($null -ne $tooltipData) {
            .$ShowTooltip $tooltipData.Text $tooltipData.ForegroundColor $tooltipData.BackgroundColor
        }
    }
    catch {
        .$WriteLog "[ThreadJobSb] ERROR: $_"
        .$WriteLog $_.ScriptStackTrace
    }
}