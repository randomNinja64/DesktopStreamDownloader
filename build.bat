@echo off
setlocal EnableExtensions
set ROOT=%~dp0
set OUT=%ROOT%DesktopStreamDownloader
set SETUP=%ROOT%DesktopStreamDownloader Setup.exe

:: Find MSBuild
for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set MSBUILD=%%i
if not defined MSBUILD (echo ERROR: MSBuild not found. & exit /b 1)

:: Build solution
echo Building solution ...
"%MSBUILD%" "%ROOT%DesktopStreamDownloader.sln" /p:Configuration=Release /m /v:minimal || exit /b %ERRORLEVEL%
echo.

:: Stage output
if exist "%OUT%" rmdir /s /q "%OUT%"
mkdir "%OUT%"

copy /Y "%ROOT%bin\Release\DesktopStreamDownloader.exe" "%OUT%\" >nul
copy /Y "%ROOT%bin\Release\DesktopStreamDownloader.exe.config" "%OUT%\" >nul
if exist "%ROOT%bin\Release\Newtonsoft.Json.dll" copy /Y "%ROOT%bin\Release\Newtonsoft.Json.dll" "%OUT%\" >nul

:: Optional deps from deps\ (WebTools set + ffmpeg for yt-dlp remux/recode)
echo Packaging optional dependencies ...
call :CopyIfExists "%ROOT%deps\curl.exe" "%OUT%\curl.exe"
call :CopyIfExists "%ROOT%deps\curl-ca-bundle.crt" "%OUT%\curl-ca-bundle.crt"
call :CopyIfExists "%ROOT%deps\yt-dlp.exe" "%OUT%\yt-dlp.exe"
call :CopyIfExists "%ROOT%deps\yt-dlp-xp.exe" "%OUT%\yt-dlp-xp.exe"
call :CopyIfExists "%ROOT%deps\ffmpeg.exe" "%OUT%\ffmpeg.exe"

:: Optional third-party license texts
call :CopyLicenses

:: Optional NSIS installer
call :BuildInstaller
if errorlevel 1 exit /b %ERRORLEVEL%

echo.
echo Done: %OUT%
if exist "%SETUP%" echo Installer: %SETUP%
exit /b 0

:CopyIfExists
if exist "%~1" (
  copy /Y "%~1" "%~2" >nul
  echo   packaged %~nx2
)
exit /b 0

:CopyLicenses
if not exist "%ROOT%THIRD_PARTY_LICENSES" exit /b 0
set "_any="
for %%F in ("%ROOT%THIRD_PARTY_LICENSES\*") do (
  if /I not "%%~nxF"==".gitkeep" set "_any=1"
)
if not defined _any exit /b 0
mkdir "%OUT%\THIRD_PARTY_LICENSES" 2>nul
for %%F in ("%ROOT%THIRD_PARTY_LICENSES\*") do (
  if /I not "%%~nxF"==".gitkeep" (
    copy /Y "%%F" "%OUT%\THIRD_PARTY_LICENSES\" >nul
    echo   packaged THIRD_PARTY_LICENSES\%%~nxF
  )
)
exit /b 0

:BuildInstaller
set "MAKENSIS="
if exist "%ProgramFiles(x86)%\NSIS\makensis.exe" set "MAKENSIS=%ProgramFiles(x86)%\NSIS\makensis.exe"
if not defined MAKENSIS if exist "%ProgramFiles%\NSIS\makensis.exe" set "MAKENSIS=%ProgramFiles%\NSIS\makensis.exe"
where makensis >nul 2>&1 && for /f "delims=" %%i in ('where makensis') do if not defined MAKENSIS set "MAKENSIS=%%i"
if not defined MAKENSIS (
  echo NSIS not found; skipping installer.
  exit /b 0
)
if not exist "%OUT%\DesktopStreamDownloader.exe" (
  echo ERROR: staged build missing DesktopStreamDownloader.exe
  exit /b 1
)
if not exist "%OUT%\Newtonsoft.Json.dll" (
  echo ERROR: staged build missing Newtonsoft.Json.dll
  exit /b 1
)
echo.
echo Building installer ...
"%MAKENSIS%" /V2 "/DDIST_DIR=%OUT%" "/DSETUP_OUT=%SETUP%" "%ROOT%installer\InstallScript.nsi"
if errorlevel 1 (
  echo ERROR: NSIS installer build failed.
  exit /b 1
)
echo   packaged DesktopStreamDownloader Setup.exe
exit /b 0
