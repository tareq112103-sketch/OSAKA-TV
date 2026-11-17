#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
#ifndef SourceDir
  #define SourceDir "..\..\publish\windows-x64"
#endif

#define AppName "OSAKA TV"
#define AppPublisher "OSAKA TV"
#define AppExeName "OSAKA-TV.exe"

[Setup]
AppId={{A74D6A6B-49B5-4B3A-9C6D-0A5A7A000001}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\OSAKA TV
DefaultGroupName=OSAKA TV
DisableProgramGroupPage=yes
OutputDir=..\..\..\dist
OutputBaseFilename=OSAKA-TV-Setup-{#AppVersion}
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
UninstallDisplayIcon={app}\{#AppExeName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
CloseApplications=force
RestartApplications=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\OSAKA TV"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\OSAKA TV"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "Launch OSAKA TV"; Flags: postinstall nowait skipifsilent
