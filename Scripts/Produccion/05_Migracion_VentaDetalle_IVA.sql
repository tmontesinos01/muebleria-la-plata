-- =====================================================
-- Script de Migración: Agregar campos IVA a VentaDetalle
-- Fecha: 2024-01-15
-- Descripción: Actualizar registros existentes de VentaDetalle con cálculos de IVA
-- =====================================================

-- IMPORTANTE: Este script debe ejecutarse después de actualizar la aplicación
-- para que los nuevos campos estén disponibles en la base de datos

-- Actualizar registros existentes de VentaDetalle
-- Establecer alícuota IVA por defecto (21%) y calcular IVA y Total
UPDATE VentaDetalle 
SET 
    AlicuotaIVA = 21,
    IVA = Subtotal * 0.21,
    Total = Subtotal * 1.21,
    FechaLog = GETDATE()
WHERE 
    Activo = 1 
    AND (AlicuotaIVA IS NULL OR AlicuotaIVA = 0)
    AND (IVA IS NULL OR IVA = 0)
    AND (Total IS NULL OR Total = 0);

-- Verificar la migración
SELECT 
    COUNT(*) as TotalRegistros,
    COUNT(CASE WHEN AlicuotaIVA = 21 THEN 1 END) as ConAlicuotaIVA,
    COUNT(CASE WHEN IVA > 0 THEN 1 END) as ConIVA,
    COUNT(CASE WHEN Total > 0 THEN 1 END) as ConTotal
FROM VentaDetalle 
WHERE Activo = 1;

-- Mostrar algunos ejemplos de la migración
SELECT TOP 10
    Id,
    IdVenta,
    IdProducto,
    Cantidad,
    PrecioUnitario,
    Subtotal,
    AlicuotaIVA,
    IVA,
    Total,
    FechaCreacion,
    FechaLog
FROM VentaDetalle 
WHERE Activo = 1
ORDER BY FechaCreacion DESC;

-- =====================================================
-- INSTRUCCIONES DE USO:
-- =====================================================
-- 1. Ejecutar este script DESPUÉS de desplegar la nueva versión de la aplicación
-- 2. Verificar que todos los registros se actualizaron correctamente
-- 3. Confirmar que los cálculos de IVA son correctos
-- 4. Realizar pruebas de facturación con datos existentes
-- =====================================================
