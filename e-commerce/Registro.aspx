<%@ Page Title="Registro de Usuario" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="e_commerce.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5 mb-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-5">
                
                <div class="card shadow-sm border-0 rounded-3">
                    <div class="card-body p-5">
                        <div class="text-center mb-4">
                            <h3 class="fw-bold">Crea tu cuenta</h3>
                            <p class="text-muted">Sumate para guardar tus favoritos y comprar</p>
                        </div>
                        
                        <div class="mb-3">
                            <label class="form-label fw-bold">Correo Electrónico</label>
                            
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" placeholder="ejemplo@correo.com" required></asp:TextBox>
                        </div>
                        
                        <div class="mb-3">
                            <label class="form-label fw-bold">Contraseña</label>
                            <asp:TextBox runat="server" ID="txtPass" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                        </div>
                        
                        <div class="mb-4">
                            <label class="form-label fw-bold">Confirmar Contraseña</label>
                            <asp:TextBox runat="server" ID="txtConfirmarPass" CssClass="form-control" TextMode="Password" required></asp:TextBox>
                        </div>
                        
                        <div class="d-grid gap-2 mb-4">
                            <asp:Button Text="Registrarme" runat="server" CssClass="btn btn-primary fw-bold py-2" ID="btnRegistrar" OnClick="btnRegistrar_Click" />
                        </div>
                        
                        <div class="text-center">
                            <span class="text-muted">¿Ya tenés una cuenta?</span>
                            <a href="Login.aspx" class="text-decoration-none fw-bold ms-1">Iniciá sesión acá</a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>