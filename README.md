<p align="center">
	<img src="Assets/DSD.ico" alt="DesktopStreamDownloader Icon" width="120" />
</p>
<h3 align="center">DesktopStreamDownloader</h3>
<p align="center"><em>YT-DLP-based Search/Download GUI for Windows XP+</em></p>

## About

DesktopStreamDownloader is a desktop application designed to search and download videos via [yt-dlp](https://github.com/nicolaasjan/yt-dlp). The application is written targeting .NET 3.5 and Windows XP.


## Requirements

- Windows XP or later
- .NET Framework 3.5
- cURL, YT-DLP, and FFmpeg executables (included with release build, not with source code)

## Building

To build the release version of the project, place the required executables (`curl.exe`, `curl-ca-bundle.crt`, `yt-dlp.exe`, `yt-dlp-xp.exe`, and `ffmpeg.exe`) in the `deps` folder and use the `build.bat` script (Visual Studio/Visual Studio Build Tools/MSBuild required). The application uses `yt-dlp-xp.exe` on Windows XP and `yt-dlp.exe` otherwise. If [NSIS](https://nsis.sourceforge.io/) is installed, `build.bat` also produces `DesktopStreamDownloader Setup.exe`.

Alternatively, the project can be built directly in Visual Studio; however, the needed dependencies won't be packaged.

## Usage

The application will prompt you for a download folder when first launched. Change it later with Browse on the Downloads tab.

- Search is performed in the Search tab, and downloads are managed in the Downloads tab.
- Downloads can be added to the download queue using the "Add to Queue" button.
- Options can be changed using the options button in the Downloads tab

## Options

Options are available from the Options button on the Downloads tab:

- **Default Quality** - Preferred download resolution to be passed into YT-DLP (`144p`, `240p`, `360p`, `480p`, `720p`, or `1080p`; default `480p`)

## Credits

- cURL builds (for TLS on legacy systems) are provided by [LoRd_MuldeR](https://github.com/lordmulder)
- YT-DLP builds for Windows XP are provided by [nicolaasjan](https://github.com/nicolaasjan)
- FFmpeg builds for Windows XP are provided by [sherpya](https://sourceforge.net/projects/mplayer-win32/)

## License

DesktopStreamDownloader is licensed under the MIT license. For third party executables, see licenses under THIRD_PARTY_LICENSES.
