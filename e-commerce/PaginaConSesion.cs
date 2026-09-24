using System;
using System.Web;
using System.Web.UI;
using Negocio;

namespace e_commerce
{
    //Página base para las páginas que requieren un usuario logueado (ej: MiPerfil, Favoritos).
    //Para protegerla alcanza con heredar de esta clase en lugar de System.Web.UI.Page.
    public class PaginaConSesion : Page
    {
        //ProcessRequest es el punto de entrada de la página: ASP.NET lo llama una vez por request
        //y es el que ejecuta TODO el ciclo de vida (Init, Load, eventos de los botones, PreRender, Render).
        //Si no hay acceso, no llamo a base.ProcessRequest: la página directamente no se ejecuta.
        public override void ProcessRequest(HttpContext context)
        {
            if (!TieneAcceso(context))
            {
                RechazarAcceso(context);

                //Redirect con false no corta nada (y con true corta con una excepción).
                //CompleteRequest le avisa a ASP.NET que saltee el resto del pipeline y mande la respuesta
                //(el 302) tal como está: sin el HTML de la página ni su ViewState.
                context.ApplicationInstance.CompleteRequest();
                return;
            }

            base.ProcessRequest(context);
        }

        //Regla de acceso: hay un usuario logueado. PaginaAdmin la cambia por "es admin".
        protected virtual bool TieneAcceso(HttpContext context)
        {
            return Seguridad.sesionActiva(context.Session["usuario"]);
        }

        //Qué hacer si no tiene acceso: mandarlo al login.
        protected virtual void RechazarAcceso(HttpContext context)
        {
            context.Response.Redirect("~/Login.aspx", false);
        }
    }
}
