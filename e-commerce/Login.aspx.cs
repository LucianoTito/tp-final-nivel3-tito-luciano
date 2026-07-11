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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                Response.Redirect("Default.aspx", false);
            }
        }


        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    lblError.Text = "Por favor, complete ambos campos para ingresar.";
                    lblError.Visible = true;
                    return;
                }

                Usuario user = new Usuario();
                UsuarioNegocio negocio = new UsuarioNegocio();

                user.Email = txtEmail.Text;
                user.Pass = txtPassword.Text;

                if (negocio.Loguear(user))
                {
                    //Si loguear es true
                    Session.Add("usuario", user);
                    Response.Redirect("Default.aspx", false);

                }
                else
                {
                    lblError.Text = "Email o contraseñas incorrectos. Intente nuevamente";
                    lblError.Visible = true;
                }

            }
            catch (Exception ex)
            {
                //detalle técnico solo para diagnóstico; al usuario mensaje genérico
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "Ocurrió un error inesperado al intentar iniciar sesión. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);

            }
        }
    }
}
