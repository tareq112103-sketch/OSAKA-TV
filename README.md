# OSAKA TV

OSAKA TV is an Android IPTV player for Android phones, tablets and Android TV. It is based on the upstream Kotlin / Jetpack Compose / AndroidX Media3 project and is maintained in this repository.

## App identity
- App name: **OSAKA TV**
- Android application ID: `com.osaka.tv`
- Minimum Android version: Android 8.0 (API 26)
- Target SDK: 35
- Stack: Kotlin, Jetpack Compose, AndroidX Media3 (ExoPlayer + HLS)
- Inputs: user-provided M3U / M3U8 playlist URL or local playlist file

## Build
Requirements: JDK 17 and Android SDK 35.
```bash
gradle wrapper --gradle-version 8.9
./gradlew assembleDebug
```
APK: `app/build/outputs/apk/debug/app-debug.apk`.

## GitHub Actions
Every push to `main` and every manual workflow run builds the debug APK. Download **OSAKA-TV-debug-apk** from the workflow run's Artifacts section.

## Updates
This repository is the source of truth for OSAKA TV. Future builds are produced from its `main` branch. Android does not permit silent installation of sideloaded APK updates: users must approve installation. Release signing must use the same private keystore for every upgrade. Never commit keystores or passwords.

## Playlist and content
OSAKA TV is a player, not a broadcaster or content provider. No third-party channel list is bundled by default. Add an M3U/M3U8 playlist that you own or are authorized to use in Settings. `playlist.m3u` is an empty starter file.

## Development
The initial source migration preserves the upstream playback, playlist parsing, channel browsing and settings implementation while changing the Android package, app identity, preference namespace and build pipeline. Use only playlists, streams and EPG sources you are authorized to access.
