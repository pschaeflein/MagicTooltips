
#Get public and private function definition files.
$Public = @( Get-ChildItem -Path $PSScriptRoot\Public\*.ps1 -Recurse -ErrorAction SilentlyContinue )
$Private = @( Get-ChildItem -Path $PSScriptRoot\Private\*.ps1 -Recurse -ErrorAction SilentlyContinue )

#Dot source the files
Foreach ($import in @($Public + $Private)) {
    Try {
        . $import.fullname
    }
    Catch {
        Write-Error -Message "Failed to import function $($import.fullname): $_"
    }
}

.$WriteLog "------------------------"
.$WriteLog "Initializing"

[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$Context = @{
    WriteLog = $WriteLog
    ShowTooltip = $ShowTooltip
    Providers = @{
        Kubernetes = $ProviderKubernetes
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$CommandList = Get-CommandList

Remove-PSReadlineKeyHandler -Key SpaceBar
Set-PSReadlineKeyHandler -Key SpaceBar -ScriptBlock { Invoke-KeyHandler }