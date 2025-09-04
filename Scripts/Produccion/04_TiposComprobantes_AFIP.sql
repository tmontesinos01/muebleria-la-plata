-- =====================================================
-- Script de Tipos de Comprobantes AFIP
-- Fecha: 2024-01-15
-- Descripción: Inserciones de tipos de comprobantes válidos para AFIP
-- =====================================================

-- Tipos de comprobantes válidos para AFIP
-- Incluye todos los tipos principales y el ticket

INSERT INTO TipoComprobante (Id, Nombre, Abreviatura, Signo, Activo, FechaCreacion, FechaLog, UserLog) VALUES
-- FACTURAS
('factura_a', 'Factura A', 'FA', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('factura_b', 'Factura B', 'FB', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('factura_c', 'Factura C', 'FC', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('factura_e', 'Factura E', 'FE', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('factura_m', 'Factura M', 'FM', 1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- FACTURAS MIPYME
('factura_mipyme_a', 'Factura MiPyme A', 'FMA', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('factura_mipyme_b', 'Factura MiPyme B', 'FMB', 1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- NOTAS DE CRÉDITO
('nota_credito_a', 'Nota de Crédito A', 'NCA', -1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_credito_b', 'Nota de Crédito B', 'NCB', -1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_credito_c', 'Nota de Crédito C', 'NCC', -1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_credito_e', 'Nota de Crédito E', 'NCE', -1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_credito_mipyme_a', 'Nota de Crédito MiPyme A', 'NCMA', -1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- NOTAS DE DÉBITO
('nota_debito_a', 'Nota de Débito A', 'NDA', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_debito_b', 'Nota de Débito B', 'NDB', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_debito_c', 'Nota de Débito C', 'NDC', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('nota_debito_e', 'Nota de Débito E', 'NDE', 1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- TICKET (COMPROBANTE PARA CONSUMIDOR FINAL)
('ticket', 'Ticket', 'T', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('ticket_factura_b', 'Ticket Factura B', 'TFB', 1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- COMPROBANTES ESPECIALES
('recibo', 'Recibo', 'R', 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('presupuesto', 'Presupuesto', 'P', 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('remito', 'Remito', 'RE', 0, 1, GETDATE(), GETDATE(), 'Sistema');

-- =====================================================
-- DESCRIPCIÓN DE TIPOS DE COMPROBANTES:
-- =====================================================
-- FACTURAS:
-- - Factura A: Para Responsables Inscriptos (IVA 21%)
-- - Factura B: Para Consumidores Finales (sin IVA discriminado)
-- - Factura C: Para Exentos
-- - Factura E: Para Exportación
-- - Factura M: Para Monotributo
-- - Factura MiPyme: Para MiPyMEs (nuevo régimen)

-- NOTAS DE CRÉDITO:
-- - Anulan total o parcialmente facturas emitidas
-- - Signo: -1 (restan del total)

-- NOTAS DE DÉBITO:
-- - Incrementan el monto de facturas emitidas
-- - Signo: 1 (suman al total)

-- TICKET:
-- - Comprobante simplificado para consumidor final
-- - No requiere datos del comprador
-- - Límite según tope AFIP

-- COMPROBANTES ESPECIALES:
-- - Recibo: Para cobros
-- - Presupuesto: Para cotizaciones (Signo: 0)
-- - Remito: Para entregas (Signo: 0)
-- =====================================================

-- Verificar los tipos de comprobantes insertados
SELECT Id, Nombre, Abreviatura, Signo, Activo 
FROM TipoComprobante 
WHERE Activo = 1
ORDER BY Nombre;
