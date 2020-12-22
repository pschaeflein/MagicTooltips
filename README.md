# ✨ Magic Tooltips ✨

A PowerShell module to display contextual information about the command you're currently entering.

![Magic Tooltips Demo](/media/demo.gif)

Pairs nicely with custom prompts, such as [oh-my-posh3](https://github.com/JanDeDobbeleer/oh-my-posh3)!
![Magic Tooltips with oh-my-posh3](/media/oh-my-posh3.png)


Supported Providers:
- Kubernetes - Shows the current kubernetes context
- Azure - Shows the name of the current azure subscription
- (more to come)

## Installation

You can install and import Magic Tooltips from the PowerShell Gallery:

```pwsh
Install-Module MagicTooltips
Import-Module MagicTooltips
```

To make the module auto-load, add the Import-Module line to your [PowerShell profile](#powershell-profile).

## Configuration

Configuration is done by setting global variables in your [PowerShell profile](#powershell-profile).

### Commands
To configure when different tooltips are shown, edit the command variables with a comma separted list of commands
```pwsh
# Kubernetes
$global:MagicTooltips_KubernetesCommands = "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmlist"

# Azure
$global:MagicTooltips_AzureCommands = "az,terraform,pulumi,terragrunt"

```

### Colors
To configure the colors, use hex colors in the following variables
```pwsh
$global:MagicTooltips_KubernetesFgColor="#AE5FD6"
$global:MagicTooltips_KubernetesBgColor="#000000"
$global:MagicTooltips_AzureFgColor="#3A96DD"
$global:MagicTooltips_AzureBgColor="#000000"
```

### Debug
To enable debug logs, set the following variable.
```pwsh
$global:MagicTooltips_Debug="true"
```
 The log file is called `magictooltips.log` and is written to the module directory, which can be found using
```pwsh
(Get-Module MagicTooltips).ModuleBase
```


## PowerShell Profile

The path to your PowerShell profile is stored by PowerShell in the variable `$profile`. The following commands will launch a text editor with your profile:

Visual Studio Code
```pwsh
code $profile
```

Notepad
```pwsh
notepad $profile
```

Once you have made changes to your profile, you can reload your profile in PowerShell:
```pwsh
.$profile
```

## Roadmap
- More Providers
    - AWS
    - Gcloud
- Customization
    - Colors
    - Templates
    - Placement
- Caching for performance improvements

## Acknowledgments
- [Powerlevel10k](https://github.com/romkatv/powerlevel10k)
- [oh-my-posh3](https://github.com/JanDeDobbeleer/oh-my-posh3)
- [Nerd Fonts](https://www.nerdfonts.com/)
