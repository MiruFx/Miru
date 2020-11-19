@echo off

REM %USERPROFILE%\.dotnet\tools\miru.cmd

REM Bypass "Terminate Batch Job" prompt.	
if "%~1"=="-FIXED_CTRL_C" (
   REM Remove the -FIXED_CTRL_C parameter
   SHIFT
) ELSE (
   REM Run the batch with <NUL and -FIXED_CTRL_C
   CALL <NUL %0 -FIXED_CTRL_C %*
   GOTO :EOF
)

if "%~1"=="run-app" goto :run_app
if "%~1"=="run-test" goto :test
if "%~1"=="run-pagetest" goto :pagetest
if "%~1"=="new" goto :run_cli
if "%~1"=="@app" goto :run_at_app
if "%~1"=="@test" goto :run_at_test
if "%~1"=="@pagetest" goto :run_at_pagetest
if "%~1"=="--version" goto :run_version
if "%~1"=="-v" goto :run_version

goto :miru

:test
    set project="tests"
    goto :run_test

:pagetest
    set project="pagetests"
    goto :run_test

:miru    
    set project="app"
    goto :run_miru

:run_app:
    for /f "delims=" %%i in ('MiruCli app') do set project_dir=%%i
    dotnet run -p %project_dir% -- %~1 %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9
    exit /b 0
    
:run_miru
    for /f "delims=" %%i in ('MiruCli %project%') do set project_dir=%%i
    dotnet run -p %project_dir% --no-build -- miru %~1 %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9
    exit /b 0
    
:run_cli
    MiruCli %~1 %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9
    exit /b 0

:run_version
    MiruCli --version
    exit /b 0
    
:run_test
    for /f "delims=" %%i in ('MiruCli %project%') do set project_dir=%%i
    dotnet run -p %project_dir% -- %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9
    exit /b 0
    
:run_at_app
    for /f "delims=" %%i in ('MiruCli app') do set project_dir=%%i
    start /wait /b cmd /c "cd "%project_dir%" && %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9"
    exit /b 0
    
:run_at_test
    for /f "delims=" %%i in ('MiruCli tests') do set project_dir=%%i
    start /wait /b cmd /c "cd "%project_dir%" && %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9"
    exit /b 0
    
:run_at_pagetest
    for /f "delims=" %%i in ('MiruCli pagetests') do set project_dir=%%i
    start /wait /b cmd /c "cd "%project_dir%" && %~2 %~3 %~4 %~5 %~6 %~7 %~8 %~9"
    exit /b 0