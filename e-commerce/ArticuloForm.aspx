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

        <div class="row">
            <%-- COLUMNA IZQUIERDA: Formulario de datos --%>
            <div class="col-md-6">
                
                <div class="mb-3">
                    <label for="txtCodigo" class="form-label fw-bold">Código de Artículo <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
                
                <div class="mb-3">
                    <label for="txtNombre" class="form-label fw-bold">Nombre <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
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
                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" TextMode="Number" step="0.01" min="0" ClientIDMode="Static"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label for="txtDescripcion" class="form-label fw-bold">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </div>

                <%-- BOTONERA DE ACCIÓN --%>
                <div class="mt-4 mb-5">
                    <asp:Button ID="btnAceptar" runat="server" Text="💾 Guardar Artículo" CssClass="btn btn-primary me-2" OnClick="btnAceptar_Click" OnClientClick="return validar();" />
                    <a href="ArticulosLista.aspx" class="btn btn-outline-secondary me-2">Cancelar</a>
                   <asp:Button ID="btnEliminar" runat="server" Text="🗑️ Eliminar Físicamente" CssClass="btn btn-danger" OnClick="btnEliminar_Click" OnClientClick="return confirm('¿Está seguro que desea eliminar de forma permanente este artículo?');" Visible="false" />
                </div>
            </div>

            <%-- COLUMNA DERECHA: Imagen Dinámica con AJAX --%>
            <div class="col-md-6 d-flex flex-column align-items-center">
                
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                
                <asp:UpdatePanel ID="UpdatePanelImagen" runat="server" class="w-100">
                    <ContentTemplate>
                        <div class="mb-3">
                            <label for="txtImagenUrl" class="form-label fw-bold">URL de la Imagen</label>

                            
                            <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtImagenUrl_TextChanged"></asp:TextBox>
                        </div>
                        
                        <div class="text-center mt-4">
                            <asp:Image ID="imgArticulo" runat="server" 
                                ImageUrl="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" 
                                CssClass="img-fluid rounded shadow" style="max-height: 400px; object-fit: contain;" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
   
   <script>
       // función que evalúa y pinta los campos 
       function evaluarCampo(input) {
           let valor = input.value.trim();
           let esValido = true;

           
           if (valor === "") {
               esValido = false;
           }
           
           else if (input.id === "txtPrecio") {
               if (Number(valor) < 0) esValido = false;
           }
           //no puede ser un número negativo aislado 
           else if (input.id === "txtCodigo") {
               // si es un número válido y es menor a 0, lo rechazo
               if (!isNaN(valor) && Number(valor) < 0) esValido = false;
           }

           // pinto de rojo o verde
           if (!esValido) {
               input.classList.add("is-invalid");
               input.classList.remove("is-valid");
           } else {
               input.classList.remove("is-invalid");
               input.classList.add("is-valid");
           }

           return esValido;
       }

       // escucho cuando el DOM termina de cargar
       document.addEventListener("DOMContentLoaded", function () {
           const txtCodigo = document.getElementById("txtCodigo");
           const txtNombre = document.getElementById("txtNombre");
           const txtPrecio = document.getElementById("txtPrecio");

           // inyecto los eventos
           [txtCodigo, txtNombre, txtPrecio].forEach(input => {
               if (input) {

                   // Evento 1: cuando el cursor sale del campo
                   input.addEventListener("blur", function () {
                       evaluarCampo(this);
                   });

                   // Evento 2: cuando el usuario escribe (en tiempo real)
                   input.addEventListener("input", function () {
                       if (this.classList.contains("is-invalid")) {
                           evaluarCampo(this);
                       }
                   });
               }
           });
       });

       // función principal que se ejecuta al presionar guardar
       function validar() {
           const txtCodigo = document.getElementById("txtCodigo");
           const txtNombre = document.getElementById("txtNombre");
           const txtPrecio = document.getElementById("txtPrecio");

           // evaluo todos para que se pinten de rojo o verde
           let v1 = evaluarCampo(txtCodigo);
           let v2 = evaluarCampo(txtNombre);
           let v3 = evaluarCampo(txtPrecio);

           let esValido = v1 && v2 && v3;

          
           if (!esValido) {
               let valorPrecio = txtPrecio.value.trim();
               let valorCodigo = txtCodigo.value.trim();

               if (valorPrecio !== "" && Number(valorPrecio) < 0) {
                   alert("⛔ El precio no puede ser un número negativo. Por favor ingrese un número positivo");
               }
               else if (valorCodigo !== "" && !isNaN(valorCodigo) && Number(valorCodigo) < 0) {
                   alert("⛔ El código de artículo no puede ser un número negativo. El código solo puede contener números positivos, además de letras (opcional).");
               }
               else {
                   alert("⚠️ Por favor, completá correctamente los campos obligatorios remarcados en rojo.");
               }
           }

           return esValido;
       }
   </script>
</asp:Content>