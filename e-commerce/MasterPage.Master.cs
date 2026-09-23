using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;


namespace e_commerce
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        //Nombre de la tienda en un solo lugar: lo usan el navbar, el footer y el título de la pestaña.
        protected const string NombreTienda = "Tienda de Basti";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.sesionActiva(Session["usuario"]))
            {
                Usuario user = (Usuario)Session["usuario"];

                lblUser.Text = "Hola, " + user.Nombre;

                if(!string.IsNullOrEmpty(user.UrlImagenPerfil))
                {
                    imgAvatar.ImageUrl = "~/Images/Perfiles/" + user.UrlImagenPerfil + "?v=" + DateTime.Now.Ticks.ToString();
                }
                else
                {
                    imgAvatar.ImageUrl = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
                }
            }

        }

        //Armo el título de la pestaña en PreRender: a esta altura ya corrieron el Page_Load de la página
        //y los eventos de los botones, así que cualquier Title que haya puesto la página ya es el definitivo.
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Page.Title))
            {
                Page.Title = NombreTienda;
            }
            else
            {
                //Page.Title se escribe tal cual en el HTML (no se codifica solo), por eso lo codifico acá.
                Page.Title = HttpUtility.HtmlEncode(Page.Title) + " | " + NombreTienda;
            }
        }

        protected void btnSalir_Click (object sender, EventArgs e)
        {
            Session.Clear();

            Response.Redirect("Default.aspx", false);
        }
    }
}