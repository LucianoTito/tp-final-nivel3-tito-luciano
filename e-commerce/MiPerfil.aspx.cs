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
        protected void Page_Load(object sender, EventArgs e)
        {
			pnlExito.Visible = false;

			try
			{
				if(!IsPostBack)
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

					if(!string.IsNullOrEmpty(user.UrlImagenPerfil))
					{
						imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?v=" + DateTime.Now.Ticks.ToString();
					}
				}

			}
			catch (Exception ex)
			{

				Session.Add("error", "Error al cargar el perfil:" + ex.Message);
				Response.Redirect("Error.aspx", false);
			}

        }

		protected void btnGuardar_Click(object sender, EventArgs e)
		{
			try
			{
				//valido en el servidor
				if(string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text))
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
				if(txtImagen.PostedFile.FileName != "")
				{
					string ruta = Server.MapPath("./Images/Perfiles/");

					//si la carpeta perfiles no existe, la creo
					if(!Directory.Exists(ruta))
					{
						Directory.CreateDirectory(ruta);
					}

					string extension = Path.GetExtension(txtImagen.PostedFile.FileName);

					string nombreArchivo = "perfil-" + user.Id + extension;

					txtImagen.PostedFile.SaveAs(ruta + nombreArchivo);

					user.UrlImagenPerfil = nombreArchivo;
				}

				//guarddo los cambios en la bd
				negocio.ActualizarPerfil(user);

				imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?V=" + DateTime.Now.Ticks.ToString();

				pnlExito.Visible = true;

			}
			catch (Exception ex)
			{
				Session.Add("error", "Error al intentar guardar el perfil: " + ex.Message);
				Response.Redirect("Error.aspx", false);
			}
		}
    }
}