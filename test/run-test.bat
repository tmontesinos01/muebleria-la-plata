@echo off
echo ========================================
echo EJECUTANDO TEST DEL SISTEMA DE MUEBLERIA
echo ========================================
echo.

cd TestProject
echo Compilando proyecto de test...
dotnet build

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Fallo en la compilacion
    pause
    exit /b 1
)

echo.
echo Ejecutando test...
echo.
dotnet run

echo.
echo Test completado.
pause
