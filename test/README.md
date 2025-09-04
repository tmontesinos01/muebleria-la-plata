# 🧪 Test del Sistema de Mueblería

Este test verifica que todo el sistema esté funcionando correctamente, incluyendo Firebase Firestore, Firebase Storage, y la creación de categorías y productos.

## 📋 Lo que hace el test:

1. **📁 Crea 2 categorías:**
   - PINO (Muebles de pino natural)
   - MELAMINA (Muebles de melamina)

2. **🛋️ Crea 3 productos:**
   - Mesa de Pino ($45,000) - Categoría PINO
   - Estantería Melamina ($25,000) - Categoría MELAMINA  
   - Silla de Pino ($15,000) - Categoría PINO

3. **🖼️ Sube imágenes:**
   - Asocia la imagen `test-image.png` a cada producto
   - Guarda las URLs en Firebase Storage

4. **🔍 Verifica los datos:**
   - Recupera los productos desde la base de datos
   - Verifica que las imágenes se subieron correctamente
   - Muestra todos los detalles de los productos creados

## 🚀 Cómo ejecutar el test:

### Opción 1: Script automático (Recomendado)

**Windows:**
```bash
cd test
run-test.bat
```

**Linux/Mac:**
```bash
cd test
./run-test.sh
```

### Opción 2: Manual

```bash
cd test/TestProject
dotnet build
dotnet run
```

## 📁 Archivos necesarios:

- `firebase-credentials.json` - Credenciales de Firebase (se copia automáticamente)
- `serviceAccount.json` - Cuenta de servicio (se copia automáticamente)
- `test-image.png` - Imagen de prueba (debe existir en la carpeta test)

## ✅ Resultado esperado:

Si todo funciona correctamente, verás:

```
=== INICIANDO PRUEBA COMPLETA DEL SISTEMA DE MUEBLERÍA ===
Verificando: Firebase Firestore, Storage, Categorías y Productos

📁 PASO 1: Creando categorías PINO y MELAMINA...
✅ Categoría PINO creada con ID: [ID]
✅ Categoría MELAMINA creada con ID: [ID]

🛋️ PASO 2: Creando 3 productos con imágenes...
✅ Producto 'Mesa de Pino' creado con ID: [ID]
✅ Producto 'Estantería Melamina' creado con ID: [ID]
✅ Producto 'Silla de Pino' creado con ID: [ID]

🖼️ PASO 3: Subiendo imágenes a los productos...
✅ Imagen subida para 'Mesa de Pino': [URL]
✅ Imagen subida para 'Estantería Melamina': [URL]
✅ Imagen subida para 'Silla de Pino': [URL]

🔍 PASO 4: Recuperando productos desde la base de datos...
📊 Total de productos en la base de datos: [N]
🖼️ Productos con imagen: [N]

📋 DETALLES DE PRODUCTOS CREADOS:
🛋️ Mesa de Pino
   ID: [ID]
   Descripción: Mesa de comedor de pino macizo
   Precio: $45,000.00
   Stock: 5
   Código: MESA-PINO-001
   Categoría ID: [ID]
   Imagen: ✅ Con imagen
   URL Imagen: [URL]

[... más productos ...]

📁 PASO 5: Verificando categorías creadas...
📊 Total de categorías: [N]
🌲 Categorías PINO y MELAMINA: 2
✅ PINO - ID: [ID]
✅ MELAMINA - ID: [ID]

🎉 PRUEBA COMPLETADA EXITOSAMENTE
✅ Firebase Firestore: Funcionando
✅ Firebase Storage: Funcionando
✅ Categorías: Creadas correctamente
✅ Productos: Creados y recuperados correctamente
✅ Imágenes: Subidas y asociadas correctamente
```

## 🔧 Solución de problemas:

- **Error de credenciales**: Verifica que `firebase-credentials.json` y `serviceAccount.json` existan
- **Error de imagen**: Verifica que `test-image.png` exista en la carpeta test
- **Error de conexión**: Verifica que tengas acceso a internet y a Firebase

## 📊 Datos de prueba creados:

El test crea datos reales en tu base de datos Firebase. Los datos incluyen:

- **2 categorías** con nombres en mayúsculas (PINO, MELAMINA)
- **3 productos** con precios, stock y códigos específicos
- **3 imágenes** subidas a Firebase Storage
- **Relaciones** entre productos y categorías

Estos datos permanecerán en tu base de datos después del test.
