#!/bin/bash

echo "========================================"
echo "EJECUTANDO TEST DEL SISTEMA DE MUEBLERIA"
echo "========================================"
echo

cd TestProject
echo "Compilando proyecto de test..."
dotnet build

if [ $? -ne 0 ]; then
    echo "ERROR: Fallo en la compilación"
    exit 1
fi

echo
echo "Ejecutando test..."
echo
dotnet run

echo
echo "Test completado."
