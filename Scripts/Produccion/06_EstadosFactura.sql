-- =====================================================
-- Script de Estados de Factura
-- Fecha: 2024-01-15
-- Descripción: Inserciones de estados para facturas, notas de crédito y débito
-- =====================================================

-- Estados para Facturas
INSERT INTO EstadoFactura (Id, Nombre, Descripcion, Codigo, PermiteAnulacion, EsEstadoFinal, Activo, FechaCreacion, FechaLog, UserLog) VALUES
-- Estados de Facturas
('estado_factura_emitida', 'Factura Emitida', 'Factura emitida correctamente y válida', 'EMITIDA', 1, 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_factura_pendiente', 'Factura Pendiente', 'Factura en proceso de emisión', 'PENDIENTE', 1, 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_factura_error', 'Error en Emisión', 'Error durante la emisión de la factura', 'ERROR', 0, 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_factura_anulada', 'Factura Anulada', 'Factura anulada por el usuario', 'ANULADA', 0, 1, 1, GETDATE(), GETDATE(), 'Sistema'),

-- Estados para Notas de Crédito
('estado_nota_credito_emitida', 'Nota de Crédito Emitida', 'Nota de crédito emitida correctamente', 'NOTA_CREDITO_EMITIDA', 0, 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_nota_credito_pendiente', 'Nota de Crédito Pendiente', 'Nota de crédito en proceso de emisión', 'NOTA_CREDITO_PENDIENTE', 1, 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_nota_credito_error', 'Error en Nota de Crédito', 'Error durante la emisión de la nota de crédito', 'NOTA_CREDITO_ERROR', 0, 0, 1, GETDATE(), GETDATE(), 'Sistema'),

-- Estados para Notas de Débito
('estado_nota_debito_emitida', 'Nota de Débito Emitida', 'Nota de débito emitida correctamente', 'NOTA_DEBITO_EMITIDA', 0, 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_nota_debito_pendiente', 'Nota de Débito Pendiente', 'Nota de débito en proceso de emisión', 'NOTA_DEBITO_PENDIENTE', 1, 0, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_nota_debito_error', 'Error en Nota de Débito', 'Error durante la emisión de la nota de débito', 'NOTA_DEBITO_ERROR', 0, 0, 1, GETDATE(), GETDATE(), 'Sistema'),

-- Estados para Comprobantes No Fiscales
('estado_remito_emitido', 'Remito Emitido', 'Remito emitido correctamente', 'REMITO_EMITIDO', 0, 1, 1, GETDATE(), GETDATE(), 'Sistema'),
('estado_presupuesto_emitido', 'Presupuesto Emitido', 'Presupuesto emitido correctamente', 'PRESUPUESTO_EMITIDO', 0, 1, 1, GETDATE(), GETDATE(), 'Sistema');

-- =====================================================
-- DESCRIPCIÓN DE ESTADOS:
-- =====================================================
-- FACTURAS:
-- - EMITIDA: Factura emitida correctamente, permite anulación
-- - PENDIENTE: En proceso de emisión, permite anulación
-- - ERROR: Error en emisión, no permite anulación
-- - ANULADA: Factura anulada, estado final

-- NOTAS DE CRÉDITO:
-- - NOTA_CREDITO_EMITIDA: Emitida correctamente, estado final
-- - NOTA_CREDITO_PENDIENTE: En proceso, permite anulación
-- - NOTA_CREDITO_ERROR: Error en emisión

-- NOTAS DE DÉBITO:
-- - NOTA_DEBITO_EMITIDA: Emitida correctamente, estado final
-- - NOTA_DEBITO_PENDIENTE: En proceso, permite anulación
-- - NOTA_DEBITO_ERROR: Error en emisión

-- COMPROBANTES NO FISCALES:
-- - REMITO_EMITIDO: Remito emitido, estado final
-- - PRESUPUESTO_EMITIDO: Presupuesto emitido, estado final
-- =====================================================

-- Verificar los estados insertados
SELECT Id, Nombre, Codigo, PermiteAnulacion, EsEstadoFinal, Activo 
FROM EstadoFactura 
WHERE Activo = 1
ORDER BY Nombre;
