; -------------------------------
; Start

;copy translations start
Unicode true
;copy translations end

  !define MUI_FILE "MaximizeAlways"
  !define MUI_VERSION ""
  !define MUI_PRODUCT "Maximize Always"
  !define PRODUCT_SHORTCUT "MaximizeAlways"
  !define MUI_ICON "maximize-always-48.ico"
  !define PRODUCT_VERSION "1.3"
  
;  !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\readme.txt"

  !define MUI_CUSTOMFUNCTION_GUIINIT myGuiInit

  !include "MUI2.nsh"
  !include Library.nsh

  BrandingText "www.4dots-software.com"

  Name "Maximize Always"
  OutFile "MaximizeAlwaysSetup.exe" 
;  InstallDir "$LOCALPROGRAMFILES\4dots Software Free Sitemap Creator"
  
  InstallDir "$PROGRAMFILES\4dots Software\${PRODUCT_SHORTCUT}"

  InstallDirRegKey HKCU "Software\4dots Software\MaximizeAlways" "InstallDir"

 ;copy translations start
  ;Show all languages, despite user's codepage
  !define MUI_LANGDLL_ALLLANGUAGES
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\4dots Software\${PRODUCT_SHORTCUT}" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
;copy translations end
  
 !define DOT_MAJOR "2"
 !define DOT_MINOR "0"
 !define DOT_MINOR_MINOR "50727"  

  var ALREADY_INSTALLED
  RequestExecutionLevel admin

 
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING 
;--------------------------------
;General
 
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "license_agreement.rtf" 
 ; !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY 
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

  Page custom DonatePage
  ;!define MUI_FINISHPAGE_RUN 
  ;!define MUI_FINISHPAGE_RUN_FUNCTION "OpenWebpageFunction"
  ;!define MUI_FINISHPAGE_RUN_TEXT "Open Application Webpage for Information" 
  !insertmacro MUI_PAGE_FINISH
  
;--------------------------------
;Languages
 
    ;copy translations start 
  !insertmacro MUI_LANGUAGE "English" ; The first language is the default language
  !insertmacro MUI_LANGUAGE "French"
  !insertmacro MUI_LANGUAGE "German"
  !insertmacro MUI_LANGUAGE "Spanish"
  !insertmacro MUI_LANGUAGE "SpanishInternational"
  !insertmacro MUI_LANGUAGE "SimpChinese"
  !insertmacro MUI_LANGUAGE "TradChinese"
  !insertmacro MUI_LANGUAGE "Japanese"
  !insertmacro MUI_LANGUAGE "Korean"
  !insertmacro MUI_LANGUAGE "Italian"
  !insertmacro MUI_LANGUAGE "Dutch"
  !insertmacro MUI_LANGUAGE "Danish"
  !insertmacro MUI_LANGUAGE "Swedish"
  !insertmacro MUI_LANGUAGE "Norwegian"
  !insertmacro MUI_LANGUAGE "NorwegianNynorsk"
  !insertmacro MUI_LANGUAGE "Finnish"
  !insertmacro MUI_LANGUAGE "Greek"
  !insertmacro MUI_LANGUAGE "Russian"
  !insertmacro MUI_LANGUAGE "Portuguese"
  !insertmacro MUI_LANGUAGE "PortugueseBR"
  !insertmacro MUI_LANGUAGE "Polish"
  !insertmacro MUI_LANGUAGE "Ukrainian"
  !insertmacro MUI_LANGUAGE "Czech"
  !insertmacro MUI_LANGUAGE "Slovak"
  !insertmacro MUI_LANGUAGE "Croatian"
  !insertmacro MUI_LANGUAGE "Bulgarian"
  !insertmacro MUI_LANGUAGE "Hungarian"
  !insertmacro MUI_LANGUAGE "Thai"
  !insertmacro MUI_LANGUAGE "Romanian"
  !insertmacro MUI_LANGUAGE "Latvian"
  !insertmacro MUI_LANGUAGE "Macedonian"
  !insertmacro MUI_LANGUAGE "Estonian"
  !insertmacro MUI_LANGUAGE "Turkish"
  !insertmacro MUI_LANGUAGE "Lithuanian"
  !insertmacro MUI_LANGUAGE "Slovenian"
  !insertmacro MUI_LANGUAGE "Serbian"
  !insertmacro MUI_LANGUAGE "SerbianLatin"
  !insertmacro MUI_LANGUAGE "Arabic"
  !insertmacro MUI_LANGUAGE "Farsi"
  !insertmacro MUI_LANGUAGE "Hebrew"
  !insertmacro MUI_LANGUAGE "Indonesian"
  !insertmacro MUI_LANGUAGE "Mongolian"
  !insertmacro MUI_LANGUAGE "Luxembourgish"
  !insertmacro MUI_LANGUAGE "Albanian"
  !insertmacro MUI_LANGUAGE "Breton"
  !insertmacro MUI_LANGUAGE "Belarusian"
  !insertmacro MUI_LANGUAGE "Icelandic"
  !insertmacro MUI_LANGUAGE "Malay"
  !insertmacro MUI_LANGUAGE "Bosnian"
  !insertmacro MUI_LANGUAGE "Kurdish"
  !insertmacro MUI_LANGUAGE "Irish"
  !insertmacro MUI_LANGUAGE "Uzbek"
  !insertmacro MUI_LANGUAGE "Galician"
  !insertmacro MUI_LANGUAGE "Afrikaans"
  !insertmacro MUI_LANGUAGE "Catalan"
  !insertmacro MUI_LANGUAGE "Esperanto"
  !insertmacro MUI_LANGUAGE "Asturian"
  !insertmacro MUI_LANGUAGE "Basque"
  !insertmacro MUI_LANGUAGE "Pashto"
  !insertmacro MUI_LANGUAGE "ScotsGaelic"
  !insertmacro MUI_LANGUAGE "Georgian"
  !insertmacro MUI_LANGUAGE "Vietnamese"
  !insertmacro MUI_LANGUAGE "Welsh"
  !insertmacro MUI_LANGUAGE "Armenian"
  !insertmacro MUI_LANGUAGE "Corsican"
  !insertmacro MUI_LANGUAGE "Tatar"

	!insertmacro MUI_RESERVEFILE_LANGDLL
