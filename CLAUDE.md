# CLAUDE.md — Tienda Virtual (TP Final Nivel 3)

E-commerce "Tienda Virtual" en ASP.NET WebForms. Es mi TP final de un curso de C#: ya está aprobado y publicado en Somee.
Ahora estoy aplicando mejoras de diseño y UX que sugirió el corrector.

## Stack

- .NET Framework 4.8.1 (según los `.csproj`; el `Web.config` dice `targetFramework="4.8"`) y C# 7.3
- ASP.NET WebForms (`.aspx` + code-behind, `MasterPage.Master`)
- ADO.NET puro contra SQL Server (base `CATALOGO_WEB_DB`, script en `Script_Catalogo_DB.sql`)
- Bootstrap 5.3 por CDN (se carga en `MasterPage.Master`) + JavaScript sin frameworks
- Deploy: Somee (IIS + SQL Server)

## Arquitectura en 3 capas

| Proyecto | Qué va acá | Qué NO va acá |
|---|---|---|
| `Dominio/` | Clases de entidad (POCOs): `Articulo`, `Marca`, `Categoria`, `Usuario`. Solo propiedades y `ToString()`. | Lógica, SQL, referencias a otras capas |
| `Negocio/` | Acceso a datos (`AccesoDatos`) y reglas de negocio (`*Negocio`, `Seguridad`). Todo el SQL vive acá. Referencia a `Dominio`. | Código de UI, `Session`, `Response`, controles web |
| `e-commerce/` | Presentación: páginas `.aspx`, code-behind, MasterPage, JS/CSS, imágenes. Referencia a `Dominio` y `Negocio`. | SQL ni `SqlConnection`/`SqlCommand` directos |

Dependencias en un solo sentido: `e-commerce` → `Negocio` → `Dominio`.

## Reglas estrictas

