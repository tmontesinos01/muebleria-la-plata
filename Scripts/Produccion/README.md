# Scripts de Producción - Mueblería La Plata

Esta carpeta contiene los scripts SQL necesarios para configurar la aplicación en producción.

## 📁 Estructura de Archivos

### 01_Configuracion_TusFacturasAPP.sql
- **Propósito**: Configurar las credenciales para la integración con TusFacturasAPP
- **Contenido**: 
  - TUSFACTURAS_USER_TOKEN
  - TUSFACTURAS_API_KEY
  - TUSFACTURAS_API_TOKEN
  - TUSFACTURAS_BASE_URL

### 02_Configuracion_AFIP.sql
- **Propósito**: Configurar los datos de la empresa para facturación AFIP
- **Contenido**:
  - EMPRESA_CUIT
  - EMPRESA_RAZON_SOCIAL
  - EMPRESA_DOMICILIO
  - PUNTO_VENTA_DEFAULT
  - CONDICION_IVA_EMPRESA

### 04_TiposComprobantes_AFIP.sql
- **Propósito**: Insertar tipos de comprobantes válidos para AFIP
- **Contenido**:
  - Facturas (A, B, C, E, M, MiPyme)
  - Notas de Crédito (A, B, C, E, MiPyme)
  - Notas de Débito (A, B, C, E)
  - Ticket (para consumidor final)
  - Comprobantes especiales (Recibo, Presupuesto, Remito)

## 🚀 Instrucciones de Despliegue

### 1. Preparación
- [ ] Obtener credenciales de TusFacturasAPP
- [ ] Verificar datos de la empresa
- [ ] Confirmar punto de venta habilitado en AFIP

### 2. Configuración
- [ ] Reemplazar valores placeholder en los scripts
- [ ] Ejecutar scripts en orden numérico (01, 02, 04)
- [ ] Verificar que las configuraciones se insertaron correctamente
- [ ] Confirmar que los tipos de comprobantes están disponibles

### 3. Validación
- [ ] Probar endpoint de validación de clientes
- [ ] Verificar conectividad con TusFacturasAPP
- [ ] Confirmar que las credenciales funcionan

## ⚠️ Importante

- **NUNCA** committear credenciales reales al repositorio
- Usar variables de entorno o archivos de configuración seguros
- Mantener backups de las configuraciones
- Documentar cualquier cambio en las credenciales

## 📞 Soporte

Para dudas sobre la configuración, contactar al equipo de desarrollo.
