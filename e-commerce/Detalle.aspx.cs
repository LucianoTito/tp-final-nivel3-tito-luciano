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
    public partial class Detalle : System.Web.UI.Page

    {

        
        public Articulo ArticuloSeleccionado {  get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);

                    ArticuloNegocio negocio = new ArticuloNegocio();

                    ArticuloSeleccionado = negocio.Listar().Find(x => x.Id == id);
                }

            }
            catch (Exception)
            {
                Session.Add("error", "Hubo un problema al cargar el detalle del artículo. Es posible que no exista o que haya un problema de conexión.");
                Response.Redirect("Error.aspx", false);
            }
            
        }
    }
}