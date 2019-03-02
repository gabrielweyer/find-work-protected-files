# .NET Core Global Tool to find work protected files

List the files protected by [Windows Information Protection][wip]. This tool uses [Cipher.exe][cipher] to identify the work protected files.

[![Build Status](https://dev.azure.com/gabrielweyer/find-work-protected-files/_apis/build/status/gabrielweyer.find-work-protected-files?branchName=master)][build]

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

The `Azure DevOps` [build][build] is publishing the global tool as a build artifact named `global-tool`

```powershell
dotnet tool install --global --add-source <directory-where-you-downloaded-the-package> dotnet-fwpf
```

You can then use it:

```powershell
dotnet fwpf C:\
```

## Sample output

```plaintext
λ  dotnet fwpf C:\
===============
=  Arguments  =
===============
Searching path: 'C:\'

=====================
=  Invoking cipher  =
=====================
Invoking cipher with arguments: '/c /s:C:\' (this might take a few minutes)
cipher took: 00:05:35.7800584

===========================
=  Parsing cipher output  =
===========================

'C:\Users\gabri\AppData\Local\Microsoft\EDP\Recovery\EdpAccount_2ebb35ad-cca4-4c91-827b-e31b0a4f3216_1' is protected by company 'company-name.com'
'C:\Users\gabri\AppData\Local\Microsoft\OneNote\16.0\cache\00000555.bin' is protected by company 'company-name.com'
// Abbreviated
'C:\Users\gabri\AppData\Local\Microsoft\Outlook\Offline Address Books\71c5fb9c-c4f9-4040-a5cc-22d1144f5f40\uANRdex.oab' is protected by company 'company-name.com'
// Abbreviated
'C:\Users\gabri\AppData\Roaming\Microsoft\Teams\installTime.txt' is protected by company 'company-name.com'
'C:\Users\gabri\AppData\Roaming\Microsoft\Teams\logs.txt' is protected by company 'company-name.com'
'C:\Users\gabri\AppData\Roaming\Microsoft\Teams\QuotaManager' is protected by company 'company-name.com'
'C:\Users\gabri\AppData\Roaming\Microsoft\Teams\QuotaManager-journal' is protected by company 'company-name.com'
// Abbreviated
cipher output parsing took: 00:00:01.6937454

We're done, have a good one
```

[dotnet-core-sdk]: https://dotnet.microsoft.com/download
[wip]: https://docs.microsoft.com/en-us/windows/security/information-protection/windows-information-protection/protect-enterprise-data-using-wip
[cipher]: https://support.microsoft.com/en-au/help/298009/cipher-exe-security-tool-for-the-encrypting-file-system
[build]: https://dev.azure.com/gabrielweyer/find-work-protected-files/_build/latest?definitionId=17&branchName=master
