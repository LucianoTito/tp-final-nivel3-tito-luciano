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
        
        /* Los colores salen de las variables de la paleta (estilos.css): antes estaban escritos fijos
           (#0d6efd, #00d2ff, #6c757d) y no hubieran cambiado con la paleta. */

        /* Efecto de brillo (Glow) para las cajas de texto al hacer clic */
        .form-control:focus {
            box-shadow: 0 0 15px rgba(var(--bs-primary-rgb), 0.4);
            border-color: var(--bs-primary);
            transform: scale(1.01);
            transition: all 0.2s ease-in-out;
        }

        /* btn-magico se usa junto con btn-primary: el color lo pone la paleta, esta clase solo agrega el movimiento */
        .btn-magico {
            transition: all 0.3s ease;
        }
        .btn-magico:hover {
            transform: scale(1.03);
            box-shadow: 0 8px 20px rgba(var(--bs-primary-rgb), 0.5);
        }

        .btn-cancelar-hover {
            color: var(--bs-secondary-color);
            text-decoration: none;
            transition: all 0.3s ease;
        }
        .btn-cancelar-hover:hover {
            color: var(--bs-link-hover-color) !important;
            transform: scale(1.02);
            text-shadow: 0 0 8px rgba(var(--bs-primary-rgb), 0.3);
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
                        <asp:Button Text="Ingresar" runat="server" ID="btnIngresar" CssClass="btn btn-lg btn-primary btn-magico fw-bold" OnClick="btnIngresar_Click" />
                        
 
                        <a href="Default.aspx" class="btn btn-link mt-2 btn-cancelar-hover">⬅ Cancelar y volver al catálogo</a>
                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>