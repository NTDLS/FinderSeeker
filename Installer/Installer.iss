;#define Debug
#define CompanyName         "NetworkDLS"
#define ApplicationName     "FinderSeeker"
#define ApplicationVersion  "1.0.0.0"

[Setup]
;-- Main Setup Information
 AppName                         = {#ApplicationName}
 AppVerName                      = {#ApplicationName} {#ApplicationVersion}
 AppCopyright                    = Copyright � 2000-2023 {#CompanyName}.
 DefaultDirName                  = {commonpf}\{#CompanyName}\{#ApplicationName}
 DefaultGroupName                = {#CompanyName}\{#ApplicationName}
 UninstallDisplayIcon            = {app}\Resources\Uninstall.ico
 PrivilegesRequired              = admin
 Uninstallable                   = Yes
 Compression                     = ZIP/9
 MinVersion                      = 6.2
 ArchitecturesInstallIn64BitMode = x64
 ArchitecturesAllowed            = x86 x64
 OutputBaseFilename              = {#ApplicationName} Client {#ApplicationVersion}
 
;-- Windows 2000+ Support Dialog
 AppPublisher    = {#CompanyName}
 AppPublisherURL = http://www.NetworkDLS.com/
 AppUpdatesURL   = http://www.NetworkDLS.com/
 AppVersion      = {#ApplicationVersion}

[Files]
 Source: "_Resources\Uninstall.ico";                                DestDir: "{app}\Resources\";  Flags: IgnoreVersion;
 Source: "..\..\SetupResources\Setup\License\EULA.txt";          DestDir: "{app}";             

  
;----------------------------------------------------------(DIS.FinderSeekerClientService)----
Source: "..\FinderSeeker\bin\Release\net7.0-windows\*.json";  Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;
 Source: "..\FinderSeeker\bin\Release\net7.0-windows\*.exe";  Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;
 Source: "..\FinderSeeker\bin\Release\net7.0-windows\*.dll";  Excludes: "*vshost*"; DestDir: "{app}";              Flags: IgnoreVersion;

;---------------------------------------------------------------------

[Icons]
 Name: "{group}\Configure {#ApplicationName}"; Filename: "{app}\DIS.ClientConfiguration.exe";
 Name: "{group}\Update {#ApplicationName}";    Filename: "{app}\AutoUpdate.Exe"; WorkingDir: "{app}";

;---------------------------------------------------------------------

[Run]
 Filename: "{app}\FinderSeeker"; Description: "Launch FinderSeeker."; Flags: postinstall nowait skipifsilent shellexec;

;---------------------------------------------------------------------
