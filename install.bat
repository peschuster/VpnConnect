@ECHO OFF
pushd %~dp0

SET iu="%windir%\Microsoft.NET\Framework\v4.0.30319\installutil.exe"

IF NOT EXIST "bin\VpnConnect.exe" call build.bat

echo.
AT > NUL
IF %ERRORLEVEL% EQU 0 (


    %iu% .\bin\VpnConnect.exe

) ELSE (
    ECHO You need Administrator rights to install a Windows Service.
    ECHO Open this script again using "Run as Administrator"
)

echo.
Pause
goto End

:End
popd