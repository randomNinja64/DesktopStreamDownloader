; DesktopStreamDownloader NSIS installer
; Built by build.bat when makensis is available.

!define PRODUCT_NAME "DesktopStreamDownloader"
!define PRODUCT_VERSION "1.4.0"
!define PRODUCT_PUBLISHER "randomNinja64"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\DesktopStreamDownloader.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

!ifndef DIST_DIR
  !define DIST_DIR "..\DesktopStreamDownloader"
!endif
!ifndef SETUP_OUT
  !define SETUP_OUT "..\DesktopStreamDownloader Setup.exe"
!endif

SetCompressor lzma

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "${SETUP_OUT}"
LoadLanguageFile "${NSISDIR}\Contrib\Language files\English.nlf"
InstallDir "$PROGRAMFILES\DesktopStreamDownloader"
Icon "..\Assets\DSD.ico"
UninstallIcon "..\Assets\DSD.ico"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
DirText "Setup will install $(^Name) in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder."
LicenseText "If you accept all the terms of the agreement, choose I Agree to continue. You must accept the agreement to install $(^Name)."
LicenseData "..\LICENSE"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "${DIST_DIR}\DesktopStreamDownloader.exe"
  File "${DIST_DIR}\DesktopStreamDownloader.exe.config"
  File "${DIST_DIR}\Newtonsoft.Json.dll"
  File /nonfatal "${DIST_DIR}\curl.exe"
  File /nonfatal "${DIST_DIR}\curl-ca-bundle.crt"
  File /nonfatal "${DIST_DIR}\yt-dlp.exe"
  File /nonfatal "${DIST_DIR}\yt-dlp-xp.exe"
  File /nonfatal "${DIST_DIR}\ffmpeg.exe"
  File /nonfatal /r "${DIST_DIR}\THIRD_PARTY_LICENSES"
  CreateDirectory "$SMPROGRAMS\DesktopStreamDownloader"
  CreateShortCut "$SMPROGRAMS\DesktopStreamDownloader\DesktopStreamDownloader.lnk" "$INSTDIR\DesktopStreamDownloader.exe"
  CreateShortCut "$DESKTOP\DesktopStreamDownloader.lnk" "$INSTDIR\DesktopStreamDownloader.exe"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\DesktopStreamDownloader\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\DesktopStreamDownloader.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\DesktopStreamDownloader.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\DesktopStreamDownloader.exe"
  Delete "$INSTDIR\DesktopStreamDownloader.exe.config"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\curl.exe"
  Delete "$INSTDIR\curl-ca-bundle.crt"
  Delete "$INSTDIR\yt-dlp.exe"
  Delete "$INSTDIR\yt-dlp-xp.exe"
  Delete "$INSTDIR\ffmpeg.exe"
  RMDir /r "$INSTDIR\THIRD_PARTY_LICENSES"

  Delete "$SMPROGRAMS\DesktopStreamDownloader\Uninstall.lnk"
  Delete "$DESKTOP\DesktopStreamDownloader.lnk"
  Delete "$SMPROGRAMS\DesktopStreamDownloader\DesktopStreamDownloader.lnk"

  RMDir "$SMPROGRAMS\DesktopStreamDownloader"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd
