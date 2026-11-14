# OSAKA TV

OSAKA TV is a multi-platform IPTV player maintained in this repository:
- **Android:** phones, tablets and Android TV (Kotlin, Jetpack Compose, AndroidX Media3).
- **Windows 11:** native Windows desktop app (C# / WPF, LibVLCSharp).

## App identity
- App name: **OSAKA TV**
- Android application ID: `com.osaka.tv`
- Windows app: `windows/OSAKA-TV.Windows`
- Windows target: Windows 10/11 x64 (Windows 11 recommended)
- Windows playback: LibVLC / LibVLCSharp
- Android minimum: Android 8.0 (API 26)
- Android target SDK: 35
- Android stack: Kotlin, Jetpack Compose, AndroidX Media3 (ExoPlayer + HLS)

## Windows 11
The Windows version is a separate desktop project. It does not replace or modify the Android app.

Current desktop features:
- Loads the latest playlist from this repository on startup.
- Opens local M3U and M3U8 files.
- Browses playlist groups and searches channel names.
- Plays network streams using LibVLC.
- Supports favourites, stop/play and full-screen mode (F11; Escape exits full screen).
- Supports common stream formats handled by VLC, including HLS where the source is compatible.

### Download
After the Windows GitHub Actions workflow completes, open the **Build OSAKA TV for Windows 11** run and download the **OSAKA-TV-Windows-11-x64** artifact. Extract the ZIP and run `OSAKA-TV.exe`.

### Build locally
Install the .NET 8 SDK and run:
```powershell
dotnet restore windows/OSAKA-TV.Windows/OSAKA-TV.Windows.csproj
dotnet publish windows/OSAKA-TV.Windows/OSAKA-TV.Windows.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish/windows-x64
```

The first Windows build is an initial desktop release. Favourites are currently kept in memory for the current session; persistent settings, EPG and richer TV-style browsing can be added in subsequent updates.

## Android build
Requirements: JDK 17 and Android SDK 35.
```bash
gradle wrapper --gradle-version 8.9
./gradlew assembleDebug
```
APK: `app/build/outputs/apk/debug/app-debug.apk`.

## GitHub Actions
- Android: every push to `main` and manual workflow run builds the debug APK. Download **OSAKA-TV-debug-apk** from the Android workflow run's Artifacts section.
- Windows: changes under `windows/` (or a manual workflow run) build a self-contained Windows x64 package. Download **OSAKA-TV-Windows-11-x64** from the Windows workflow run's Artifacts section.

## Updates
This repository is the source of truth for OSAKA TV. Android sideloaded APK updates cannot install silently: users must approve installation. Release signing must use the same private keystore for every upgrade. Never commit keystores or passwords.

The Windows build is currently distributed as a GitHub Actions ZIP artifact; it is not yet an auto-updating signed installer. A later release can add an installer and in-app update checks.

## Playlist and content
`playlist.m3u` is shared by both app versions. Channel availability, geographic restrictions and stream URLs can change; some channels may not be reachable in every country. No paid or subscription-only channels are intentionally included. Use only streams you are authorized to access.
