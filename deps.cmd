@echo off
set NUGET_EXE=bin\nuget-bin\nuget.exe
set NUGET_BOOTSTRAPPER_EXE=bin\nuget-bin\nuget-bootstrap.exe
set PACKAGE_DIR=lib\packages
set SCRIPTS_DIR=tests\Beeline.Example\Scripts

if not exist %NUGET_EXE% (goto bootstrap) else (goto install)

:bootstrap
%NUGET_BOOTSTRAPPER_EXE%
move %NUGET_BOOTSTRAPPER_EXE% %NUGET_EXE%
move %NUGET_BOOTSTRAPPER_EXE%.old %NUGET_BOOTSTRAPPER_EXE%

:install
for /F %%C in ('dir /b /s packages.config') do %NUGET_EXE% install %%C -o %PACKAGE_DIR%

:copyjs
if not exist %SCRIPTS_DIR% mkdir %SCRIPTS_DIR%

cd %PACKAGE_DIR%
for /F %%S in ('dir /b /s *.js') do xcopy /y /c %%S ..\..\%SCRIPTS_DIR%\

if errorlevel 1 pause else exit
