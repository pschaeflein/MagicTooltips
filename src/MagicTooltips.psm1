#Get public and private function definition files.
$Public = @( Get-ChildItem -Path $PSScriptRoot\Public\*.ps1 -Recurse -ErrorAction SilentlyContinue )
$Private = @( Get-ChildItem -Path $PSScriptRoot\Private\*.ps1 -Recurse -ErrorAction SilentlyContinue )

#Dot source the files
foreach ($import in @($Public + $Private)) {
    try {
        . $import.fullname
    }
    catch {
        Write-Error -Message "Failed to import function $($import.fullname): $_"
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$Context = @{
    WriteLog    = $WriteLog
    ShowTooltip = $ShowTooltip
    Providers   = @{
        Kubernetes = $ProviderKubernetes
        Azure      = $ProviderAzure
    }
    Configuration = Get-Configuration
}

.$WriteLog "------------------------"
.$WriteLog "Initializing"

[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$CommandList = Get-CommandList

function Register-MagicTooltips {
    .$WriteLog "Registering"
    Remove-PSReadlineKeyHandler -Key SpaceBar
    Set-PSReadlineKeyHandler -Key SpaceBar -ScriptBlock { Invoke-KeyHandler }
}

Export-ModuleMember Register-MagicTooltips
Register-MagicTooltips
