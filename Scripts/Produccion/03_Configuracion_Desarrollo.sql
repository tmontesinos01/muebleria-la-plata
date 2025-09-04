-- =====================================================
-- Script de Configuración para Desarrollo
-- Fecha: 2024-01-15
-- Descripción: Configuraciones para ambiente de desarrollo/testing
-- =====================================================

-- IMPORTANTE: Este script es solo para desarrollo/testing
-- NO usar en producción

-- Configuración de credenciales de prueba para TusFacturasAPP
INSERT INTO Configuracion (Id, Clave, Valor, Descripcion, Activo, FechaCreacion, FechaLog, UserLog) VALUES
('tusfacturas_user_token_dev', 'TUSFACTURAS_USER_TOKEN', 'TOKEN_DE_DESARROLLO', 'Token de usuario para TusFacturasAPP (DEV)', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_api_key_dev', 'TUSFACTURAS_API_KEY', 'API_KEY_DE_DESARROLLO', 'API Key para TusFacturasAPP (DEV)', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_api_token_dev', 'TUSFACTURAS_API_TOKEN', 'API_TOKEN_DE_DESARROLLO', 'API Token para TusFacturasAPP (DEV)', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_base_url_dev', 'TUSFACTURAS_BASE_URL', 'https://www.tusfacturas.app/app/api/v2', 'URL base de la API de TusFacturasAPP (DEV)', 1, GETDATE(), GETDATE(), 'Sistema');

-- Configuración de datos de prueba para AFIP
INSERT INTO Configuracion (Id, Clave, Valor, Descripcion, Activo, FechaCreacion, FechaLog, UserLog) VALUES
('empresa_cuit_dev', 'EMPRESA_CUIT', '20123456789', 'CUIT de prueba para desarrollo', 1, GETDATE(), GETDATE(), 'Sistema'),
('empresa_razon_social_dev', 'EMPRESA_RAZON_SOCIAL', 'EMPRESA DE PRUEBA S.A.', 'Razón social de prueba', 1, GETDATE(), GETDATE(), 'Sistema'),
('empresa_domicilio_dev', 'EMPRESA_DOMICILIO', 'Calle Falsa 123, La Plata, Buenos Aires', 'Domicilio de prueba', 1, GETDATE(), GETDATE(), 'Sistema'),
('punto_venta_default_dev', 'PUNTO_VENTA_DEFAULT', '0001', 'Punto de venta por defecto (DEV)', 1, GETDATE(), GETDATE(), 'Sistema'),
('condicion_iva_empresa_dev', 'CONDICION_IVA_EMPRESA', '1', 'Condición IVA de prueba (1=Responsable Inscripto)', 1, GETDATE(), GETDATE(), 'Sistema');

-- =====================================================
-- NOTAS IMPORTANTES:
-- =====================================================
-- 1. Este script es SOLO para desarrollo/testing
-- 2. Los valores son de prueba y NO funcionarán en producción
-- 3. Reemplazar con credenciales reales antes de usar en producción
-- 4. Usar el script 01_Configuracion_TusFacturasAPP.sql para producción
-- =====================================================

-- Verificar las configuraciones insertadas
SELECT Clave, Valor, Descripcion, Activo 
FROM Configuracion 
WHERE Clave IN ('TUSFACTURAS_USER_TOKEN', 'TUSFACTURAS_API_KEY', 'TUSFACTURAS_API_TOKEN', 'TUSFACTURAS_BASE_URL', 'EMPRESA_CUIT', 'EMPRESA_RAZON_SOCIAL', 'EMPRESA_DOMICILIO', 'PUNTO_VENTA_DEFAULT', 'CONDICION_IVA_EMPRESA')
ORDER BY Clave;
