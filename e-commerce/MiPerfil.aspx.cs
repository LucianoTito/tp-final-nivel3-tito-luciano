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
    //PaginaConSesion: si no hay usuario logueado, la página no se ejecuta y se redirige al login.
    //Antes el chequeo estaba solo dentro de !IsPostBack: en un postback sin sesión, btnGuardar_Click corría igual.
    public partial class MiPerfil : PaginaConSesion
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

                Usuario usuarioSesion = (Usuario)Session["usuario"];
                UsuarioNegocio negocio = new UsuarioNegocio();

                //Trabajo sobre una COPIA: si modificara directo el objeto de la sesión y después falla una
                //validación o la base, el header mostraría datos que nunca se guardaron.
                Usuario user = new Usuario
                {
                    Id = usuarioSesion.Id,
                    Email = usuarioSesion.Email,
                    Pass = usuarioSesion.Pass,
                    Admin = usuarioSesion.Admin,
                    UrlImagenPerfil = usuarioSesion.UrlImagenPerfil,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text
                };

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

                //Recién ahora que la base se actualizó bien, la sesión pasa a tener los datos nuevos.
                //La Master arma el header en PreRender (después de este click), así que ya los muestra.
                Session["usuario"] = user;

                //Si el usuario no tiene imagen, no toco el ImageUrl: queda la imagen genérica del .aspx.
                //Sin este if, la URL quedaba "~/Images/Perfiles/?v=..." (una carpeta, no una imagen) y se veía rota.
                if (!string.IsNullOrEmpty(user.UrlImagenPerfil))
                {
                    imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?v=" + DateTime.Now.Ticks.ToString();
                }

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
