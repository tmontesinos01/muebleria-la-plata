# 🧪 TEST COMPLETO DEL SISTEMA - MUEBLERÍA LA PLATA

## 📋 DESCRIPCIÓN

Este test completo simula todo el proceso de facturación electrónica AFIP/ARCA del sistema de mueblería, desde la creación de productos hasta la emisión de comprobantes electrónicos.

## 🎯 OBJETIVOS

- ✅ **Validar funcionalidad completa** del sistema de facturación
- ✅ **Probar todos los endpoints** implementados
- ✅ **Verificar integración** con TusFacturasAPP
- ✅ **Confirmar almacenamiento** en Firebase/Firestore
- ✅ **Simular flujo real** de trabajo

## 🚀 FUNCIONALIDADES PROBADAS

### 📦 **Gestión de Productos**
- **Uso inteligente**: Usa productos existentes si están disponibles
- **Creación automática**: Crea productos faltantes si es necesario
- **Mínimo requerido**: 5 productos para el test completo
- Validación de datos

### 👥 **Gestión de Clientes**
- **Uso inteligente**: Usa clientes existentes si están disponibles
- **Creación automática**: Crea clientes faltantes si es necesario
- **Mínimo requerido**: 3 clientes para el test completo
- Validación de clientes en AFIP

### 🛒 **Simulación de Ventas**
- Simulación de ventas para facturación
- Datos preparados para emisión de comprobantes

### 🧾 **Facturación Electrónica**
- Emisión de Facturas A y B
- Emisión de Tickets
- Emisión de Tickets Factura B

### 📝 **Notas de Crédito/Débito**
- Emisión de notas de crédito
- Emisión de notas de débito
- Consulta de notas por factura

### ❌ **Anulación y Reimpresión**
- Anulación de facturas
- Reimpresión de facturas

## 📊 DATOS DE PRUEBA

### **Productos Creados (5)**
1. Mesa de Comedor - $50,000
2. Silla de Comedor - $15,000
3. Sofá 3 Cuerpos - $120,000
4. Mesa de Centro - $25,000
5. Lámpara de Pie - $18,000

### **Clientes Creados (3)**
1. Juan Pérez (DNI: 12345678)
2. María González (DNI: 87654321)
3. Empresa ABC S.A. (CUIT: 20123456789)

### **Ventas Simuladas (2)**
1. Venta Simulada 1: Mesa + 4 Sillas
2. Venta Simulada 2: Sofá + Mesa de Centro

### **Comprobantes Emitidos**
- 2 Facturas (A y B)
- 2 Tickets (estándar y factura B)
- 1 Nota de Crédito
- 1 Nota de Débito

## 🛠️ REQUISITOS

### **Sistema**
- .NET 9.0 o superior
- Servidor de la API ejecutándose
- Conexión a Firebase/Firestore
- Credenciales de TusFacturasAPP configuradas

### **Configuración**
- Base de datos con tipos de comprobantes insertados
- Configuración de TusFacturasAPP en la tabla Configuracion
- Punto de venta habilitado en AFIP

## 🚀 EJECUCIÓN

### **Método 1: Script Automático (Recomendado)**
```bash
# En Windows
run-test.bat

# En Linux/Mac
chmod +x run-test.sh
./run-test.sh
```

### **Método 2: Manual**
```bash
# Compilar
dotnet build

# Ejecutar
dotnet run

# Con URL personalizada
dotnet run https://localhost:7000
```

## 📋 FLUJO DE EJECUCIÓN

1. **Configuración**: Verifica conexión al servidor
2. **Productos**: Usa existentes o crea hasta 5 productos
3. **Clientes**: Usa existentes o crea hasta 3 clientes
4. **Validación AFIP**: Valida clientes en AFIP
5. **Ventas**: Simula 2 ventas para facturación
6. **Facturas**: Emite 2 facturas (A y B)
7. **Tickets**: Emite 2 tickets
8. **Notas**: Emite notas de crédito y débito
9. **Anulación**: Anula una factura
10. **Reimpresión**: Reimprime una factura
11. **Consulta**: Consulta todos los datos creados

## 🔍 VERIFICACIÓN EN FIREBASE STUDIO

### **Colecciones a Revisar**

#### **Productos**
- Verificar productos existentes o creados (mínimo 5)
- Revisar nombres y precios

#### **Clientes**
- Verificar clientes existentes o creados (mínimo 3)
- Revisar documentos y datos

#### **Ventas**
- Verificar ventas pendientes de facturación
- Revisar estructura de datos

#### **Facturas**
- Verificar facturas emitidas con CAE
- Revisar tickets emitidos
- Verificar notas de crédito/débito
- Revisar facturas anuladas

#### **Configuración**
- Verificar credenciales de TusFacturasAPP
- Revisar configuración de punto de venta

#### **Tipos de Comprobantes**
- Verificar que están todos los tipos configurados
- Revisar: facturas, notas, tickets

#### **Estados de Factura**
- Verificar estados configurados
- Revisar: EMITIDA, ANULADA, etc.

## 📊 RESULTADOS ESPERADOS

### **Éxito**
- ✅ Productos disponibles (existentes o creados)
- ✅ Clientes disponibles (existentes o creados)
- ✅ Todas las ventas simuladas
- ✅ Todas las facturas emitidas con CAE
- ✅ Todos los tickets emitidos
- ✅ Notas de crédito/débito emitidas
- ✅ Facturas anuladas correctamente
- ✅ Reimpresión exitosa

### **Fallos Comunes**
- ❌ Servidor no disponible
- ❌ Credenciales de TusFacturasAPP incorrectas
- ❌ Tipos de comprobantes no configurados
- ❌ Punto de venta no habilitado en AFIP
- ❌ Errores de conectividad con Firebase

## 🔧 SOLUCIÓN DE PROBLEMAS

### **Error: Servidor no disponible**
```bash
# Verificar que el servidor esté ejecutándose
curl https://localhost:7000/swagger

# Iniciar servidor si es necesario
cd ../WebApi
dotnet run
```

### **Error: Credenciales TusFacturasAPP**
- Verificar que las credenciales estén configuradas en Firebase
- Revisar tabla Configuracion
- Confirmar que las credenciales sean válidas

### **Error: Tipos de comprobantes**
- Ejecutar script `04_TiposComprobantes_AFIP.sql`
- Verificar que los tipos estén activos

### **Error: Punto de venta**
- Verificar configuración `PUNTO_VENTA_DEFAULT`
- Confirmar que esté habilitado en AFIP

## 📈 MÉTRICAS DE ÉXITO

| Componente | Esperado | Crítico |
|------------|----------|---------|
| **Productos** | 5+ | ✅ |
| **Clientes** | 3+ | ✅ |
| **Ventas Simuladas** | 2 | ✅ |
| **Facturas** | 2 | ✅ |
| **Tickets** | 2 | ✅ |
| **Notas** | 2 | ✅ |
| **Anulaciones** | 1 | ✅ |
| **Reimpresiones** | 1 | ✅ |

## 🎯 CONCLUSIÓN

Este test completo valida que el sistema de mueblería está **100% funcional** para facturación electrónica AFIP/ARCA. Si el test se ejecuta exitosamente, el sistema está listo para producción.

### **Próximos Pasos**
1. ✅ Ejecutar test completo
2. ✅ Verificar resultados en Firebase Studio
3. ✅ Corregir errores si los hay
4. ✅ Publicar sistema a producción