-- =====================================================
-- Script de Configuración para TusFacturasAPP
-- Fecha: 2024-01-15
-- Descripción: Inserciones necesarias para la integración con TusFacturasAPP
-- =====================================================

-- Configuración de credenciales para TusFacturasAPP
-- IMPORTANTE: Reemplazar los valores por las credenciales reales antes de ejecutar en producción

INSERT INTO Configuracion (Id, Clave, Valor, Descripcion, Activo, FechaCreacion, FechaLog, UserLog) VALUES
('tusfacturas_user_token', 'TUSFACTURAS_USER_TOKEN', 'REEMPLAZAR_CON_TOKEN_REAL', 'Token de usuario para TusFacturasAPP', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_api_key', 'TUSFACTURAS_API_KEY', 'REEMPLAZAR_CON_API_KEY_REAL', 'API Key para TusFacturasAPP', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_api_token', 'TUSFACTURAS_API_TOKEN', 'REEMPLAZAR_CON_API_TOKEN_REAL', 'API Token para TusFacturasAPP', 1, GETDATE(), GETDATE(), 'Sistema'),
('tusfacturas_base_url', 'TUSFACTURAS_BASE_URL', 'https://www.tusfacturas.app/app/api/v2', 'URL base de la API de TusFacturasAPP', 1, GETDATE(), GETDATE(), 'Sistema');

-- =====================================================
-- INSTRUCCIONES DE USO:
-- =====================================================
-- 1. Obtener las credenciales reales desde tu cuenta de TusFacturasAPP
-- 2. Reemplazar los valores 'REEMPLAZAR_CON_*_REAL' con las credenciales reales
-- 3. Ejecutar este script en la base de datos de producción
-- 4. Verificar que las configuraciones se insertaron correctamente
-- =====================================================

-- Verificar las configuraciones insertadas
SELECT Clave, Valor, Descripcion, Activo 
FROM Configuracion 
WHERE Clave IN ('TUSFACTURAS_USER_TOKEN', 'TUSFACTURAS_API_KEY', 'TUSFACTURAS_API_TOKEN', 'TUSFACTURAS_BASE_URL')
ORDER BY Clave;
