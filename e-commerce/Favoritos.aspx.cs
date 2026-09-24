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
    //PaginaConSesion: si no hay usuario logueado, la página no se ejecuta y se redirige al login.
    public partial class Favoritos : PaginaConSesion
    {
        public List<Articulo> ListaFavoritos { get; set; } = new List<Articulo>(); //lee el foreach del html
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario user = (Usuario)Session["usuario"];
                FavoritoNegocio negocio = new FavoritoNegocio();

                ListaFavoritos = negocio.ListarFavoritos(user.Id);
            }
            catch (Exception ex)
            {
                //detalle técnico solo para diagnóstico; al usuario mensaje genérico
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudieron cargar tus favoritos. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }

        }
    }
}
