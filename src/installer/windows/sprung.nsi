Name "Sprung"
Outfile "sprung_setup.exe"
SetCompressor lzma
InstallDir "$PROGRAMFILES\Sprung"

Var StartMenuFolder

!include 'MUI.nsh'

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU Sprung $StartMenuFolder
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

!define NETInstaller "dotnetfx30SP1setup.exe"
Section "Microsoft .NET Framework" SecFramework
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v3.0" NETFrameworkInstalled 0
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727" NETFrameworkInstalled 0
  File /oname=$TEMP\${NETInstaller} ${NETInstaller}
 
  DetailPrint "Starting Microsoft .NET Framework 3.0 Setup..."
  ExecWait "$TEMP\${NETInstaller}"
  Return
 
  NETFrameworkInstalled:
  DetailPrint "Microsoft .NET Framework is already installed"
 
SectionEnd
 
Section -
  SetOutPath $INSTDIR

  File /r "Sprung\*"

  !insertmacro MUI_STARTMENU_WRITE_BEGIN Sprung
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall Sprung.lnk" "$INSTDIR\uninstall.exe"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Sprung.lnk" "$INSTDIR\sprung.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
  
  CreateShortCut "$DESKTOP\Sprung.lnk" "$INSTDIR\sprung.exe"

  WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd

Section Uninstall
  # Always delete the uninstaller first
  Delete "$INSTDIR\uninstall.exe"

  RMDir /r "$INSTDIR"

  !insertmacro MUI_STARTMENU_GETFOLDER Sprung $StartMenuFolder
  RMDir /r "$SMPROGRAMS\$StartMenuFolder"

  Delete "$DESKTOP\Sprung.lnk"
SectionEnd
