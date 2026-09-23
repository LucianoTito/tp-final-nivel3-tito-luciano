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
    public partial class ArticulosLista : System.Web.UI.Page
    {
        public bool FiltroAvanzado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = chkAvanzado.Checked;

            //Limpio el error del filtro en cada request: CssClass y Text se guardan en el ViewState,
            //así que si no los reseteo, el rojo quedaría pegado en el próximo postback.
            txtFiltroAvanzado.CssClass = "form-control";
            lblErrorFiltro.Text = "";

            chkAvanzado.InputAttributes.Add("class", "form-check-input border-secondary");

            if (!Seguridad.esAdmin(Session["usuario"]))
            {
                //Si no es admin le mando la pantalla de error
                Session.Add("error", "Acceso denegado. Se requieren permisos de administrador para operar en esta sección.");
                Response.Redirect("Error.aspx", false);
                return;
            }
            //Cargo los datos
            try
            {
                if (!IsPostBack)
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();

                    //Una sola ida a la base: reutilizo la misma lista para la Session y para la grilla.
                
                    List<Articulo> articulos = negocio.Listar();

                    Session.Add("listaArticulos", articulos);

                    dgvArticulos.DataSource = articulos;
                    dgvArticulos.DataBind();

                    if (dgvArticulos.Rows.Count > 0)
                    {
                        dgvArticulos.UseAccessibleHeader = true;
                        dgvArticulos.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudo cargar la lista de artículos. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void dgvArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = dgvArticulos.SelectedDataKey.Value.ToString();

            Response.Redirect("ArticuloForm.aspx?id=" + id, false);
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> lista = (List<Articulo>)Session["listaArticulos"];

            List<Articulo> listaFiltrada = lista.FindAll(x =>
            x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()) ||
            x.Marca.Descripcion.ToUpper().Contains(txtFiltro.Text.ToUpper())
            );

            dgvArticulos.DataSource = listaFiltrada;
            dgvArticulos.DataBind();
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            //alterno la visibilidad y apago la textbx del filtro rápido
            FiltroAvanzado = chkAvanzado.Checked;
            txtFiltro.Enabled = !FiltroAvanzado;

            //Al activar el filtro avanzado, precargo los criterios segun el campo por defecto ("Código").
            //Sin esto, ddlCriterio queda vacío y al presionar Buscar sin tocar el campo explota (NullReference).
            if (FiltroAvanzado)
            {
                ddlCampo_SelectedIndexChanged(sender, e);
            }
        }
        protected void btnLimpiarRapido_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";

            dgvArticulos.DataSource = Session["listaArticulos"];
            dgvArticulos.DataBind();
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCriterio.Items.Clear();

            if (ddlCampo.SelectedItem.ToString() == "Precio")
            {
                ddlCriterio.Items.Add("Igual a");
                ddlCriterio.Items.Add("Mayor a");
                ddlCriterio.Items.Add("Menor a");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Empieza con");
                ddlCriterio.Items.Add("Termina con");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                //Guarda defensiva: si no hay campo o criterio seleccionado, aviso y salgo (evita NullReference).
                if (ddlCampo.SelectedItem == null || ddlCriterio.SelectedItem == null)
                {
                    MostrarErrorFiltro("Seleccioná un campo y un criterio antes de ejecutar la búsqueda.");
                    return;
                }

                string campo = ddlCampo.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text.Trim();

                //Valido también en el servidor: la validación JS se puede saltear (JS desactivado, request armado a mano).
                string error = ValidarFiltroAvanzado(campo, filtro);
                if (error != null)
                {
                    //Muestro el mensaje en la página y NO ejecuto la búsqueda (la grilla queda como estaba).
                    MostrarErrorFiltro(error);
                    return;
                }

                ArticuloNegocio negocio = new ArticuloNegocio();

                //llamo a la bd pasandole los 3 parámetros
                dgvArticulos.DataSource = negocio.Filtrar(
                    campo,
                    ddlCriterio.SelectedItem.ToString(),
                    filtro
                    );

                dgvArticulos.DataBind();
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudo ejecutar el filtro avanzado. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }

        //Mismas reglas que obtenerErrorFiltro en el JS de ArticulosLista.aspx. Devuelve el mensaje de error o null.
        private string ValidarFiltroAvanzado(string campo, string filtro)
        {
            if (filtro == "")
            {
                return "Ingresá un valor para buscar.";
            }

            if (campo == "Precio")
            {
                if (!ArticuloNegocio.TryParsePrecio(filtro, out decimal precio))
                {
                    return "El precio tiene que ser un número (podés usar coma o punto para los decimales).";
                }
                if (precio < 0)
                {
                    return "El precio no puede ser negativo.";
                }
                if (precio > ArticuloNegocio.PrecioMaximo)
                {
                    return "El precio ingresado es demasiado grande.";
                }
            }

            return null;
        }

        //Pinta el TextBox de rojo (is-invalid) y muestra el mensaje debajo (invalid-feedback).
        private void MostrarErrorFiltro(string mensaje)
        {
            txtFiltroAvanzado.CssClass = "form-control is-invalid";
            lblErrorFiltro.Text = mensaje;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            txtFiltroAvanzado.Text = "";

            chkAvanzado.Checked = false;
            FiltroAvanzado = false;

            txtFiltro.Enabled = true;
            btnLimpiarRapido.Enabled = true;

            ddlCampo.SelectedIndex = 0;
            ddlCampo_SelectedIndexChanged(sender, e);

            dgvArticulos.DataSource = Session["listaArticulos"];
            dgvArticulos.DataBind();
        }
    }
}
