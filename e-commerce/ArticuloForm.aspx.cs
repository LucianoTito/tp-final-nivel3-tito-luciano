using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;


namespace e_commerce
{
    public partial class ArticuloForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.esAdmin(Session["usuario"]))
            {
                Session.Add("error", "Acceso denegado. Se requieren permisos de administrador para operar aquí.");
                Response.Redirect("Error.aspx", false);

            }
            try
            {
                if (!IsPostBack)
                {
                    //DropDownList (Desplegables)
                    MarcaNegocio marcaNegocio = new MarcaNegocio();
                    List<Marca> listaMarcas = marcaNegocio.listar();
                    ddlMarca.DataSource = listaMarcas;
                    ddlMarca.DataValueField = "Id";
                    ddlMarca.DataTextField = "Descripcion";
                    ddlMarca.DataBind();

                    CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                    List<Categoria> listaCategorias = categoriaNegocio.listar();
                    ddlCategoria.DataSource = listaCategorias;
                    ddlCategoria.DataValueField = "Id";
                    ddlCategoria.DataTextField = "Descripcion";
                    ddlCategoria.DataBind();

                    //Evaluo el modo, si viene con ID es una modificación 
                    string id = Request.QueryString["id"];
                    if (id != null)
                    {
                        ArticuloNegocio negocio = new ArticuloNegocio();

                        Articulo seleccionado = negocio.Listar().Find(X => X.Id == int.Parse(id));

                        //Precargo los datos en el formulario
                        txtCodigo.Text = seleccionado.Codigo;
                        txtNombre.Text = seleccionado.Nombre;
                        txtDescripcion.Text = seleccionado.Descripcion;
                        txtImagenUrl.Text = seleccionado.ImagenUrl;
                        txtPrecio.Text = seleccionado.Precio.ToString();

                        //posiciono los desplegables en la opción correcta
                        ddlMarca.SelectedValue = seleccionado.Marca.Id.ToString();
                        ddlCategoria.SelectedValue = seleccionado.Categoria.Id.ToString();

                        //Fordar que la imagen se dibuje disparando el evento manualmente
                        txtImagenUrl_TextChanged(sender, e);

                        btnEliminar.Visible = true;

                    }
                }

            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al cargar el formulario: "+ ex.Message);
                Response.Redirect("Error.aspx", false );
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            imgArticulo.ImageUrl = txtImagenUrl.Text;   
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtImagenUrl.Text.Length > 500)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaUrl", "alert('⚠️ La URL de la imagen es demasiado larga. Ingrese un enlace que no supere los 1000 caracteres.');", true);
                    return;
                }

                
                if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtPrecio.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaVacios", "alert('⚠️ Los campos Código, Nombre y Precio son estrictamente obligatorios.');", true);
                    return;
                }

                if (txtCodigo.Text.Length > 50)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaCodigoLargo", "alert('⚠️ El Código no puede superar los 50 caracteres.'); document.getElementById('txtCodigo').classList.add('is-invalid');", true);
                    return;
                }

                if (txtNombre.Text.Length > 50)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaNombreLargo", "alert('⚠️ El Nombre no puede superar los 50 caracteres.'); document.getElementById('txtNombre').classList.add('is-invalid');", true);
                    return;
                }

                if (txtDescripcion.Text.Length > 150)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaDescLarga", "alert('⚠️ La Descripción es demasiado larga (máximo 150 caracteres).'); document.getElementById('txtDescripcion').classList.add('is-invalid');", true);
                    return;
                }


                decimal precioValidado;
                if (!decimal.TryParse(txtPrecio.Text, out precioValidado) || precioValidado < 0)
                {
                    string scriptPrecio = "alert('⛔ El precio ingresado no es válido. No puede ser negativo.'); document.getElementById('txtPrecio').classList.add('is-invalid');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaPrecio", scriptPrecio, true);
                    return;
                }

                decimal codigoNumerico;
                if (decimal.TryParse(txtCodigo.Text, out codigoNumerico) && codigoNumerico < 0)
                {
                    string scriptCodigoNeg = "alert('⛔ El código de artículo no puede ser un número negativo.'); document.getElementById('txtCodigo').classList.add('is-invalid');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaCodigoNeg", scriptCodigoNeg, true);
                    return;
                }

                ArticuloNegocio negocio = new ArticuloNegocio();

                // validar código duplicado 
                List<Articulo> listaActual = negocio.Listar();
                bool codigoExiste = false;
                string codigoIngresado = txtCodigo.Text.Trim().ToUpper();

                if (Request.QueryString["id"] != null)
                {
                    //busco si existe el código en otro artículo distinto al actual
                    int idActual = int.Parse(Request.QueryString["id"]);
                    codigoExiste = listaActual.Any(x => x.Codigo.ToUpper() == codigoIngresado && x.Id != idActual);
                }
                else
                {
                    //busco si existe el código en cualquier artículo
                    codigoExiste = listaActual.Any(x => x.Codigo.ToUpper() == codigoIngresado);
                }

                if (codigoExiste)
                {
                    
                    string scriptAviso = "alert('⛔ El Código de Artículo \\'" + txtCodigo.Text + "\\' ya se encuentra registrado. Por favor, ingrese un código único.');";
                    scriptAviso += "document.getElementById('txtCodigo').classList.add('is-invalid');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaDuplicado", scriptAviso, true);

                    return;
                }

                //fin de las validaciones

                Articulo nuevo = new Articulo();

                // paso los datos HTML a un objeto
                nuevo.Codigo = txtCodigo.Text;
                nuevo.Nombre = txtNombre.Text;
                nuevo.Descripcion = txtDescripcion.Text;
                nuevo.ImagenUrl = txtImagenUrl.Text;
                nuevo.Precio = precioValidado;

                // instancio objetos internos y asocio IDs de desplegables
                nuevo.Marca = new Marca();
                nuevo.Marca.Id = int.Parse(ddlMarca.SelectedValue);

                nuevo.Categoria = new Categoria();
                nuevo.Categoria.Id = int.Parse(ddlCategoria.SelectedValue);

               //guardo o actualizo?
                if (Request.QueryString["id"] != null)
                {
                    nuevo.Id = int.Parse(Request.QueryString["id"]);
                    negocio.ModificarArticulo(nuevo);
                }
                else
                {
                    negocio.AgregarArticulo(nuevo);
                }

                Response.Redirect("ArticulosLista.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al intentar guardar el artículo: " + ex.Message);
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnEliminar_Click (object sender, EventArgs e)
        {
            try
            {  
                //eliminación física
                if(Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    ArticuloNegocio negocio = new ArticuloNegocio();    
                    negocio.Eliminar(id);

                    Response.Redirect("ArticulosLista.aspx",false);
                }

            }
            catch (Exception ex)
            {

                Session.Add("error", "Error al intentar eliminar el artículo: "+ ex.Message);
                Response.Redirect("Error.aspx", false ) ;
            }
        }
    }
}