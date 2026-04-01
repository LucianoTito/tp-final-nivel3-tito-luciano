<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="e_commerce.Detalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <% if (ArticuloSeleccionado != null) { %>

        <div class="row mt-5">
            
            <div class="col-md-6 text-center">
                <img src="<%: ArticuloSeleccionado.ImagenUrl %>" class="img-fluid rounded shadow" alt="<%: ArticuloSeleccionado.Nombre %>" 
                     onerror="this.src='https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png'" 
                     style="max-height: 500px; object-fit: contain;" />
            </div>

           
            <div class="col-md-6">
                <h2 class="fw-bold"><%: ArticuloSeleccionado.Nombre %></h2>
                <p class="text-muted">Código de Producto: <%: ArticuloSeleccionado.Codigo %></p>
                
                <h3 class="text-success fw-bold mt-4">$ <%: ArticuloSeleccionado.Precio.ToString("N2") %></h3>
                
                <p class="mt-4 fs-5"><%: ArticuloSeleccionado.Descripcion %></p>

                <ul class="list-group mt-4">
                    <li class="list-group-item"><strong>Marca:</strong> <%: ArticuloSeleccionado.Marca.Descripcion %></li>
                    <li class="list-group-item"><strong>Categoría:</strong> <%: ArticuloSeleccionado.Categoria.Descripcion %></li>
                </ul>

                <a href="Default.aspx" class="btn btn-outline-secondary mt-4">⬅ Volver al Catálogo</a>
            </div>
        </div>

    <% } else { %>

        <div class="alert alert-warning text-center mt-5" role="alert">
            <h4 class="alert-heading">¡Ups!</h4>
            <p>No se pudo encontrar el artículo solicitado o no existe.</p>
            <hr>
            <a href="Default.aspx" class="btn btn-primary">Volver al Catálogo</a>
        </div>

    <% } %>

</asp:Content>
