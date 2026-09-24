<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="e_commerce.Detalle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <% if (ArticuloSeleccionado != null) { %>

        <div class="row mt-5">
            
            <%-- mb-4 mb-md-0: en celular la imagen queda arriba del texto, así no se pegan --%>
            <div class="col-md-6 mb-4 mb-md-0">
                <%-- onerror: primero se anula a sí mismo (this.onerror=null) para no quedar en bucle si sin-imagen.svg también fallara --%>
                <div class="articulo-img-detalle shadow">
                    <img src="<%: ArticuloSeleccionado.ImagenUrl %>" alt="<%: ArticuloSeleccionado.Nombre %>"
                         onerror="this.onerror=null; this.src='Images/sin-imagen.svg';" />
                </div>
            </div>

           
            <div class="col-md-6">
                <h2 class="fw-bold"><%: ArticuloSeleccionado.Nombre %></h2>
                <p class="text-muted">Código de Producto: <%: ArticuloSeleccionado.Codigo %></p>
                
                <h3 class="precio mt-4">$ <%: ArticuloSeleccionado.Precio.ToString("N2") %></h3>
                
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
