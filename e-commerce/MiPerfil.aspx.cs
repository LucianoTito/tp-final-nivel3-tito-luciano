using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Dominio;
using Negocio;

namespace e_commerce
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        // Whitelist de extensiones de imagen permitidas (validación del lado del servidor).
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

        // Tamaño máximo permitido para la imagen de perfil: 2 MB.
        private const int TamanioMaximoBytes = 2 * 1024 * 1024;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlExito.Visible = false;

            try
            {
                if (!IsPostBack)
                {
                    if (!Seguridad.sesionActiva(Session["usuario"]))
                    {
                        Response.Redirect("Login.aspx", false);
                        return;
                    }

                    Usuario user = (Usuario)Session["usuario"];

                    txtEmail.Text = user.Email;
                    txtNombre.Text = user.Nombre;
                    txtApellido.Text = user.Apellido;

                    if (!string.IsNullOrEmpty(user.UrlImagenPerfil))
                    {
                        imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?v=" + DateTime.Now.Ticks.ToString();
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
                //valido en el servidor
                if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text))
                {
                    Session.Add("error", "Los campos Nombre y Apellido son obligatorios.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                Usuario user = (Usuario)Session["usuario"];
                UsuarioNegocio negocio = new UsuarioNegocio();

                user.Nombre = txtNombre.Text;
                user.Apellido = txtApellido.Text;

                //manejo de la img física
                //Uso ContentLength > 0 (no el FileName): es la forma confiable de saber si realmente se subió un archivo.
                if (txtImagen.PostedFile != null && txtImagen.PostedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(txtImagen.PostedFile.FileName).ToLowerInvariant();

                    //BARRERA 1: solo permito extensiones de imagen reales.
                    //accept="image/*" en el HTML es solo del lado cliente y se saltea fácil.
                    if (Array.IndexOf(ExtensionesPermitidas, extension) < 0)
                    {
                        Session.Add("error", "Formato de imagen no permitido. Solo se aceptan archivos .jpg, .jpeg o .png.");
                        Response.Redirect("Error.aspx", false);
                        return;
                    }

                    //BARRERA 2: límite de tamaño para evitar subir archivos enormes.
                    if (txtImagen.PostedFile.ContentLength > TamanioMaximoBytes)
                    {
                        Session.Add("error", "La imagen es demasiado grande. El tamaño máximo permitido es 2 MB.");
                        Response.Redirect("Error.aspx", false);
                        return;
                    }

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

                imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?V=" + DateTime.Now.Ticks.ToString();

                pnlExito.Visible = true;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudieron guardar los cambios del perfil. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}
