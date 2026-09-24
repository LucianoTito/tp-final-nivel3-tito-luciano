using System.Web;
using Negocio;

namespace e_commerce
{
    //Página base para las páginas que requieren un usuario administrador (ej: ArticulosLista, ArticuloForm).
    //Hereda el corte de PaginaConSesion y solo cambia la regla y a dónde se lo manda.
    public class PaginaAdmin : PaginaConSesion
    {
        protected override bool TieneAcceso(HttpContext context)
        {
            return Seguridad.esAdmin(context.Session["usuario"]);
        }

        protected override void RechazarAcceso(HttpContext context)
        {
            context.Session["error"] = "Acceso denegado. Se requieren permisos de administrador para operar en esta sección.";
            context.Response.Redirect("~/Error.aspx", false);
        }
    }
}
