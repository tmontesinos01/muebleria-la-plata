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
    pause
    exit /b 1
)

echo .NET detectado correctamente
echo.

REM Verificar que el proyecto esté compilado
if not exist "bin\Debug\net9.0\TestProject.exe" (
    echo Compilando proyecto de pruebas...
    dotnet build
    if %errorlevel% neq 0 (
        echo ERROR: No se pudo compilar el proyecto
        pause
        exit /b 1
    )
)

echo Proyecto compilado correctamente
echo.

REM Verificar que el servidor esté ejecutándose
echo Verificando que el servidor este ejecutandose...
curl -s -o nul -w "%%{http_code}" https://localhost:7000/swagger
if %errorlevel% neq 0 (
    echo.
    echo ADVERTENCIA: No se pudo conectar al servidor en https://localhost:7000
    echo Asegurate de que el servidor este ejecutandose antes de continuar
    echo.
    set /p continuar="¿Deseas continuar de todos modos? (s/n): "
    if /i not "%continuar%"=="s" (
        echo Test cancelado por el usuario
        pause
        exit /b 0
    )
)

echo.
echo ========================================
echo   INICIANDO TEST COMPLETO DEL SISTEMA
echo ========================================
echo.

REM Ejecutar el test
dotnet run

echo.
echo ========================================
echo   TEST COMPLETADO
echo ========================================
echo.
echo Revisa los resultados arriba y verifica los datos en Firebase Studio
echo.
pause