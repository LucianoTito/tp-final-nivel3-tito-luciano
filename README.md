# 🛒 Tienda de Basti

**E-commerce en ASP.NET WebForms · TP Final C# Nivel 3 (Maxi Programa)**

Proyecto Full-Stack desarrollado en **C# (.NET Framework 4.8.1)** y **ASP.NET WebForms**. Aplicación de catálogo de productos con autenticación, gestión de artículos (CRUD), sistema de favoritos y perfil de usuario.

Desarrollado aplicando una **Arquitectura en 3 Capas** estricta y buenas prácticas de seguridad en el procesamiento de datos.

> 🌐 **Live Demo:** [https://tiendavirtual-lucianobasti.somee.com/](https://tiendavirtual-lucianobasti.somee.com/)

![Demo de la aplicación: catálogo y panel de administración](img/demo.gif)

## 🔐 Accesos de Prueba

Para probar la aplicación sin necesidad de registrarse (el login es por email):

| Rol | Email | Contraseña |
|---|---|---|
| Cliente (catálogo, favoritos y perfil) | `test@test.com` | `test` |

¿Querés probar el panel de administración? [Escribime por LinkedIn](https://www.linkedin.com/in/luciano-tito-cedron/) y te paso un acceso.

## ✨ Características y Logros Técnicos

- **Arquitectura en 3 Capas:** separación de responsabilidades en proyectos de Dominio, Negocio y Presentación.
- **Acceso a datos con ADO.NET:** consultas 100% parametrizadas (sin ORMs) para prevenir inyección SQL. La eliminación de un artículo borra también sus favoritos dentro de una transacción (`SqlTransaction`): se aplican las dos operaciones o ninguna.
- **Control de acceso centralizado:** las páginas protegidas heredan de clases base (`PaginaConSesion`, `PaginaAdmin`) que cortan la ejecución en `ProcessRequest`. Sin permiso, la página no se ejecuta: ni `Page_Load`, ni eventos de botones, ni render.
- **Validación en cliente y servidor con las mismas reglas:** feedback en tiempo real con JavaScript y Bootstrap (`is-invalid` / `invalid-feedback`), y la misma validación en C# como barrera real (obligatorios, largos máximos de las columnas, precios, duplicados). Mensajes y confirmaciones con componentes de Bootstrap (alertas y modal), sin `alert()` ni `confirm()` nativos.
- **Prevención de XSS:** los datos se muestran codificados (`<%: %>`, `HttpUtility.HtmlEncode`) y el JavaScript usa `textContent` en lugar de `innerHTML`.
- **HTTPS en producción:** redirección permanente (301) de http a https con IIS URL Rewrite, cookies de sesión `Secure` y `HttpOnly`, y encabezados de seguridad (`X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`).
- **Credenciales fuera del repositorio:** la cadena de conexión vive en `claves.config`, que no se sube al repositorio ni se incluye en la publicación.
- **Integridad de datos:** el email de usuario no se puede repetir (validado en la aplicación y con una restricción `UNIQUE` en la base de datos); el código de artículo (SKU) no se puede repetir (validado en la aplicación).
- **Formularios sin reenvíos accidentales:** Post/Redirect/Get con mensajes flash en `Session` al guardar el perfil, y `UpdatePanel` para filtrar y eliminar en la grilla sin recargar la página.
- **Diseño y accesibilidad:** paleta de colores coherente con contraste WCAG AA verificado y foco visible al navegar con teclado.
- **Cultura es-AR:** los precios se muestran como `$ 80.000,00` en cualquier servidor; la lectura de los precios que escribe el usuario no depende del idioma del servidor (acepta coma o punto).
- **Catálogo con imágenes locales:** fotos de producto de [Unsplash](https://unsplash.com) servidas desde el propio sitio, en un marco de proporción fija. Autores y licencia en [`CREDITOS.md`](e-commerce/Images/Articulos/CREDITOS.md).

## 🔧 Mejoras aplicadas después de la corrección

El TP fue **aprobado**. Después de la corrección apliqué las observaciones recibidas (diseño, experiencia de usuario y datos de demostración) y una **revisión propia de seguridad**, que llevó al control de acceso centralizado, a la eliminación con transacción, a la codificación de toda salida de datos y a la configuración de HTTPS, cookies y encabezados para producción.

## 🛠️ Stack Tecnológico

- **Back-End:** C# 7.3, .NET Framework 4.8.1, ASP.NET WebForms.
- **Front-End:** HTML5, CSS3, JavaScript, Bootstrap 5.
- **Base de Datos:** SQL Server (ADO.NET puro, sin ORMs).
- **Deploy:** Somee (IIS + SQL Server).

## ⚙️ Preparación y Puesta en Marcha

1. **Clonar el repositorio** y abrir la solución `TpFinalNivel3TitoLuciano.slnx` en Visual Studio.
2. **Crear la base de datos:** en SQL Server Management Studio, crear una base vacía (`CREATE DATABASE CATALOGO_WEB_DB;`) y ejecutar sobre ella `Script_Catalogo_DB.sql` (en la raíz), que crea las tablas y carga los datos iniciales de prueba. En un hosting compartido, ejecutarlo directamente sobre la base que da el hosting.
   - El script crea un administrador (`admin@admin.com` / `admin`) **solo para el entorno local**. En producción, esa contraseña debe cambiarse apenas se carga la base.
   - Si la base ya existe con los datos originales, `Script_Datos_Demo.sql` actualiza el catálogo de demostración sin recrear las tablas.
3. **Configurar la conexión:**
   - En la carpeta `e-commerce`, copiar `claves.config.example` y renombrar la copia como `claves.config`.
   - Completar la cadena de conexión con los datos de tu servidor SQL.
   - `claves.config` está en `.gitignore`: nunca se sube al repositorio. Tampoco se publica: en el servidor hay que crearlo a mano.
4. **Ejecutar:** compilar y lanzar el proyecto con IIS Express (F5).

## 👤 Sobre el Autor

**Luciano Facundo Tito Cedrón.** Estudiante de la Tecnicatura Universitaria en Programación (UTN-FRGP). Busco mi primer rol como **desarrollador backend .NET junior**, remoto o en Salta.

También me interesa el trabajo con datos y el ecosistema de Microsoft Azure.
