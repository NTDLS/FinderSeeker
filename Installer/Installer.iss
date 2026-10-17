; Build the program first, self-contained so that the installed app needs no .NET runtime:
;   dotnet publish FinderSeeker\FinderSeeker.csproj -c Release -r win-x64 --self-contained
#define CompanyName         "NetworkDLS"
#define ApplicationName     "FinderSeeker"
#define ApplicationVersion  "2.0.0.0"
#define PublishFolder       "..\FinderSeeker\bin\Release\net10.0-windows\win-x64\publish"

[Setup]
 AppName                         = {#ApplicationName}
 AppVerName                      = {#ApplicationName} {#ApplicationVersion}
 AppCopyright                    = Copyright (c) 2000-2026 {#CompanyName}.
 DefaultDirName                  = {commonpf}\{#CompanyName}\{#ApplicationName}
 DefaultGroupName                = {#CompanyName}\{#ApplicationName}
 UninstallDisplayIcon            = {app}\Resources\Uninstall.ico
 PrivilegesRequired              = admin
 Uninstallable                   = Yes
 Compression                     = ZIP/9
 MinVersion                      = 10.0
 ArchitecturesInstallIn64BitMode = x64compatible
 ArchitecturesAllowed            = x64compatible
 OutputBaseFilename              = {#ApplicationName} {#ApplicationVersion}
 AppPublisher                    = {#CompanyName}
 AppPublisherURL                 = http://www.NetworkDLS.com/
 AppUpdatesURL                   = http://www.NetworkDLS.com/
 AppVersion                      = {#ApplicationVersion}

[Files]
 Source: "_Resources\Uninstall.ico"; DestDir: "{app}\Resources\"; Flags: IgnoreVersion;
 Source: "{#PublishFolder}\*";       DestDir: "{app}";            Flags: IgnoreVersion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[Icons]
 Name: "{group}\{#ApplicationName}"; Filename: "{app}\FinderSeeker.exe"; WorkingDir: "{app}";

[Run]
 Filename: "{app}\FinderSeeker.exe"; Description: "Launch {#ApplicationName}."; Flags: postinstall nowait skipifsilent shellexec;
