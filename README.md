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

### Download and install
After the Windows GitHub Actions workflow completes, download **OSAKA-TV-Setup-1.0.0.exe** from the **OSAKA-TV-Windows-11-x64** artifact. Run the installer and follow the setup wizard. It installs OSAKA TV, creates a Start Menu shortcut, and includes an uninstaller. A desktop shortcut is available as an optional installer task.

The same artifact also contains **OSAKA-TV-Windows-Portable-x64.zip** for users who prefer not to install the app. Extract it and run `OSAKA-TV.exe`.

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
APK: `Android/build/outputs/apk/debug/app-debug.apk`.

## GitHub Actions
- **Android APK:** runs only when Android source/build files change (or when manually started). Open the [Android APK workflow](.github/workflows/android.yml) and download **OSAKA-TV-Android-APK** from its latest successful run.
- **Windows 11:** runs only when files under `windows/` change (or when manually started). Download **OSAKA-TV-Windows-11-x64** from the Windows workflow run's Artifacts section.

## Updates
The Windows version is distributed as an Inno Setup installer and a portable ZIP through GitHub Actions. The installer is currently unsigned, so Windows SmartScreen may show an unknown-publisher warning. This is expected until a code-signing certificate is configured. Automatic in-app updates are not included yet.

## Playlist and content
`playlist.m3u` is shared by both app versions. Channel availability, geographic restrictions and stream URLs can change; some channels may not be reachable in every country. No paid or subscription-only channels are intentionally included. Use only streams you are authorized to access.

## LG webOS
The LG Smart TV version is a separate web app under `webos/`. It loads the shared `playlist.m3u` from GitHub, provides remote-friendly channel browsing, category filtering, search, favourites and native HTML5 video playback.

### Build and download
The **Build OSAKA TV for LG webOS** GitHub Actions workflow packages an installable `.ipk` file. Open the workflow run and download the `OSAKA-TV-webOS-IPK` artifact. The package is a developer/test build; installation uses LG Developer Mode and webOS CLI. LG Content Store distribution requires LG's separate review and approval.

For build and device setup instructions, see [webos/README.md](webos/README.md).
