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
    public partial class Favoritos : System.Web.UI.Page
    {
        public List<Articulo> ListaFavoritos { get; set; } = new List<Articulo>(); //lee el foreach del html
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Seguridad.sesionActiva(Session["usuario"]))
                {
                    Response.Redirect("Login.aspx", false); 
                    return;
                }

                Usuario user = (Usuario)Session["usuario"];
                FavoritoNegocio negocio = new FavoritoNegocio();

                ListaFavoritos = negocio.ListarFavoritos(user.Id);
            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar tus favoritos: "+ ex.Message);
                Response.Redirect("Error.aspx", false); 
            }

        }
    }
}