;copy translations end
 
;-------------------------------- 
;Installer Sections     

Section "install" installinfo
;Add files
   SetShellVarContext current

  SetOutPath "$INSTDIR"
;  inetc::get /end /SILENT "http://www.4dots-software.com/installmonetizer/minimizetotraytool.php" "$PLUGINSDIR\Installmanager.exe" /end
  ;ExecWait "$PLUGINSDIR\Installmanager.exe 101"
 
  File "MaximizeAlways\bin\debug\CryptoObfuscator_Output\${MUI_FILE}.exe"
  File "MaximizeAlways\bin\debug\MaximizeAlwaysConfig.exe"
  ;File "MaximizeAlwaysConfig\bin\debug\MaximizeAlwaysConfig.exe"
  ;File "readme.txt"
  ;File "manual\MaximizeAlways Help Manual.chm"
  File "MaximizeAlways\bin\debug\ddb.dat"
  File "MaximizeAlways\bin\debug\CryptoObfuscator_Output\MaximizeAlwaysAdminRights.exe"
  File "MaximizeAlways\bin\debug\MaximizeAlwaysLauncher.exe"
  ;File "C:\code\Misc\=Redist\dotNetFx45_Full_setup.exe"    
  ;File "c:\code\misc\=redist\luminati\lum_sdk.dll"
  ;File "c:\code\misc\=redist\luminati\lum_sdk32.dll"
  ;File "c:\code\misc\=redist\luminati\net_updater32.exe"
  ;File /oname=LuminatiHelper.exe "c:\code\misc\=redist\luminati\LuminatiHelper.exe"
  ;File "minimize-to-tray-tool-150.png"
 
 Call IsDotNetInstalledAdv
 
 ;Call CheckForDotVersion45Up
  ;Pop $0
  
  ;${If} $0 == 0
  ;ExecWait '"$INSTDIR\dotNetFx45_Full_setup.exe"  /quiet /norestart'  
  ;ExecWait '"$INSTDIR\dotNetFx45_Full_setup.exe"'
  ;ExecWait '"$INSTDIR\NDP452-KB2901907-x86-x64-AllOS-ENU.exe"'
  ;${EndIf}  
  
  ;nsExec::Exec "$INSTDIR\net_updater32.exe --install win_minimizetotraytool.4dotssoftware.com"
 
;create desktop shortcut
  CreateShortCut "$DESKTOP\${PRODUCT_SHORTCUT}.lnk" "$INSTDIR\${MUI_FILE}Launcher.exe"  "" "$INSTDIR\${MUI_FILE}.exe" 0
 
