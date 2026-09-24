using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Dominio;
using Negocio;

namespace e_commerce
{
    //PaginaConSesion: si no hay usuario logueado, la página no se ejecuta y se redirige al login.
    //Antes el chequeo estaba solo dentro de !IsPostBack: en un postback sin sesión, btnGuardar_Click corría igual.
    public partial class MiPerfil : PaginaConSesion
    {
        // Whitelist de extensiones de imagen permitidas (validación del lado del servidor).
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

        // Tamaño máximo permitido para la imagen de perfil: 2 MB.
        private const int TamanioMaximoBytes = 2 * 1024 * 1024;

        // Solo letras (con tildes y ñ) y espacios. Misma regla que REGEX_LETRAS en el JS de MiPerfil.aspx.
        private static readonly Regex RegexLetras = new Regex(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlExito.Visible = false;

            //Limpio los errores en cada request: CssClass, Text y los atributos se guardan en el ViewState,
            //así que si no los reseteo, el rojo quedaría pegado en el próximo postback.
            LimpiarErrores();

            try
            {
                if (!IsPostBack)
                {
                    Usuario user = (Usuario)Session["usuario"];

                    txtEmail.Text = user.Email;
                    txtNombre.Text = user.Nombre;
                    txtApellido.Text = user.Apellido;

                    if (!string.IsNullOrEmpty(user.UrlImagenPerfil))
                    {
                        imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?v=" + DateTime.Now.Ticks.ToString();
                    }

                    //Mensaje flash del Post/Redirect/Get: si venimos de guardar, lo muestro UNA vez y lo borro,
                    //así no vuelve a aparecer si el usuario aprieta F5.
                    if (Session["mensajePerfil"] != null)
                    {
                        //El Label escribe el Text tal cual en el HTML: lo codifico (regla 4).
                        lblMensajeExito.Text = HttpUtility.HtmlEncode(Session["mensajePerfil"].ToString());
                        pnlExito.Visible = true;
                        Session.Remove("mensajePerfil");
                    }
                }

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudo cargar tu perfil. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Trabajo con los valores sin espacios al principio y al final (y así se guardan).
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();

                //Valido en el servidor (la validación JS se puede saltear). Los errores se muestran en la misma
                //página, debajo de cada campo, en lugar de mandar a Error.aspx.
                //Evalúo los tres antes de cortar, para marcar todos los errores de una sola vez.
                bool nombreOk = MostrarResultado(txtNombre, lblErrorNombre, ValidarNombreOApellido("nombre", nombre));
                bool apellidoOk = MostrarResultado(txtApellido, lblErrorApellido, ValidarNombreOApellido("apellido", apellido));
                bool imagenOk = MostrarResultado(txtImagen, lblErrorImagen, ValidarImagen());

                if (!(nombreOk && apellidoOk && imagenOk))
                {
                    return;
                }

                Usuario usuarioSesion = (Usuario)Session["usuario"];
                UsuarioNegocio negocio = new UsuarioNegocio();

                //Trabajo sobre una COPIA: si modificara directo el objeto de la sesión y después falla
                //la base, el header mostraría datos que nunca se guardaron.
                Usuario user = new Usuario
                {
                    Id = usuarioSesion.Id,
                    Email = usuarioSesion.Email,
                    Pass = usuarioSesion.Pass,
                    Admin = usuarioSesion.Admin,
                    UrlImagenPerfil = usuarioSesion.UrlImagenPerfil,
                    Nombre = nombre,
                    Apellido = apellido
                };

                //manejo de la img física (ya validada: extensión y tamaño)
                //Uso ContentLength > 0 (no el FileName): es la forma confiable de saber si realmente se subió un archivo.
                if (txtImagen.PostedFile != null && txtImagen.PostedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(txtImagen.PostedFile.FileName).ToLowerInvariant();

                    string ruta = Server.MapPath("./Images/Perfiles/");

                    //si la carpeta perfiles no existe, la creo
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    //El nombre lo genero yo ; no uso el nombre original del archivo del usuario.
                    string nombreArchivo = "perfil-" + user.Id + extension;

                    txtImagen.PostedFile.SaveAs(Path.Combine(ruta, nombreArchivo));

                    user.UrlImagenPerfil = nombreArchivo;
                }

                //guardo los cambios en la bd
                negocio.ActualizarPerfil(user);

                //Recién ahora que la base se actualizó bien, la sesión pasa a tener los datos nuevos.
                Session["usuario"] = user;

                //Post/Redirect/Get: en vez de mostrar el resultado como respuesta de este POST, redirijo a la
                //misma página con un GET. Así, si el usuario aprieta F5, el navegador repite el GET (recarga)
                //y no vuelve a enviar el formulario ni a subir la imagen. El mensaje viaja como "flash" en Session.
                //El header se actualiza igual: la página nueva lee Session["usuario"], que ya tiene los datos nuevos.
                Session["mensajePerfil"] = "Usuario actualizado satisfactoriamente.";
                Response.Redirect("MiPerfil.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudieron guardar los cambios del perfil. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }

        //----- Reglas: las mismas que errorNombreOApellido y errorImagen del JS en MiPerfil.aspx -----
        //Cada método devuelve el mensaje de error, o null si el valor es válido.

        private string ValidarNombreOApellido(string etiqueta, string valor)
        {
            if (valor == "")
                return "El " + etiqueta + " es obligatorio.";
            if (!RegexLetras.IsMatch(valor))
                return "El " + etiqueta + " solo puede tener letras y espacios.";
            if (valor.Length > 50)
                return "El " + etiqueta + " no puede superar los 50 caracteres.";
            return null;
        }

        private string ValidarImagen()
        {
            //la imagen es opcional
            if (txtImagen.PostedFile == null || txtImagen.PostedFile.ContentLength == 0)
                return null;

            string extension = Path.GetExtension(txtImagen.PostedFile.FileName).ToLowerInvariant();

            //BARRERA 1: solo permito extensiones de imagen reales.
            //accept="image/*" en el HTML es solo del lado cliente y se saltea fácil.
            if (Array.IndexOf(ExtensionesPermitidas, extension) < 0)
                return "Formato de imagen no permitido. Solo se aceptan archivos .jpg, .jpeg o .png.";

            //BARRERA 2: límite de tamaño para evitar subir archivos enormes.
            if (txtImagen.PostedFile.ContentLength > TamanioMaximoBytes)
                return "La imagen es demasiado grande. El tamaño máximo permitido es 2 MB.";

            return null;
        }

        //----- Feedback de Bootstrap desde el servidor -----

        //Si hay error, pinta el campo de rojo (is-invalid) y escribe el mensaje en su invalid-feedback.
        //Devuelve true si el campo es válido.
        private bool MostrarResultado(TextBox txt, Label lbl, string error)
        {
            if (error == null)
                return true;

            txt.CssClass = "form-control is-invalid";
            lbl.Text = HttpUtility.HtmlEncode(error);
            return false;
        }

        //Sobrecarga para el input de archivo (es un HtmlInputFile, no un TextBox: la clase va en Attributes).
        private bool MostrarResultado(System.Web.UI.HtmlControls.HtmlInputFile input, Label lbl, string error)
        {
            if (error == null)
                return true;

            input.Attributes["class"] = "form-control form-control-sm is-invalid";
            lbl.Text = HttpUtility.HtmlEncode(error);
            return false;
        }

        private void LimpiarErrores()
        {
            txtNombre.CssClass = "form-control";
            txtApellido.CssClass = "form-control";
            txtImagen.Attributes["class"] = "form-control form-control-sm";
            lblErrorNombre.Text = "";
            lblErrorApellido.Text = "";
            lblErrorImagen.Text = "";
        }
    }
}
