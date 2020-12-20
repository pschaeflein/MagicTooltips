# ✨ Magic Tooltips ✨

A PowerShell module to display contextual information about the command you're currently entering.

Supported Providers:
- Kubernetes
- Azure
- (more to come)

## Installation

You can install and import Magic Tooltips from the PowerShell Gallery:

```pwsh
Install-Module MagicTooltips
Import-Module MagicTooltips
```

To make the module auto-load, add the Import-Module line to your [PowerShell profile](#powershell-profile).

## Configuraion

Configuration is done by setting global variables in your [PowerShell profile](#powershell-profile).

### Commands
To customize when different tooltips are shown, edit the command variables with a comma separted list of commands
```pwsh
# Kubernetes
$global:MagicTooltips_KubernetesCommands = "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmlist"

# Azure
$global:MagicTooltips_AzureCommands = "az,terraform,pulumi,terragrunt"

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
