# .NET Core Global Tool to find work protected files

List the files protected by [Windows Information Protection][wip]. This tool uses [Cipher.exe][cipher] to identify the work protected files.

## Pre-requisites

- `Windows`
- [.NET Core SDK 2.2.104][dotnet-core-sdk]

Once you're done, run this command to initialise `Cake`:

```powershell
.\bootstrap.ps1
```

You can then run the build script:

```powershell
dotnet cake build.cake
```

If you want to pack the `.NET Core Global Tool` you can run: `dotnet cake build.cake --pack`

## Use the .NET Core global tool

The `Azure DevOps` build is publishing the global tool as a build artifact named `global-tool`

```powershell
dotnet tool install --global --add-source <directory-where-you-downloaded-the-package> dotnet-fwpf
```

You can then use it:

```powershell
dotnet fwpf C:\
```

[dotnet-core-sdk]: https://dotnet.microsoft.com/download
[wip]: https://docs.microsoft.com/en-us/windows/security/information-protection/windows-information-protection/protect-enterprise-data-using-wip
[cipher]: https://support.microsoft.com/en-au/help/298009/cipher-exe-security-tool-for-the-encrypting-file-system
