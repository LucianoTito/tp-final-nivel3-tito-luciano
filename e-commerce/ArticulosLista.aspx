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
            <% if (FiltroAvanzado) { %>
                <div class="row mb-4 bg-light p-3 rounded border shadow-sm">
                   
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
                        </div>
                    </div>
                    
                    <div class="col-md-3 d-flex align-items-end mb-3">
                        
                        <asp:Button Text="🔍 Buscar" runat="server" CssClass="btn btn-primary me-2 w-50" ID="btnBuscar" OnClick="btnBuscar_Click" />
                        <asp:Button Text="🧹 Limpiar" runat="server" CssClass="btn btn-outline-secondary w-50" ID="btnLimpiar" OnClick="btnLimpiar_Click" />
                    </div>
                </div>
            <% } %>
      

            <%-- grilla de datos  --%>
            <div class="table-responsive shadow-sm rounded">
                <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-striped table-hover table-bordered mb-0 align-middle" 
                    AutoGenerateColumns="false" DataKeyNames="Id" OnSelectedIndexChanged="dgvArticulos_SelectedIndexChanged">
                    
                    <HeaderStyle CssClass="table-dark" />
                    
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
</asp:Content>