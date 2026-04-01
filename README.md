# 🛒 Tienda Virtual (TP Final - Nivel 3)

Proyecto Full-Stack desarrollado en C# (.NET Framework 4.8.1) y ASP.NET WebForms. Aplicación de catálogo de productos con autenticación, gestión de artículos (CRUD), sistema de favoritos asíncrono y perfil de usuario. 

Desarrollado aplicando una arquitectura limpia en capas y fuertes estándares de seguridad en el procesamiento de datos.

## 🔐 Accesos de Prueba 
Para probar la funcionalidad completa del sistema sin necesidad de registrarse, utilizar las siguientes credenciales:
* **Administrador (Acceso total al CRUD):** Usuario: `admin` | Clave: `admin`
* **Cliente (Catálogo y Favoritos):** Usuario: `test` | Clave: `test`

## Características y Logros Técnicos Principales
- **Arquitectura en 3 Capas:** Separación estricta de responsabilidades (`Dominio`, `Negocio`, `Presentación`).
- **Seguridad y Doble Validación (Front/Back):** - Control de UX en tiempo real con JavaScript y Bootstrap (is-valid/is-invalid).
  - Barreras en el servidor (C#) previniendo ingresos nulos, valores negativos y desbordamiento de texto (Truncamiento SQL).
  - Prevención de Códigos de Artículo (SKU) duplicados mediante consultas `LINQ`.
- **Manejo Avanzado de Estado:** - Uso de `ScriptManager.RegisterStartupScript` para feedback asíncrono (Flash Messages) sin perder los datos del formulario.
  - Redirecciones seguras controlando el ciclo de vida de WebForms (prevención de `ThreadAbortException`).
- **Sistema de Favoritos:** Interfaz dinámica tipo *Toggle* basada en la memoria de sesión del usuario.

## 🛠️ Stack Tecnológico
- **Back-End:** C# 7.3, .NET Framework 4.8.1, ASP.NET WebForms.
- **Front-End:** HTML5, CSS3, JavaScript, Bootstrap 5.
- **Base de Datos:** SQL Server (ADO.NET puro, sin ORMs).

## ⚙️ Preparación y Puesta en Marcha

1. Clonar el repositorio y abrir la solución `e-commerce.sln` en Visual Studio.
2. **Base de datos:** La aplicación espera una base de datos llamada `CATALOGO_WEB_DB` en la instancia local `.\SQLEXPRESS`.
3. **Conexión:** La cadena de conexión puede ser modificada en la clase `Negocio/AccesoDatos.cs`.
4. Compilar y ejecutar utilizando IIS Express (F5).

Sobre el Autor
Luciano Tito Cedrón Estudiante de la Tecnicatura Universitaria en Programación (UTN-FRGP) y desarrollador en formación constante Actualmente orientando mi carrera hacia la Ingeniería de Datos en el ecosistema de Microsoft Azure.
