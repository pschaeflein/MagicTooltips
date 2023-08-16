# ✨ Magic Tooltips ✨

A PowerShell module to display contextual information about the command you're currently entering.

![Magic Tooltips Demo](/media/demo.gif)

Pairs nicely with custom prompts, such as [oh-my-posh](https://github.com/JanDeDobbeleer/oh-my-posh)!
![Magic Tooltips with oh-my-posh3](media/oh-my-posh.png)


Supported Providers:
- Microsoft Graph Powershell - Shows the name of the connected account
- M365 - Shows the name of the logged-in account for the CLI for Microsoft 365
- Kubernetes - Shows the current kubernetes context
- Azure - Shows the name of the current azure subscription
- AWS - Shows the name of the selected AWS Profile (the AWS_Profile environment variable)
- Microsoft Graph CLI (PREVIEW) - Shows the name of the connected account

---
## Prerequisites
- Powershell 7+
- CLI tools installed and in your path for one or more of the supported providers
- (optional) A [Nerd Font](https://www.nerdfonts.com/) installed and selected as your terminal's font

---
## Installation

You can install and import Magic Tooltips from the PowerShell Gallery:

```pwsh
Install-Module MagicTooltips
Import-Module MagicTooltips -Force
```

To make the module auto-load, add the Import-Module line to your [PowerShell profile](#powershell-profile).

---
## Configuration

MagicTooltips is configured by setting a global variables in your [PowerShell profile](#powershell-profile). Below is a sample showing all of the possible settings and their default values.

> **NOTE:** v2 Breaking change
>
> The Azure provider has been separated into AzCLI and AzPwsh.

```pwsh
$global:MagicTooltipsSettings = @{
    Debug = $true
    HorizontalAlignment = "Right"
    VerticalOffset = -1
    HorizontalOffset = 0
    Providers= @{
        MG = @{
            NounPrefixes = "mg"
            FgColor => "#32A5E6"
            BgColor => "";
            Template => "\uf871 {value}";
        }
        M365 = @{
            Commands = "m365"
            FgColor  = "#EF5350"
            BgColor  = ""
            Template = "\uf8c5 {value}"
        }
        AzCLI = @{
            Commands = "az,terraform,pulumi,terragrunt"
            FgColor = "#3A96DD"
            BgColor = ""
            Template = "\ufd03 {value}"
        }
        AzPwsh = @{
            NounPrefixes = "az"
            FgColor = "#3A96DD"
            BgColor = ""
            Template = "\ufd03 {value}"
        }
        Kubernetes = @{
            Commands = "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmfile"
            FgColor = "#AE5FD6"
            BgColor = ""
            Template = "\ufd31 {value}"
        }
        Aws = @{
            Commands = "aws,awless,terraform,pulumi,terragrunt"
            FgColor = "#EC7211"
            BgColor = ""
            Template = "\uf270 {value}"
        }
    }
}
```

Feel free to delete settings that you do not want to change. For example, if the only thing you want to change is to add `k` to the list of kubernetes commands, this is a perfectly valid configuration:

```pwsh
$global:MagicTooltipsSettings = @{
    Providers= @{
        Kubernetes = @{
            Commands = "kubectl,helm,kubens,kubectx,oc,istioctl,kogito,k9s,helmfile,k"
        }
    }
}
```

### Triggers
To configure what will trigger MagicTooltips, edit the `Command` and `NounPrefixes` settings for a provider. This is a comma-separated list of values. If the entry in the terminal contains a command, or a PowerShell command begins with a specified prefix, the provider will be triggered to display a MagicTooltip.

### Colors
To configure the colors, use hex colors in the `FgColor` and `BgColor` variables.

### Templates
MagicTooltips are displayed using a simple template language in the `Template` variables. The string `{value}` will be replaced with the value returned by the provider (Microsoft Graph connected account, for example).

If you would like to use icons in your template, make sure you have a [Nerd Font](https://www.nerdfonts.com/) selected as your terminal's font. Specify icons using the syntax ` \uf871` where `f871` is the hex code for the unicode character you wish to print. You can find these hex codes on the [Nerd Font Cheat Sheet](https://www.nerdfonts.com/cheat-sheet).

### Placement
To configure placement, set the following variables: 

- `HorizontalAlignment` default "right". Possible values are "left" or "right"
- `HorizontalOffset` default 0. specify the number of columns to offset from the left or right edge of the terminal
- `VerticalOffset` default 0. specify the number of rows to offset from the cursor position. Negative values will cause printing 
to appear above the cursor row, while positive values will print below the cursor row.

### Debug
To enable debug logs, set the `Debug` variable to `$true`. The log file is called `magictooltips.log` and is written to the module directory, which can be found using
```pwsh
(Get-Module MagicTooltips).ModuleBase
```

(Debug logs will not work if the module is installed with the `AllUsers` scope.)

---
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


---
## Acknowledgments
- [Powerlevel10k](https://github.com/romkatv/powerlevel10k)
- [oh-my-posh3](https://github.com/JanDeDobbeleer/oh-my-posh3)
- [Nerd Fonts](https://www.nerdfonts.com/)
- [gitmoji](https://gitmoji.dev/)