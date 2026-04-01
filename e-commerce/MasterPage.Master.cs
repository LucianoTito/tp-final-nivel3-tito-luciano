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

        protected void btnSalir_Click (object sender, EventArgs e)
        {
            Session.Clear();

            Response.Redirect("Default.aspx", false);
        }
    }
}