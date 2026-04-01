using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace e_commerce
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["error"] != null)
                {
                    lblMensaje.Text = Session["error"].ToString();

                }
                else
                {
                    lblMensaje.Text = "Ocurrió un error inesperado. Por favor, intentá nuevamente.";
                }
            }

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            //Limpio el error de la memoria para q no quede trabado
            Session.Remove("error");

            Response.Redirect("Default.aspx", false);
        }
    }
}