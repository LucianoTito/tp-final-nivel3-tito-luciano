<%@ Page Title="Formulario de Artículo" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ArticuloForm.aspx.cs" Inherits="e_commerce.ArticuloForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row mb-4">
            <div class="col">
                <h2 class="fw-bold">Gestión de Artículo</h2>
                <hr />
            </div>
        </div>

        <%-- mensaje general: lo muestra el JS (sacando d-none) o el servidor, cuando hay algún campo inválido --%>
        <div id="alertaErrores" runat="server" ClientIDMode="Static" class="alert alert-danger d-none" role="alert">
            ⚠️ No se pudo guardar: revisá los campos marcados en rojo.
        </div>

        <div class="row">
            <%-- COLUMNA IZQUIERDA: Formulario de datos --%>
            <%-- asp:Panel con DefaultButton: Enter en un campo "clickea" Guardar (y pasa por validar()).
                 Sin esto, Enter dispara el primer botón del form, que es "Salir" del navbar --%>
            <asp:Panel runat="server" DefaultButton="btnAceptar" CssClass="col-md-6">

                <%-- cada invalid-feedback va justo después de su TextBox: Bootstrap solo lo muestra si el TextBox tiene is-invalid --%>
                <div class="mb-3">
                    <label for="txtCodigo" class="form-label fw-bold">Código de Artículo <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="lblErrorCodigo" runat="server" CssClass="invalid-feedback" />
                </div>

                <div class="mb-3">
                    <label for="txtNombre" class="form-label fw-bold">Nombre <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="lblErrorNombre" runat="server" CssClass="invalid-feedback" />
                </div>
                
                <div class="row">
                    <div class="col-md-6 mb-3">
                        <label for="ddlMarca" class="form-label fw-bold">Marca</label>
                        <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-6 mb-3">
                        <label for="ddlCategoria" class="form-label fw-bold">Categoría</label>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                </div>

              <div class="mb-3">
                    <label for="txtPrecio" class="form-label fw-bold">Precio ($) <span class="text-danger">*</span></label>
                    <%-- campo de texto (no type="number") para aceptar coma o punto y que los errores
                         los muestre Bootstrap y no los globos nativos del navegador.
                         inputmode="decimal" hace que el celular muestre el teclado numérico --%>
                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" inputmode="decimal" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="lblErrorPrecio" runat="server" CssClass="invalid-feedback" />
                </div>

                <div class="mb-3">
                    <label for="txtDescripcion" class="form-label fw-bold">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="lblErrorDescripcion" runat="server" CssClass="invalid-feedback" />
                </div>

                <%-- BOTONERA DE ACCIÓN --%>
                <div class="mt-4 mb-5">
                    <asp:Button ID="btnAceptar" runat="server" Text="💾 Guardar Artículo" CssClass="btn btn-primary me-2" OnClick="btnAceptar_Click" OnClientClick="return validar();" />
                    <a href="ArticulosLista.aspx" class="btn btn-outline-secondary me-2">Cancelar</a>
                   <asp:Button ID="btnEliminar" runat="server" Text="🗑️ Eliminar Físicamente" CssClass="btn btn-danger" OnClick="btnEliminar_Click" OnClientClick="return confirm('¿Está seguro que desea eliminar de forma permanente este artículo?');" Visible="false" />
                </div>
            </asp:Panel>

            <%-- COLUMNA DERECHA: Imagen Dinámica con AJAX --%>
            <div class="col-md-6 d-flex flex-column align-items-center">
                
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                
                <asp:UpdatePanel ID="UpdatePanelImagen" runat="server" class="w-100">
                    <ContentTemplate>
                        <div class="mb-3">
                            <label for="txtImagenUrl" class="form-label fw-bold">URL de la Imagen</label>

                            
                            <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtImagenUrl_TextChanged" ClientIDMode="Static"></asp:TextBox>
                            <asp:Label ID="lblErrorImagenUrl" runat="server" CssClass="invalid-feedback" />
                        </div>
                        
                        <%-- mismo marco que en Detalle, así el admin ve la imagen como la va a ver el cliente --%>
                        <%-- onerror: primero se anula a sí mismo (this.onerror=null) para no quedar en bucle si sin-imagen.svg también fallara --%>
                        <div class="articulo-img-detalle shadow mt-4">
                            <asp:Image ID="imgArticulo" runat="server"
                                ImageUrl="~/Images/sin-imagen.svg"
                                AlternateText="Vista previa de la imagen del artículo"
                                onerror="this.onerror=null; this.src='Images/sin-imagen.svg';" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
   
   <script>
       // Límite del tipo money de SQL Server (columna Precio). Mismo valor que ArticuloNegocio.PrecioMaximo.
       var PRECIO_MAXIMO = 922337203685477;

       // Convierte "10,5" o "10.5" en número; devuelve null si no es un número válido.
       // Mismas reglas que ArticuloNegocio.TryParsePrecio en el servidor.
       function leerDecimal(valor) {
           var normalizado = valor.trim().replace(/,/g, ".");

           if (!/^[+-]?(\d+\.?\d*|\.\d+)$/.test(normalizado)) {
               return null;
           }
           return parseFloat(normalizado);
       }

       // Reglas de cada campo (por id). Cada una recibe el valor sin espacios al principio y al final
       // y devuelve el mensaje de error, o null si está bien. Son las mismas que en ArticuloForm.aspx.cs.
       var reglas = {
           txtCodigo: function (v) {
               if (v === "") return "El código es obligatorio.";
               if (v.length > 50) return "El código no puede superar los 50 caracteres.";
               var numero = leerDecimal(v);
               if (numero !== null && numero < 0) return "El código no puede ser un número negativo.";
               return null;
           },
           txtNombre: function (v) {
               if (v === "") return "El nombre es obligatorio.";
               if (v.length < 3) return "El nombre tiene que tener al menos 3 caracteres.";
               if (v.length > 50) return "El nombre no puede superar los 50 caracteres.";
               return null;
           },
           txtPrecio: function (v) {
               if (v === "") return "El precio es obligatorio.";
               var precio = leerDecimal(v);
               if (precio === null) return "El precio tiene que ser un número (podés usar coma o punto para los decimales).";
               if (precio <= 0) return "El precio tiene que ser mayor a 0.";
               if (precio > PRECIO_MAXIMO) return "El precio ingresado es demasiado grande.";
               return null;
           },
           txtDescripcion: function (v) {
               if (v.length > 150) return "La descripción no puede superar los 150 caracteres.";
               return null;
           },
           txtImagenUrl: function (v) {
               if (v.length > 1000) return "La URL de la imagen no puede superar los 1000 caracteres.";
               return null;
           }
       };

       // Evalúa un campo, lo pinta de rojo o verde y escribe el mensaje en su invalid-feedback.
       function evaluarCampo(input) {
           var mensaje = reglas[input.id](input.value.trim());
           var feedback = input.parentNode.querySelector(".invalid-feedback");

           if (mensaje) {
               // textContent (no innerHTML): el texto se muestra tal cual, nunca se interpreta como HTML
               feedback.textContent = mensaje;
               input.classList.add("is-invalid");
               input.classList.remove("is-valid");
               return false;
           }

           input.classList.remove("is-invalid");
           input.classList.add("is-valid");
           return true;
       }

       // Escucho en "document" (delegación de eventos) y no en cada TextBox: txtImagenUrl está dentro
       // del UpdatePanel y se vuelve a crear en cada postback parcial, y un listener puesto directamente
       // sobre él se perdería. Uso "focusout" porque, a diferencia de "blur", sube hasta document.
       document.addEventListener("focusout", function (e) {
           if (reglas[e.target.id]) {
               evaluarCampo(e.target);
           }
       });

       // Mientras escribe, solo re-evalúo si el campo ya estaba en rojo (para no retarlo antes de tiempo).
       document.addEventListener("input", function (e) {
           if (reglas[e.target.id] && e.target.classList.contains("is-invalid")) {
               evaluarCampo(e.target);
           }
       });

       // Se ejecuta al presionar Guardar (OnClientClick). Si devuelve false, se cancela el postback.
       function validar() {
           var primeroInvalido = null;

           // evalúo TODOS los campos (no corto en el primero) para que se pinten todos los errores juntos
           Object.keys(reglas).forEach(function (id) {
               var input = document.getElementById(id);
               if (input && !evaluarCampo(input) && primeroInvalido === null) {
                   primeroInvalido = input;
               }
           });

           var alerta = document.getElementById("alertaErrores");

           if (primeroInvalido) {
               alerta.classList.remove("d-none");
               primeroInvalido.focus();
               return false;
           }

           alerta.classList.add("d-none");
           return true;
       }
   </script>
</asp:Content>