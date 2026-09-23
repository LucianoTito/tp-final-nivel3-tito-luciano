<%@ Page Title="Administración de Artículos" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ArticulosLista.aspx.cs" Inherits="e_commerce.ArticulosLista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4">
        <div class="col">
            <h2 class="fw-bold">Gestión de Artículos</h2>
            <hr />
        </div>
    </div>

    <div class="row mb-4">
        <div class="col">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanelFiltro" runat="server">
        <ContentTemplate>
          <%-- filtro rápido y chkBox --%>
            <div class="row mb-4 align-items-end">
                <div class="col-md-6">
                    <asp:Label Text="Búsqueda Rápida:" runat="server" CssClass="form-label fw-bold" />
                    
                    
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged" placeholder="Ej: Samsung, Celular..." />
                        <asp:Button Text="🧹" runat="server" CssClass="btn btn-outline-secondary" ID="btnLimpiarRapido" OnClick="btnLimpiarRapido_Click" ToolTip="Limpiar Búsqueda Rápida" />
                    </div>
                </div>
                
                <div class="col-md-6 d-flex align-items-end mb-2">
                    <div class="form-check">
                        <asp:CheckBox runat="server" ID="chkAvanzado" AutoPostBack="true" OnCheckedChanged="chkAvanzado_CheckedChanged" />
                        <label class="form-check-label fw-bold" for="<%= chkAvanzado.ClientID %>">Habilitar Filtro Avanzado</label>
                    </div>
                </div>
            </div>

            <%--filtro avanzado con renderizado cond. --%>
            <%-- asp:Panel con DefaultButton: si se aprieta Enter en el filtro, se "clickea" btnBuscar
                 (y pasa por la validación JS). Sin esto, Enter dispara el primer botón del form: "Salir" del navbar --%>
            <% if (FiltroAvanzado) { %>
                <asp:Panel runat="server" DefaultButton="btnBuscar" CssClass="row mb-4 bg-light p-3 rounded border shadow-sm">
                   
                    <div class="col-md-3">
                        <div class="mb-3">
                            <asp:Label Text="Campo" runat="server" CssClass="form-label fw-bold" />
                            <asp:DropDownList runat="server" ID="ddlCampo" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">       
                                <asp:ListItem Text="Código" />
                                <asp:ListItem Text="Precio" />
                                <asp:ListItem Text="Nombre" />
                                <asp:ListItem Text="Marca" />
                                <asp:ListItem Text="Categoría" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-3">
                        <div class="mb-3">
                            <asp:Label Text="Criterio" runat="server" CssClass="form-label fw-bold" />
                            <asp:DropDownList runat="server" ID="ddlCriterio" CssClass="form-select"></asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-3">
                        <div class="mb-3">
                            <asp:Label Text="Filtro" runat="server" CssClass="form-label fw-bold" />
                            <asp:TextBox runat="server" ID="txtFiltroAvanzado" CssClass="form-control" />
                            <%-- mensaje de error: Bootstrap solo lo muestra si el TextBox de arriba tiene is-invalid --%>
                            <asp:Label runat="server" ID="lblErrorFiltro" CssClass="invalid-feedback" />
                        </div>
                    </div>
                    
                    <div class="col-md-3 d-flex align-items-end mb-3">
                        
                        <asp:Button Text="🔍 Buscar" runat="server" CssClass="btn btn-primary me-2 w-50" ID="btnBuscar" OnClick="btnBuscar_Click" OnClientClick="return validarFiltroAvanzado();" />
                        <asp:Button Text="🧹 Limpiar" runat="server" CssClass="btn btn-outline-secondary w-50" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                    </div>
                </asp:Panel>
            <% } %>
      

            <%-- grilla de datos  --%>
            <div class="table-responsive shadow-sm rounded">
                <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-striped table-hover table-bordered mb-0 align-middle" 
                    AutoGenerateColumns="false" DataKeyNames="Id" OnSelectedIndexChanged="dgvArticulos_SelectedIndexChanged">
                    
                    <HeaderStyle CssClass="table-dark" />

                    <%-- se muestra en lugar de la grilla cuando la lista viene vacía --%>
                    <EmptyDataTemplate>
                        <div class="text-center text-muted py-4">No se encontraron artículos con ese criterio.</div>
                    </EmptyDataTemplate>

                    <Columns>
                        <asp:BoundField HeaderText="Código" DataField="Codigo" />
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                        <asp:BoundField HeaderText="Marca" DataField="Marca.Descripcion" />
                        <asp:BoundField HeaderText="Categoría" DataField="Categoria.Descripcion" />
                        <asp:BoundField HeaderText="Precio" DataField="Precio" DataFormatString="{0:C2}" />
                        
                        <asp:CommandField HeaderText="Acción" ShowSelectButton="true" SelectText="✍️ Editar" ControlStyle-CssClass="btn btn-sm btn-outline-primary" />
                    </Columns>
                </asp:GridView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
        </div>
    </div>

    <div class="row">
        <div class="col">
            <%-- Btn para ir al formulario de alta --%>
            <a href="ArticuloForm.aspx" class="btn btn-success fw-bold">➕ Agregar Nuevo Artículo</a>
        </div>
    </div>

    <%-- Este script está FUERA del UpdatePanel: se carga una sola vez con la página y sus funciones
         siguen existiendo después de cada postback parcial (el UpdatePanel solo reemplaza el HTML de adentro). --%>
    <script>
        // Límite del tipo money de SQL Server (la columna Precio). Un número más grande hace fallar la consulta.
        var PRECIO_MAXIMO = 922337203685477;

        // Mismas reglas que ValidarFiltroAvanzado en ArticulosLista.aspx.cs. Devuelve el mensaje de error o null.
        function obtenerErrorFiltro(campo, valor) {
            if (valor === "") {
                return "Ingresá un valor para buscar.";
            }

            if (campo === "Precio") {
                // acepto coma o punto como separador decimal
                var normalizado = valor.replace(",", ".");

                // solo dígitos, con signo y punto decimal opcionales (rechaza letras, "1e3", "1.000,50", etc.)
                if (!/^[+-]?(\d+\.?\d*|\.\d+)$/.test(normalizado)) {
                    return "El precio tiene que ser un número (podés usar coma o punto para los decimales).";
                }

                var precio = parseFloat(normalizado);

                if (precio < 0) {
                    return "El precio no puede ser negativo.";
                }
                if (precio > PRECIO_MAXIMO) {
                    return "El precio ingresado es demasiado grande.";
                }
            }

            return null;
        }

        // Se ejecuta al apretar Buscar (OnClientClick). Si devuelve false, se cancela el postback.
        function validarFiltroAvanzado() {
            // Busco los elementos en el momento del click, no al cargar la página: después de cada
            // postback parcial el UpdatePanel los reemplaza por elementos nuevos.
            var txtFiltro = document.getElementById("<%= txtFiltroAvanzado.ClientID %>");
            var ddlCampo = document.getElementById("<%= ddlCampo.ClientID %>");
            var lblError = document.getElementById("<%= lblErrorFiltro.ClientID %>");

            // si el filtro no está en pantalla, dejo que decida el servidor
            if (!txtFiltro || !ddlCampo || !lblError) {
                return true;
            }

            var campo = ddlCampo.options[ddlCampo.selectedIndex].text;
            var mensaje = obtenerErrorFiltro(campo, txtFiltro.value.trim());

            if (mensaje) {
                lblError.textContent = mensaje;
                txtFiltro.classList.add("is-invalid");
                txtFiltro.focus();
                return false;
            }

            txtFiltro.classList.remove("is-invalid");
            return true;
        }

        // Cuando el usuario corrige el valor, saco el rojo. Escucho en "document" (delegación de eventos)
        // porque el TextBox se vuelve a crear en cada postback parcial: un listener puesto directamente
        // sobre él se perdería junto con el elemento viejo.
        document.addEventListener("input", function (e) {
            if (e.target.id === "<%= txtFiltroAvanzado.ClientID %>") {
                e.target.classList.remove("is-invalid");
            }
        });
    </script>
</asp:Content>