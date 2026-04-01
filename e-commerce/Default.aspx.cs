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
    public partial class Default : System.Web.UI.Page
    {
        
        // si por algun error la carga falla, la lista está vacía, pero no es null, el foreach no explota
        public List<Articulo> ListaArticulos { get; set; } = new List<Articulo>();

      
        public List<int> ListaFavoritosUsuario { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
    
            if (Seguridad.sesionActiva(Session["usuario"]))
            {
                Usuario user = (Usuario)Session["usuario"];
                FavoritoNegocio favoritoNegocio = new FavoritoNegocio();

           
                if (Request.QueryString["idAdd"] != null)
                {
                    try
                    {
                        int idArticulo = int.Parse(Request.QueryString["idAdd"]);
                        favoritoNegocio.InsertarFavorito(user.Id, idArticulo);

                        Session.Add("mensajeFav", "❤️ ¡Se ha agregado a tus favoritos!");

                       
                        Response.Redirect("Default.aspx", true);
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                        // esta excepción la lanza Redirect a propósito para cortar el hilo.
                        // la atrapo aquí en silencio para que no se coma la redirección. Silencia el corte de hilo
                    }
                    catch (Exception ex)
                    {
                        Session.Add("error", "Error SQL al guardar favorito: " + ex.Message);
                        Response.Redirect("Error.aspx", false);
                    }
                }

               
                if (Request.QueryString["idRm"] != null)
                {
                    try
                    {
                        int idArt = int.Parse(Request.QueryString["idRm"]);
                        favoritoNegocio.EliminarFavorito(user.Id, idArt);

                        Session.Add("mensajeFav", "💔 Se ha quitado de tus favoritos.");
                        Response.Redirect("Default.aspx", true);
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                       
                    }
                    catch (Exception ex)
                    {
                        Session.Add("error", "Error SQL al quitar favorito: " + ex.Message);
                        Response.Redirect("Error.aspx", false);
                    }
                }

               
                ListaFavoritosUsuario = favoritoNegocio.ListarFavoritos(user.Id).Select(a => a.Id).ToList();
            }


            // carga del catálogo de artículos
            
            try
            {
               
                if (!IsPostBack)
                {
                   
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    ListaArticulos = negocio.Listar();

                    
                    Session.Add("listaArticulos", ListaArticulos);
                }
                else
                {
                    
                    if (Session["listaArticulos"] != null)
                    {
                        ListaArticulos = (List<Articulo>)Session["listaArticulos"];
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al cargar el catálogo de artículos: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            
            string filtro = txtFiltro.Text.ToUpper();

            List<Articulo> listaFiltrada = ListaArticulos.FindAll(x =>
                    x.Nombre.ToUpper().Contains(filtro) ||
                    x.Marca.Descripcion.ToUpper().Contains(filtro) ||
                    x.Categoria.Descripcion.ToUpper().Contains(filtro)
                );

            ListaArticulos = listaFiltrada;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
           
            if (Session["listaArticulos"] != null)
            {
                ListaArticulos = (List<Articulo>)Session["listaArticulos"];
            }
        }
    }
}