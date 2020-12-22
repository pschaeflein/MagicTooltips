[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseDeclaredVarsMoreThanAssignments', '')]
$WriteLog = {
    param
    (
        [string]$Message
    )

    if ($Context.Configuration.Debug -ne "true") {
        return
    }

    $path = "$($PSScriptRoot)\..\magictooltips.log"
    $FormattedDate = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    "$FormattedDate $Message" | Out-File -FilePath $Path -Append
}