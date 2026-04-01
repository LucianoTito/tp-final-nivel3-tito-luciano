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
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.sesionActiva(Session["usuario"]))
            {
                Response.Redirect("Default.aspx", false);
            }

        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != txtConfirmarPass.Text)
                {
                    Session.Add("error", "Las contraseñas no coinciden.Por favor, reviselas e intente de nuevo.");
                    Response.Redirect("Error.aspx", false);
                    return;
                }

                Usuario user = new Usuario();
                UsuarioNegocio negocio = new UsuarioNegocio();

                user.Email = txtEmail.Text;
                user.Pass = txtPass.Text;

                user.Id = negocio.InsertarNuevo(user);

                //como el usuario se acaba de registrar, lo logueamos automaticamente
                Session.Add("usuario", user);

                Response.Redirect("Default.aspx", false);   
            }
            catch (Exception ex)
            {

                Session.Add("error", "Ocurrió un error al intentar registrar el usuario. Por favor, intente de nuevo más tarde." + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        } 
    
    
    
    }
    
}