1. **No migrar** a MVC, Razor Pages, Blazor ni .NET Core / .NET 5+. Todo se hace en WebForms sobre .NET Framework.
2. **Acceso a datos solo con ADO.NET a través de la clase `AccesoDatos`** (`SetearConsulta`, `SetearParametro`, `EjecutarLectura`, `EjecutarAccion`, `EjecutarAccionScalar`, `CerrarConexion`). Nada de Entity Framework, Dapper ni otro ORM.
3. **Siempre consultas parametrizadas** (`@parametro` + `SetearParametro`). Nunca concatenar input del usuario en el SQL. Si hace falta elegir una columna u operador dinámico, sale de una whitelist con `switch` (como en `ArticuloNegocio.Filtrar`).
4. **Mostrar datos en `.aspx` con `<%: %>`** (codificado en HTML) para prevenir XSS. `<%= %>` solo para valores que no vienen del usuario ni de la base, como `ClientID`.
5. **Validar en el cliente (JS + clases de Bootstrap `is-valid` / `is-invalid`) y también en el servidor (C#).** La validación del cliente es para la UX; la del servidor es la barrera real (nulos, negativos, largos máximos de las columnas SQL, duplicados).
6. **Confirmaciones y mensajes con componentes de Bootstrap** (modal, alert, toast), no con `alert()` / `confirm()` nativos.
7. **Nunca leer, modificar ni commitear `e-commerce/claves.config`** (tiene la cadena de conexión real). Si hace falta ver el formato, usar `claves.config.example`.
8. **No hacer `git push` ni reescribir el historial** (`rebase`, `commit --amend`, `reset --hard`, `push --force`) sin preguntarme antes.
9. **Un commit por tarea**, con mensaje en español estilo `tipo: descripción` (el scope es opcional, como en el historial: `fix(db): ...`, `refactor(config): ...`). Tipos: `feat`, `fix`, `refactor`, `style`, `docs`, `chore`.

## Forma de trabajo

- Estoy aprendiendo: **explicame cada cambio y el porqué** (qué problema resuelve, por qué esa solución y no otra, y qué concepto de WebForms / C# / Bootstrap aplica).
- Cambios chicos y enfocados en la tarea pedida; no refactorizar código que no tiene que ver.
- Respetar el estilo del código existente: nombres en español, comentarios en español, mismos patrones.
- **Al terminar cada tarea, decime qué probar en Visual Studio**: qué página abrir con F5 (IIS Express), con qué usuario (cliente o admin) y qué casos probar (el camino feliz, errores de validación, sesión cerrada).

## Convenciones del código existente

- Métodos de `*Negocio`: `new AccesoDatos()` → `try` / `catch { throw; }` / `finally { datos.CerrarConexion(); }`.
- Manejo de errores en las páginas: `System.Diagnostics.Debug.WriteLine(ex.ToString())` para el detalle técnico (solo se ve en la ventana Salida de Visual Studio), `Session.Add("error", "<mensaje amigable>")` y `Response.Redirect("Error.aspx", false)`. Al usuario nunca se le muestra `ex.ToString()` ni `ex.Message`.
- Claves de sesión: `"usuario"` (objeto `Usuario`), `"listaArticulos"`, `"mensajeFav"`, `"error"`.
- Control de acceso: una página protegida **hereda** de una clase base en lugar de `System.Web.UI.Page`:
  - `PaginaConSesion` (requiere usuario logueado; si no, redirige a `Login.aspx`). La usan `MiPerfil` y `Favoritos`.
  - `PaginaAdmin` (requiere admin; si no, redirige a `Error.aspx` con "Acceso denegado"). La usan `ArticulosLista` y `ArticuloForm`.
  - Las dos cortan en `ProcessRequest`: si no hay acceso, la página no se ejecuta (ni `Page_Load`, ni eventos de botones, ni render). No repetir el chequeo en `Page_Load` ni en los eventos.
  - Nunca proteger una página con `if (...) Response.Redirect(url, false)` en `Page_Load`: el redirect no corta el ciclo de vida y los eventos de los botones se ejecutan igual.
  - `Seguridad.sesionActiva` / `Seguridad.esAdmin` se siguen usando para mostrar u ocultar cosas (ej: botones del navbar, favoritos en `Default`).
- Mensajes al usuario (cliente y servidor): con componentes de Bootstrap, nunca con `alert()` ni `RegisterStartupScript`.
  - Errores de un campo: `is-invalid` en el control + un `asp:Label` con `invalid-feedback` justo después (el servidor pone `CssClass` y `Text`; el JS usa `classList` y `textContent`). Resetear clases y textos en `Page_Load`, porque quedan en el ViewState.
  - Mensajes generales (éxito, error, advertencia): un `alert` de Bootstrap que el servidor muestra u oculta.
  - Todo texto que el servidor pone en un `Label` o en `Page.Title` se codifica con `HttpUtility.HtmlEncode`, porque esos controles lo escriben tal cual en el HTML.
  - Después de guardar con éxito: Post/Redirect/Get, con el mensaje como flash en `Session` (se muestra una vez y se borra), así F5 no reenvía el formulario.
- Formularios con TextBox: envolverlos en `asp:Panel DefaultButton="..."`. Si no, con sesión iniciada Enter dispara el primer botón del form, que es "Salir" del navbar.
- Colores (paleta "Azul y ámbar", definida en `Content/estilos.css`): `btn-primary` para acciones principales, `btn-outline-secondary` / `btn-outline-primary` para secundarias, `btn-acento` / `btn-outline-acento` para lo destacado (Registrarse, favoritos), rojo (`danger`) solo para eliminar y errores, verde (`success`) solo para mensajes de éxito. Navbar, footer y cabecera de grilla usan `navbar-marca`, `footer-marca` y `encabezado-grilla`. No usar `btn-warning` / `btn-info` ni colores hexadecimales fijos en las páginas: siempre las variables de la paleta.

## Estructura del proyecto

**Páginas (`e-commerce/`)**
- `MasterPage.Master`: navbar con los botones según la sesión o el rol, footer, Bootstrap por CDN.
- `Default.aspx`: catálogo en tarjetas, búsqueda y agregar/quitar favoritos (`?idAdd=` / `?idRm=`).
- `Detalle.aspx`: detalle de un artículo (`?id=`).
- `Login.aspx` / `Registro.aspx`: login por email y alta de usuario.
- `MiPerfil.aspx`: edición de nombre, apellido e imagen de perfil (se guarda en `Images/Perfiles/`). Requiere sesión.
- `Favoritos.aspx`: favoritos del usuario logueado.
- `ArticulosLista.aspx`: (admin) GridView de artículos con filtro avanzado, y eliminación desde la grilla con un modal de Bootstrap.
- `ArticuloForm.aspx`: (admin) alta y modificación de un artículo (`?id=` para editar).
- `Error.aspx`: muestra el mensaje amigable guardado en `Session["error"]` (o uno genérico si no hay).

**Clases de Negocio (`Negocio/`)**
- `AccesoDatos`: conexión, comando y lector de ADO.NET; lee `conexionDB` desde la configuración. Soporta transacciones (`IniciarTransaccion`, `ConfirmarTransaccion`, `CancelarTransaccion`).
- `ArticuloNegocio`: `Listar`, `ObtenerPorId`, `Filtrar`, `AgregarArticulo`, `ModificarArticulo`, `Eliminar`.
- `MarcaNegocio` / `CategoriaNegocio`: `listar()` para los desplegables.
- `UsuarioNegocio`: `Loguear`, `InsertarNuevo`, `ExisteEmail`, `ActualizarPerfil`.
- `FavoritoNegocio`: `Existe`, `InsertarFavorito`, `EliminarFavorito`, `ListarFavoritos`.
- `Seguridad` (estática): `sesionActiva`, `esAdmin`.

## Deuda conocida (candidata a las mejoras de UX)

Sin deuda conocida por ahora.
