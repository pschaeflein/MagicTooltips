function Get-CommandList() {
    $commandList = @{
        "kubectl.exe"   = "kubernetes"
        "helm.exe"      = "kubernetes"
        "az.cmd"        = "azure"
        "terraform.exe" = "azure"
    }

    $keys = @() + $commandList.Keys
    foreach ($key in $keys) {
        $alias = $key.replace(".exe", "").replace(".cmd", "");
        if ($key -ne $alias) {
            $commandList.Add($alias, $commandList[$key])
        }
    }

    $keys = @() + $commandList.Keys
    foreach ($key in $keys) {
        $aliases = (get-alias).Where( { $_.Definition -eq $key }).Name;
        foreach ($alias in $aliases) {
            $commandList.Add($alias, $commandList[$key])
        }
    }

    return $commandList
}