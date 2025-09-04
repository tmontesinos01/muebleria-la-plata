-- =====================================================
-- Script de Configuración para AFIP
-- Fecha: 2024-01-15
-- Descripción: Configuraciones necesarias para facturación AFIP
-- =====================================================

-- Configuración de datos de la empresa para AFIP
INSERT INTO Configuracion (Id, Clave, Valor, Descripcion, Activo, FechaCreacion, FechaLog, UserLog) VALUES
('empresa_cuit', 'EMPRESA_CUIT', 'REEMPLAZAR_CON_CUIT_EMPRESA', 'CUIT de la empresa para facturación AFIP', 1, GETDATE(), GETDATE(), 'Sistema'),
('empresa_razon_social', 'EMPRESA_RAZON_SOCIAL', 'REEMPLAZAR_CON_RAZON_SOCIAL', 'Razón social de la empresa', 1, GETDATE(), GETDATE(), 'Sistema'),
('empresa_domicilio', 'EMPRESA_DOMICILIO', 'REEMPLAZAR_CON_DOMICILIO', 'Domicilio fiscal de la empresa', 1, GETDATE(), GETDATE(), 'Sistema'),
('punto_venta_default', 'PUNTO_VENTA_DEFAULT', '0001', 'Punto de venta por defecto para facturación', 1, GETDATE(), GETDATE(), 'Sistema'),
('condicion_iva_empresa', 'CONDICION_IVA_EMPRESA', '1', 'Condición IVA de la empresa (1=Responsable Inscripto)', 1, GETDATE(), GETDATE(), 'Sistema');

-- =====================================================
-- INSTRUCCIONES DE USO:
-- =====================================================
-- 1. Reemplazar los valores 'REEMPLAZAR_CON_*' con los datos reales de tu empresa
-- 2. Verificar que el CUIT sea correcto (11 dígitos)
-- 3. Asegurarse de que el punto de venta esté habilitado en AFIP
-- 4. Ejecutar este script en la base de datos de producción
-- =====================================================

-- Verificar las configuraciones insertadas
SELECT Clave, Valor, Descripcion, Activo 
FROM Configuracion 
WHERE Clave IN ('EMPRESA_CUIT', 'EMPRESA_RAZON_SOCIAL', 'EMPRESA_DOMICILIO', 'PUNTO_VENTA_DEFAULT', 'CONDICION_IVA_EMPRESA')
ORDER BY Clave;
