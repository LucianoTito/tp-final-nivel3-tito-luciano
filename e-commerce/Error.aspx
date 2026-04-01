<%@ Page Title="Error" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="e_commerce.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center align-items-center" style="min-height: 60vh;">
        <div class="col-md-6 col-lg-5">
            <div class="card border-danger shadow-lg text-center p-4">
                <div class="card-body">
                    <h1 class="display-1 text-danger mb-3">⚠️</h1>
                    <h3 class="card-title fw-bold text-dark">¡Ups! Algo salió mal</h3>
                    
                    <%-- Etiqueta donde se inyecta el mje de error específico --%>
                    <p class="card-text fs-5 text-secondary mt-3">
                        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                    </p>

                    <div class="mt-4">
                        <asp:Button ID="btnVolver" runat="server" Text="Volver al Inicio" CssClass="btn btn-outline-danger btn-lg px-4" OnClick="btnVolver_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>