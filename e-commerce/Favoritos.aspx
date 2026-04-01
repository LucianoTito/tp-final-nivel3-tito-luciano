<%@ Page Title="Mis Favoritos" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="e_commerce.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h2 class="mb-4 fw-bold">❤️ Mis Artículos Favoritos</h2>

        <%-- mje si la lista está vacía --%>
        <% if (ListaFavoritos == null || ListaFavoritos.Count == 0) { %>
            <div class="alert alert-info text-center shadow-sm">
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
                            <img src="<%: art.ImagenUrl %>" class="card-img-top p-2" alt="<%: art.Nombre %>" 
                                 onerror="this.src='https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png'"
                                 style="max-height: 200px; object-fit: contain;">
                            
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title"><%: art.Nombre %></h5>
                                <p class="card-text flex-grow-1 text-muted"><%: art.Descripcion %></p>
                                <p class="card-text fs-5 text-success fw-bold">$ <%: art.Precio.ToString("N2") %></p>
                                
                                <div class="mt-auto">
                                    <a href="Detalle.aspx?id=<%: art.Id %>" class="btn btn-primary w-100 mb-2">Ver Detalles</a>
                                    <%-- btn para sacarlo directamente --%>
                                    <a href="Default.aspx?idRm=<%: art.Id %>" class="btn btn-outline-danger w-100">💔 Quitar de la lista</a>
                                </div>
                            </div>
                        </div>
                    </div>
                <% } %>
            </div>
            
        <% } %>
    </div>
</asp:Content>