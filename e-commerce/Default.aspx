<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="e_commerce.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="mb-4">¡Bienvenido a mi Tienda Virtual!</h1>

    <%--  BARRA DE BÚSQUEDA --%>
    <div class="row mb-4">
        <div class="col-md-6">
            <div class="input-group shadow-sm">
                <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" placeholder="Buscar producto..." />
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" CssClass="btn btn-success" OnClick="btnBuscar_Click" />
                <asp:Button Text="Limpiar" runat="server" ID="btnLimpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>

    <%-- CARTEL FLASH DE FAVORITOS --%>
    <% if (Session["mensajeFav"] != null) { %>
        <div class="alert alert-success alert-dismissible fade show text-center fw-bold shadow-sm" role="alert">
            <%= Session["mensajeFav"].ToString() %>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <%-- borro el mensaje de la memoria para que no vuelva a salir si el usuario aprieta F5 --%>
        <% Session.Remove("mensajeFav"); %>
    <% } %>
<%--  GRILLA DE TARJETAS --%>
    <div class="row row-cols-1 row-cols-md-4 g-4 mb-5">
        
        <% foreach (Dominio.Articulo art in ListaArticulos) { %>
            
            <div class="col">
                <div class="card h-100 shadow-sm">
                    <img src="<%: art.ImagenUrl %>" class="card-img-top p-2" alt="<%: art.Nombre %>" 
                         onerror="this.src='https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png'"
                         style="max-height: 200px; object-fit: contain;">
                    
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title"><%: art.Nombre %></h5>
                        <p class="card-text flex-grow-1 text-muted"><%: art.Descripcion %></p>
                        <p class="card-text fs-5 text-success fw-bold">$ <%: art.Precio.ToString("N2") %></p>
                        
                        <%-- Botonera inferior de la tarjeta --%>
                        <div class="d-flex justify-content-between align-items-center mt-auto">
                            
                            <a href="Detalle.aspx?id=<%: art.Id %>" class="btn btn-primary w-100 me-2">Ver Detalles</a>
                            
                            <%-- Lógica de Favoritos --%>
                            <% if (Negocio.Seguridad.sesionActiva(Session["usuario"])) { %>
                                
                                <% if (ListaFavoritosUsuario.Contains(art.Id)) { %>
                                    <%-- ya es favorito: btn p/ quitar --%>
                                    <a href="Default.aspx?idRm=<%: art.Id %>" class="btn btn-danger" title="Quitar de Favoritos">
                                        ❤️
                                    </a>
                                <% } else { %>
                                    <%-- no es favorito: btn rojo p/ agregar --%>
                                    <a href="Default.aspx?idAdd=<%: art.Id %>" class="btn btn-outline-danger" title="Agregar a Favoritos">
                                        🤍
                                    </a>
                                <% } %>

                            <% } %>
                        </div>
                    </div>
                </div>
            </div>

        <% } %>

    </div>

</asp:Content>