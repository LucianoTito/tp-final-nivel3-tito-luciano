<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="e_commerce.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--  BARRA DE BÚSQUEDA --%>
    <div class="row mb-4">
        <div class="col-md-6">
            <div class="input-group shadow-sm">
                <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control" placeholder="Buscar producto..." />
                <asp:Button Text="Buscar" runat="server" ID="btnBuscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                <asp:Button Text="Limpiar" runat="server" ID="btnLimpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>

    <%-- CARTEL FLASH DE FAVORITOS --%>
    <% if (Session["mensajeFav"] != null) { %>
        <div class="alert alert-success alert-dismissible fade show text-center fw-bold shadow-sm" role="alert">
            <%-- <%: %> codifica el texto (regla 4): aunque hoy el mensaje es fijo, si algún día incluye un dato
                 (ej: el nombre del artículo), no se podría inyectar HTML --%>
            <%: Session["mensajeFav"] %>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <%-- borro el mensaje de la memoria para que no vuelva a salir si el usuario aprieta F5 --%>
        <% Session.Remove("mensajeFav"); %>
    <% } %>
<%--  GRILLA DE TARJETAS --%>
    <div class="row row-cols-1 row-cols-md-4 g-4">
        
        <% foreach (Dominio.Articulo art in ListaArticulos) { %>
            
            <div class="col">
                <div class="card h-100 shadow-sm">
                    <%-- onerror: primero se anula a sí mismo (this.onerror=null) para no quedar en bucle si sin-imagen.svg también fallara --%>
                    <div class="articulo-img-tarjeta">
                        <img src="<%: art.ImagenUrl %>" alt="<%: art.Nombre %>"
                             onerror="this.onerror=null; this.src='Images/sin-imagen.svg';">
                    </div>
                    
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title"><%: art.Nombre %></h5>
                        <p class="card-text flex-grow-1 text-muted"><%: art.Descripcion %></p>
                        <p class="card-text fs-5 precio">$ <%: art.Precio.ToString("N2") %></p>
                        
                        <%-- Botonera inferior de la tarjeta --%>
                        <div class="d-flex justify-content-between align-items-center mt-auto">
                            
                            <a href="Detalle.aspx?id=<%: art.Id %>" class="btn btn-primary w-100 me-2">Ver Detalles</a>
                            
                            <%-- Lógica de Favoritos: color de acento (ámbar), no rojo, porque no es un error ni algo que se borra.
                                 ♥ / ♡ son caracteres de texto (toman el color del botón); los emoji ❤️ 🤍 traen su propio color fijo.
                                 aria-label: para que un lector de pantalla diga la acción y no "corazón negro" --%>
                            <% if (Negocio.Seguridad.sesionActiva(Session["usuario"])) { %>

                                <% if (ListaFavoritosUsuario.Contains(art.Id)) { %>
                                    <%-- ya es favorito: btn p/ quitar --%>
                                    <a href="Default.aspx?idRm=<%: art.Id %>" class="btn btn-acento" title="Quitar de Favoritos" aria-label="Quitar de Favoritos">
                                        &#9829;
                                    </a>
                                <% } else { %>
                                    <%-- no es favorito: btn p/ agregar --%>
                                    <a href="Default.aspx?idAdd=<%: art.Id %>" class="btn btn-outline-acento" title="Agregar a Favoritos" aria-label="Agregar a Favoritos">
                                        &#9825;
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