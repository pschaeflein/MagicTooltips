Copy-Item -Path "./src/" -Destination "./MagicTooltips"  -Recurse

Set-PSRepository -Name PSGallery -InstallationPolicy Trusted

$nuGetApiKey = $env:PS_GALLERY_KEY

$releaseNotes = $env:RELEASE_NOTES
$moduleVersion = ($env:RELEASE_VERSION) -replace "v",""
Write-Host "ModuleVersion: $moduleVersion"

$manifestPath = Resolve-Path -Path "./MagicTooltips/MagicTooltips.psd1"
Write-Host "Manifest Path: $manifestPath"

Update-ModuleManifest -ReleaseNotes $releaseNotes -Path $manifestPath.Path -ModuleVersion $moduleVersion #-Verbose

$moduleFilePath = Resolve-Path -Path "./MagicTooltips/MagicTooltips.psm1"
Write-Host "Module File Path: $moduleFilePath"

$modulePath = Split-Path -Parent $moduleFilePath
Write-Host "Module Path: $modulePath"


try{
    Publish-Module -Path $modulePath -NuGetApiKey $nuGetApiKey -ErrorAction Stop -Force
    Write-Host "v$($moduleVersion) has been Published to the PowerShell Gallery!"
}
catch {
    throw $_
}