;create start-menu items
  CreateDirectory "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}"
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
  ;CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\MaximizeAlways Help.lnk" "$INSTDIR\MaximizeAlways Help Manual.chm" "" "$INSTDIR\MaximizeAlways Help Manual.chm" 0
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\MaximizeAlways Config.lnk" "$INSTDIR\MaximizeAlwaysConfig.exe" "" "$INSTDIR\MaximizeAlwaysConfig.exe" 0
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\${PRODUCT_SHORTCUT}.lnk" "$INSTDIR\${MUI_FILE}Launcher.exe" "" "$INSTDIR\${MUI_FILE}.exe" 0
 
;write uninstall information to the registry
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "DisplayName" "${PRODUCT_SHORTCUT} (remove only)"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "DisplayIcon" "$INSTDIR\${MUI_FILE}.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "Publisher" "4dots Software"   
 ;Store installation folder
 
  WriteRegStr HKCU "Software\4dots Software\MaximizeAlways" "InstallDir" $INSTDIR
  WriteRegStr HKCU "Software\4dots Software\MaximizeAlways\Applications" "" ""
 
  WriteUninstaller "$INSTDIR\Uninstall.exe"
 
 ; inetc::get /SILENT "http://www.4dots-software.com/dolog/dolog.php?request_file=${PRODUCT_SHORTCUT}" "$PLUGINSDIR\temptmp.txt"  /end

SectionEnd
 
 
;--------------------------------    
;Uninstaller Section  
Section "Uninstall"

   SetShellVarContext current 
   
  ExecWait "$INSTDIR\${MUI_FILE}.exe /uninstall"  

  ;nsExec::Exec "$INSTDIR\net_updater32.exe --uninstall win_minimizetotraytool.4dotssoftware.com" 
  
;Delete Files 
  RMDir /r "$INSTDIR\*.*"    
 
;Remove the installation directory

  RMDir "$INSTDIR"
 
;Delete Start Menu Shortcuts
  Delete "$DESKTOP\${PRODUCT_SHORTCUT}.lnk"
  Delete "$DESKTOP\MaximizeAlways Config.lnk"

  Delete "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\*.*"
  RmDir  "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}"
 
;Delete Uninstaller And Unistall Registry Entries
  DeleteRegKey HKCU "SOFTWARE\4dots Software\${PRODUCT_SHORTCUT}\Applications"
  DeleteRegKey HKCU "SOFTWARE\4dots Software\${PRODUCT_SHORTCUT}"

  DeleteRegKey HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}"  
  DeleteRegKey /ifempty HKCU "Software\4dots Software\${MUI_PRODUCT}"
 
SectionEnd
 
 
;--------------------------------    
;MessageBox Section
 
 
Function .onInstSuccess
 ; MessageBox MB_OK "You have successfully installed ${MUI_PRODUCT}. Use the desktop icon to start the program."
 ExecShell "" "https://www.4dots-software.com/maximizealways/?afterinstall=true&version=${PRODUCT_VERSION}";
 
FunctionEnd
  

 
Function un.onUninstSuccess
  MessageBox MB_OK "You have successfully uninstalled ${MUI_PRODUCT}."
FunctionEnd
 


Function myGUIInit
 StrCpy $INSTDIR "$PROGRAMFILES\4dots Software\${PRODUCT_SHORTCUT}"
FunctionEnd
 
Function OpenWebpageFunction
  ExecShell "" "http://www.4dots-software.com/maximizealways/?afterinstall=true"
FunctionEnd

;eof

