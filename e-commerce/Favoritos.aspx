<%@ Page Title="Mis Favoritos" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="e_commerce.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4 fw-bold">❤️ Mis Artículos Favoritos</h2>

        <%-- mje si la lista está vacía --%>
        <% if (ListaFavoritos == null || ListaFavoritos.Count == 0) { %>
            <div class="alert alert-primary text-center shadow-sm">
                Aún no tenés artículos en tu lista de favoritos. ¡Andá al catálogo y guardá los que más te gusten!
            </div>
            <div class="text-center mt-3">
                <a href="Default.aspx" class="btn btn-primary">Ir al Catálogo</a>
            </div>
        <% } else { %>
            
            <%-- Grilla --%>
            <div class="row row-cols-1 row-cols-md-4 g-4">
                <% foreach (Dominio.Articulo art in ListaFavoritos) { %>
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
                                
                                <div class="mt-auto">
                                    <a href="Detalle.aspx?id=<%: art.Id %>" class="btn btn-primary w-100 mb-2">Ver Detalles</a>
                                    <%-- btn para sacarlo directamente. outline-secondary y no rojo: quitar un favorito
                                         no borra datos y se deshace con un click, no es una acción "peligrosa" --%>
                                    <a href="Default.aspx?idRm=<%: art.Id %>" class="btn btn-outline-secondary w-100">💔 Quitar de la lista</a>
                                </div>
                            </div>
                        </div>
                    </div>
                <% } %>
            </div>
            
        <% } %>
    </div>
</asp:Content>