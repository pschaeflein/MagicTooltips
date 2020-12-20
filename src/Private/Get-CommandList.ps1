function Get-CommandList() {
    $commandList = @{}

    function Add-Command($command, $provider) {
        .$context.WriteLog "$command = $provider"
        if (!$commandList.ContainsKey($command)) {
            $commandList.Add($command, $provider)
        }
    }

    function Add-Commands($provider, $defaultCommands) {
        .$Context.WriteLog "Add-Commands $provider"
        $commands = $defaultCommands
        $customCommands = Get-Variable "MagicTooltips_$($provider)Commands" -ValueOnly -ErrorAction SilentlyContinue

        if ($null -ne $customCommands) {
            .$Context.WriteLog "Using custom commands"
            $commands = $customCommands
        }
    
        foreach ($command in $commands.split(',')) {
            Add-Command $command $provider
        }
    }

    function Add-Aliases() {
        .$Context.WriteLog "Add-Aliases"
        $keys = @() + $commandList.Keys
        foreach ($key in $keys) {
            $alias = $key.replace(".exe", "").replace(".cmd", "");
            if ($key -ne $alias) {
                Add-Command $alias $commandList[$key]
            }
        }
    
        $keys = @() + $commandList.Keys
        foreach ($key in $keys) {
            $aliases = (get-alias).Where( { $_.Definition -eq $key }).Name;
            foreach ($alias in $aliases) {
                Add-Command $alias $commandList[$key]
            }
        }
    }

    Add-Commands "kubernetes" "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmlist"
    Add-Commands "azure" "az,terraform,pulumi,terragrunt"
    Add-Aliases
    return $commandList
}