; Usage
; Define in your script two constants:
;   DOT_MAJOR "(Major framework version)"
;   DOT_MINOR "{Minor framework version)"
;   DOT_MINOR_MINOR "{Minor framework version - last number after the second dot)"
; 
; Call IsDotNetInstalledAdv
; This function will abort the installation if the required version 
; or higher version of the .NET Framework is not installed.  Place it in
; either your .onInit function or your first install section before 
; other code.
Function IsDotNetInstalledAdv
   Push $0
   Push $1
   Push $2
   Push $3
   Push $4
   Push $5
 
  StrCpy $0 "0"
  StrCpy $1 "SOFTWARE\Microsoft\.NETFramework" ;registry entry to look in.
  StrCpy $2 0
 
  StartEnum:
    ;Enumerate the versions installed.
    EnumRegKey $3 HKLM "$1\policy" $2
 
    ;If we don't find any versions installed, it's not here.
    StrCmp $3 "" noDotNet notEmpty
 
    ;We found something.
    notEmpty:
      ;Find out if the RegKey starts with 'v'.  
      ;If it doesn't, goto the next key.
      StrCpy $4 $3 1 0
      StrCmp $4 "v" +1 goNext
      StrCpy $4 $3 1 1
 
      ;It starts with 'v'.  Now check to see how the installed major version
      ;relates to our required major version.
      ;If it's equal check the minor version, if it's greater, 
      ;we found a good RegKey.
      IntCmp $4 ${DOT_MAJOR} +1 goNext yesDotNetReg
      ;Check the minor version.  If it's equal or greater to our requested 
      ;version then we're good.
      StrCpy $4 $3 1 3
      IntCmp $4 ${DOT_MINOR} +1 goNext yesDotNetReg
 
      ;detect sub-version - e.g. 2.0.50727
      ;takes a value of the registry subkey - it contains the small version number
      EnumRegValue $5 HKLM "$1\policy\$3" 0
 
      IntCmpU $5 ${DOT_MINOR_MINOR} yesDotNetReg goNext yesDotNetReg
 
    goNext:
      ;Go to the next RegKey.
      IntOp $2 $2 + 1
      goto StartEnum
 
  yesDotNetReg: 
    ;Now that we've found a good RegKey, let's make sure it's actually
    ;installed by getting the install path and checking to see if the 
    ;mscorlib.dll exists.
    EnumRegValue $2 HKLM "$1\policy\$3" 0
    ;$2 should equal whatever comes after the major and minor versions 
    ;(ie, v1.1.4322)
    StrCmp $2 "" noDotNet
    ReadRegStr $4 HKLM $1 "InstallRoot"
    ;Hopefully the install root isn't empty.
    StrCmp $4 "" noDotNet
    ;build the actuall directory path to mscorlib.dll.
    StrCpy $4 "$4$3.$2\mscorlib.dll"
    IfFileExists $4 yesDotNet noDotNet
 
  noDotNet:
    ;Nope, something went wrong along the way.  Looks like the 
    ;proper .NET Framework isn't installed.  
 
    ;Uncomment the following line to make this function throw a message box right away 
   MessageBox MB_OK "You must have v${DOT_MAJOR}.${DOT_MINOR}.${DOT_MINOR_MINOR} or greater of the .NET Framework installed.$\n$\nPlease download and install the .NET Redistributable from the Webpage that will open and run ${MUI_FILE}Setup.exe once again !"

  ${If} ${RunningX64}

	ExecShell "open" "http://www.microsoft.com/downloads/details.aspx?FamilyID=b44a0000-acf8-4fa1-affb-40e78d788b00"
  ${Else}

	ExecShell "open" "http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5"
  ${EndIf}  




    Abort
     StrCpy $0 0
     Goto done
 
     yesDotNet:
    ;Everything checks out.  Go on with the rest of the installation.
    StrCpy $0 1
 
   done:
     Pop $4
     Pop $3
     Pop $2
     Pop $1
     Exch $0
 FunctionEnd
 
 Function DonatePage
   File /oname=$PLUGINSDIR\paypal-donate.bmp "C:\code\Misc\paypal-donate.bmp"
   
   Push $0
   Push $1

   !insertmacro MUI_HEADER_TEXT "Please Donate !" "Your donations are absolutely essential to us !"  
   nsDialogs::Create 1018
   Pop $0
   SetCtlColors $0 "" F0F0F0

   ${NSD_CreateBitmap} 150 50 73 44 ""
   Pop $0
   ${NSD_SetImage} $0 $PLUGINSDIR\\paypal-donate.bmp $1
   Push $1

   ; Register handler for click events
   ${NSD_OnClick} $0 DonateWebpage

   ${NSD_CreateLink} 50 120 100% 12 "Please Donate ! You donations are absolutely essential to us !"
   Pop $0
   ${NSD_OnClick} $0 DonateWebpage     
   
   nsDialogs::Show

   Pop $1
   ${NSD_FreeImage} $1

   Pop $1
   Pop $0 

FunctionEnd

Function .onInit
 
  ;copy translations start
  !insertmacro MUI_LANGDLL_DISPLAY
  ;copy translations end
  
FunctionEnd
 
Function DonateWebpage
	ExecShell "" "http://www.4dots-software.com/donate.php" 
FunctionEnd

;copy translations start
Function un.onInit

  !insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd
;copy translations end
;eof