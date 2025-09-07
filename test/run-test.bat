@echo off
echo ========================================
echo   SISTEMA DE PRUEBAS - MUEBLERIA LA PLATA
echo ========================================
echo.
echo Test completo del sistema de facturacion electronica AFIP/ARCA
echo.

REM Verificar que .NET esté instalado
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET no esta instalado o no esta en el PATH
    echo Por favor instala .NET 9.0 o superior
    exit /b 1
)

echo .NET detectado correctamente
echo.

echo Compilando proyecto de pruebas...
dotnet build test/TestProject/TestProject.csproj
if %errorlevel% neq 0 (
    echo ERROR: No se pudo compilar el proyecto
    exit /b 1
)

echo Proyecto compilado correctamente
echo.

echo.
echo ========================================
echo   INICIANDO TEST COMPLETO DEL SISTEMA
echo ========================================
echo.

REM Ejecutar el test
dotnet run --project test/TestProject/TestProject.csproj

echo.
echo ========================================
echo   TEST COMPLETADO
echo ========================================
echo.
echo Revisa los resultados arriba y verifica los datos en Firebase Studio
echo.
