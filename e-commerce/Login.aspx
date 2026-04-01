<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="e_commerce.Login" %>

<%-- CSS --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Efecto flotante para la tarjeta entera */
        .login-card {
            border: none;
            border-radius: 15px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }
        .login-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 1rem 3rem rgba(0,0,0,.175) !important;
        }
        
        /* Efecto de brillo (Glow) para las cajas de texto al hacer clic */
        .form-control:focus {
            box-shadow: 0 0 15px rgba(13, 110, 253, 0.4);
            border-color: #0d6efd;
            transform: scale(1.01);
            transition: all 0.2s ease-in-out;
        }

        .btn-magico {
            background: linear-gradient(45deg, #0d6efd, #00d2ff);
            border: none;
            transition: all 0.3s ease;
            background-size: 200% auto;
        }
        .btn-magico:hover {
            background-position: right center;
            transform: scale(1.03);
            box-shadow: 0 8px 20px rgba(13, 110, 253, 0.5);
        }

        .btn-cancelar-hover {
            color: #6c757d; 
            text-decoration: none;
            transition: all 0.3s ease; 
        }
        .btn-cancelar-hover:hover {
            color: #0d6efd !important; 
            transform: scale(1.02); 
            text-shadow: 0 0 8px rgba(13, 110, 253, 0.3); 
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 
    <div class="row justify-content-center align-items-center" style="min-height: 70vh;">
        <div class="col-md-6 col-lg-4">
            
            <div class="card shadow-lg login-card p-4 mb-4">
                <div class="card-body">
                    
                    <div class="text-center mb-4">
                        <h2 class="fw-bold">🔐 Iniciar Sesión</h2>
                        <p class="text-muted">Ingresá tus credenciales para administrar el catálogo</p>
                    </div>

                    <div class="mb-4">
                        <label class="form-label fw-bold">Email de Usuario</label>
                        <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control form-control-lg bg-light" placeholder="admin@admin.com" />
                    </div>

                    <div class="mb-4">
                        <label class="form-label fw-bold">Contraseña</label>
                        <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control form-control-lg bg-light" TextMode="Password" placeholder="******" />
                    </div>

                    <%-- Etiqueta oculta para tirar mensajes de error --%>
                    <asp:Label runat="server" ID="lblError" CssClass="text-danger mb-3 d-block text-center fw-bold" Visible="false" />

                    <div class="d-grid gap-2 mt-4">
                        <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" CssClass="btn btn-lg btn-magico text-white fw-bold" OnClick="btnIngresar_Click" />
                        
 
                        <a href="Default.aspx" class="btn btn-link mt-2 btn-cancelar-hover">⬅ Cancelar y volver al catálogo</a>
                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>