# OSAKA TV for LG webOS

This is a separate LG webOS TV web application. It shares the repository's public M3U playlist and does not modify Android or Windows projects.

## Build

Install Node.js 20 and the current LG webOS CLI, then run:

```bash
npm install --global @webos-tools/cli
ares-package webos --no-minify
```

The resulting `.ipk` is the installable webOS package.

## Test on a TV

1. On the LG TV, open Apps and install **Developer Mode** if available for your region/model.
2. Enable Developer Mode and set a passphrase.
3. On the computer, configure the TV with `ares-setup-device`.
4. Package and install the IPK with `ares-install`, then launch with `ares-launch`.

Use LG's current webOS CLI documentation for exact device setup commands.

## Notes

- The app fetches `playlist.m3u` from the repository's raw GitHub URL at launch and on Refresh.
- Remote navigation uses standard focusable buttons and keyboard events.
- Video playback relies on the TV's native HTML5 media stack. Stream compatibility depends on codec, container, HLS profile, DRM, and source availability.
- Automatic bitrate switching works only when the stream itself provides a compatible adaptive HLS/DASH master playlist and the TV media stack supports it.
- Favourites are stored locally in webOS browser storage.
- This initial build is a development package, not an LG Content Store release. Store distribution requires LG's submission and approval process.
