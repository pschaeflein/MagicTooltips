$distFolder = "./dist/MagicTooltips/"
if (Test-Path $distFolder) {
    Remove-Item $distFolder -Recurse -Force
}
New-Item $distFolder -ItemType "directory"

Copy-Item -Path "./src/MagicTooltips.psd1" -Destination $distFolder
Copy-Item -Path "./src/MagicTooltips.psm1" -Destination $distFolder

dotnet build ./src/MagicTooltips.csproj -o "$($distFolder)lib/"

Set-PSRepository -Name PSGallery -InstallationPolicy Trusted

$nuGetApiKey = $env:PS_GALLERY_KEY
$releaseNotes = $env:RELEASE_NOTES
$moduleVersion = ($env:RELEASE_VERSION) -replace "v",""

Write-Host "ModuleVersion: $moduleVersion"

$manifestPath = Resolve-Path -Path "$($distFolder)MagicTooltips.psd1"
Write-Host "Manifest Path: $manifestPath"

Update-ModuleManifest -ReleaseNotes $releaseNotes -Path $manifestPath.Path -ModuleVersion $moduleVersion #-Verbose

$moduleFilePath = Resolve-Path -Path "$($distFolder)MagicTooltips.psm1"
Write-Host "Module File Path: $moduleFilePath"

try{
    Publish-Module -Path $distFolder -NuGetApiKey $nuGetApiKey -ErrorAction Stop -Force
    Write-Host "v$($moduleVersion) has been Published to the PowerShell Gallery!"
}
catch {
    throw $_
}
