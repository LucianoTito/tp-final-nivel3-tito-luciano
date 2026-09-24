using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;


namespace e_commerce
{
    //PaginaAdmin: si el usuario no es admin, la página no se ejecuta (ver PaginaConSesion.ProcessRequest).
    public partial class ArticuloForm : PaginaAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Limpio los errores en cada request: CssClass, Text y los atributos se guardan en el ViewState,
            //así que si no los reseteo, el rojo quedaría pegado en el próximo postback.
            LimpiarErrores();

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

                        Articulo seleccionado = negocio.ObtenerPorId(int.Parse(id));

                        //Si el Id no corresponde a ningún artículo, corto acá (antes tiraba NullReference).
                        if (seleccionado == null)
                        {
                            Session.Add("error", "El artículo solicitado no existe o fue eliminado.");
                            Response.Redirect("Error.aspx", false);
                            return;
                        }

                        //Precargo los datos en el formulario
                        txtCodigo.Text = seleccionado.Codigo;
                        txtNombre.Text = seleccionado.Nombre;
                        txtDescripcion.Text = seleccionado.Descripcion;
                        txtImagenUrl.Text = seleccionado.ImagenUrl;
                        //InvariantCulture: siempre con punto decimal, sin importar el idioma del servidor.
                        //"0.####" muestra hasta 4 decimales (los que guarda el tipo money) sin ceros de más.
                        txtPrecio.Text = seleccionado.Precio.ToString("0.####", CultureInfo.InvariantCulture);

                        //posiciono los desplegables en la opción correcta
                        ddlMarca.SelectedValue = seleccionado.Marca.Id.ToString();
                        ddlCategoria.SelectedValue = seleccionado.Categoria.Id.ToString();

                        //Forzar que la imagen se dibuje disparando el evento manualmente
                        txtImagenUrl_TextChanged(sender, e);
                    }
                }

            }
            catch (Exception ex)
            {

             
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudo cargar el formulario del artículo. Intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            //Si el admin borra la URL, vuelvo a la imagen "sin imagen" en vez de dejar un <img> sin src.
            if (string.IsNullOrWhiteSpace(txtImagenUrl.Text))
            {
                imgArticulo.ImageUrl = "~/Images/sin-imagen.svg";
            }
            else
            {
                imgArticulo.ImageUrl = txtImagenUrl.Text;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                //Trabajo con los valores sin espacios al principio y al final (y así se guardan).
                string codigo = txtCodigo.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();
                string imagenUrl = txtImagenUrl.Text.Trim();

                //Valido también en el servidor: la validación JS se puede saltear (JS desactivado, request armado a mano).
                //Evalúo TODOS los campos antes de cortar, para marcar todos los errores de una sola vez.
                decimal precio;
                bool codigoOk = MostrarResultado(txtCodigo, lblErrorCodigo, ValidarCodigo(codigo));
                bool nombreOk = MostrarResultado(txtNombre, lblErrorNombre, ValidarNombre(nombre));
                bool precioOk = MostrarResultado(txtPrecio, lblErrorPrecio, ValidarPrecio(txtPrecio.Text, out precio));
                bool descripcionOk = MostrarResultado(txtDescripcion, lblErrorDescripcion,
                    descripcion.Length > 150 ? "La descripción no puede superar los 150 caracteres." : null);
                bool imagenOk = MostrarResultado(txtImagenUrl, lblErrorImagenUrl,
                    imagenUrl.Length > 1000 ? "La URL de la imagen no puede superar los 1000 caracteres." : null);

                if (!(codigoOk && nombreOk && precioOk && descripcionOk && imagenOk))
                {
                    MostrarAlertaErrores();
                    return;
                }

                ArticuloNegocio negocio = new ArticuloNegocio();

                // validar código duplicado
                List<Articulo> listaActual = negocio.Listar();
                bool codigoExiste = false;
                string codigoIngresado = codigo.ToUpper();

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
                    //Antes este mensaje se armaba concatenando el código dentro de un string de JavaScript:
                    //un código como  ');alert(1);//  ejecutaba JS. Ahora es texto común y MostrarResultado lo codifica.
                    MostrarResultado(txtCodigo, lblErrorCodigo, "El código \"" + codigo + "\" ya está registrado en otro artículo. Ingresá un código único.");
                    MostrarAlertaErrores();
                    return;
                }

                //fin de las validaciones

                Articulo nuevo = new Articulo();

                // paso los datos HTML a un objeto
                nuevo.Codigo = codigo;
                nuevo.Nombre = nombre;
                nuevo.Descripcion = descripcion;
                nuevo.ImagenUrl = imagenUrl;
                nuevo.Precio = precio;

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
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Session.Add("error", "No se pudo guardar el artículo. Revisá los datos e intentá nuevamente.");
                Response.Redirect("Error.aspx", false);
            }
        }

        //----- Reglas de negocio: las mismas que el objeto "reglas" del JS en ArticuloForm.aspx -----
        //Cada método devuelve el mensaje de error, o null si el valor es válido.

        private string ValidarCodigo(string codigo)
        {
            if (codigo == "")
                return "El código es obligatorio.";
            if (codigo.Length > 50)
                return "El código no puede superar los 50 caracteres.";
            if (ArticuloNegocio.TryParsePrecio(codigo, out decimal numero) && numero < 0)
                return "El código no puede ser un número negativo.";
            return null;
        }

        private string ValidarNombre(string nombre)
        {
            if (nombre == "")
                return "El nombre es obligatorio.";
            if (nombre.Length < 3)
                return "El nombre tiene que tener al menos 3 caracteres.";
            if (nombre.Length > 50)
                return "El nombre no puede superar los 50 caracteres.";
            return null;
        }

        private string ValidarPrecio(string texto, out decimal precio)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                precio = 0;
                return "El precio es obligatorio.";
            }
            //TryParsePrecio: acepta coma o punto y no depende del idioma del servidor (igual que en el filtro).
            if (!ArticuloNegocio.TryParsePrecio(texto, out precio))
                return "El precio tiene que ser un número (podés usar coma o punto para los decimales).";
            if (precio <= 0)
                return "El precio tiene que ser mayor a 0.";
            if (precio > ArticuloNegocio.PrecioMaximo)
                return "El precio ingresado es demasiado grande.";
            return null;
        }

        //----- Feedback de Bootstrap desde el servidor -----

        //Si hay error, pinta el TextBox de rojo (is-invalid) y escribe el mensaje en su invalid-feedback.
        //Devuelve true si el campo es válido.
        private bool MostrarResultado(TextBox txt, Label lbl, string error)
        {
            if (error == null)
                return true;

            txt.CssClass = "form-control is-invalid";
            //El Label escribe el Text tal cual en el HTML: codifico acá, en un solo lugar,
            //por si el mensaje incluye algo que escribió el usuario (ej: el código duplicado).
            lbl.Text = HttpUtility.HtmlEncode(error);
            return false;
        }

        private void MostrarAlertaErrores()
        {
            alertaErrores.Attributes["class"] = "alert alert-danger";
        }

        private void LimpiarErrores()
        {
            foreach (TextBox txt in new[] { txtCodigo, txtNombre, txtPrecio, txtDescripcion, txtImagenUrl })
            {
                txt.CssClass = "form-control";
            }
            foreach (Label lbl in new[] { lblErrorCodigo, lblErrorNombre, lblErrorPrecio, lblErrorDescripcion, lblErrorImagenUrl })
            {
                lbl.Text = "";
            }
            alertaErrores.Attributes["class"] = "alert alert-danger d-none";
        }
    }
}
