<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="e_commerce.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Estilo para hacer la foto de perfil perfectamente redonda */
        .foto-perfil {
            width: 200px;
            height: 200px;
            object-fit: cover;
            border-radius: 50%;
            border: 4px solid #f8f9fa;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container mt-4 mb-5">
        <div class="row justify-content-center">
            <div class="col-md-10">
                
                <h2 class="fw-bold mb-4">Configuración de Perfil</h2>

                <%-- cartel de éxito (Oculto por defecto desde C#) --%>
                <div id="pnlExito" runat="server" visible="false" class="alert alert-success alert-dismissible fade show d-flex align-items-center shadow-sm mb-4" role="alert">
                    <%-- icono svg --%>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-check-circle-fill me-3" viewBox="0 0 16 16">
                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                    </svg>
                    <div>
                        <strong>¡Excelente!</strong> Usuario actualizado satisfactoriamente.
                    </div>
                   
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>

                <div class="card shadow-sm border-0">
                    <div class="card-body p-4">
                        <div class="row">
                            
                            <%-- columna de foto de perfil --%>
                            <div class="col-md-4 d-flex flex-column align-items-center mb-4 mb-md-0 border-end">
                                <h5 class="fw-bold text-muted mb-3">Tu Avatar</h5>
                                
                                <%-- img de previsualización --%>
                                <asp:Image ID="imgNuevoPerfil" ImageUrl="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
                                    runat="server" CssClass="foto-perfil mb-4" />
                                
                                <div class="w-100 px-3">
                                    <label class="form-label text-muted small mb-1">Subir nueva foto (JPG/PNG)</label>
                               
                                    <input type="file" id="txtImagen" runat="server" class="form-control form-control-sm" accept="image/*" onchange="previsualizar(this);" />
                                </div>
                            </div>

                            <%-- columna de datos personales --%>
                            <div class="col-md-8 px-md-4">
                                <h5 class="fw-bold text-muted mb-3">Datos Personales</h5>
                                
                                <div class="mb-3">
                                    <label class="form-label fw-bold">Email (Usuario)</label>
                                 
                                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control bg-light" ReadOnly="true" />
                                </div>
                                
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label fw-bold">Nombre <span class="text-danger">*</span></label>
                                        <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label fw-bold">Apellido <span class="text-danger">*</span></label>
                                        <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" ClientIDMode="Static"/>
                                    </div>
                                </div>

                                <hr class="my-4" />
                                
                                <%-- Botonera --%>
                                <div class="d-flex justify-content-end">
                                    <a href="Default.aspx" class="btn btn-outline-secondary me-2">Cancelar</a>
                                    <asp:Button Text="💾 Guardar Cambios" CssClass="btn btn-primary px-4" ID="btnGuardar" OnClick="btnGuardar_Click" OnClientClick="return validar()" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <%--script de previsualización y actualización --%>
    <script>
    
        function previsualizar(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgNuevoPerfil.ClientID %>').src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        // validación de nombres
        function validar() {
            const txtNombre = document.getElementById("txtNombre");
            const txtApellido = document.getElementById("txtApellido");
            let formularioValido = true;

            
            const regexLetras = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;

           
            if (txtNombre.value.trim() === "" || !regexLetras.test(txtNombre.value.trim())) {
                txtNombre.classList.add("is-invalid");
                txtNombre.classList.remove("is-valid");
                formularioValido = false;
            } else {
                txtNombre.classList.remove("is-invalid");
                txtNombre.classList.add("is-valid");
            }

            
            if (txtApellido.value.trim() === "" || !regexLetras.test(txtApellido.value.trim())) {
                txtApellido.classList.add("is-invalid");
                txtApellido.classList.remove("is-valid");
                formularioValido = false;
            } else {
                txtApellido.classList.remove("is-invalid");
                txtApellido.classList.add("is-valid");
            }

            if (!formularioValido) {
                alert("Por favor, completá tu nombre y apellido correctamente (solo letras).");
            }

            return formularioValido;
        }

       
        document.addEventListener("DOMContentLoaded", function () {
            const txtNombre = document.getElementById("txtNombre");
            const txtApellido = document.getElementById("txtApellido");

            [txtNombre, txtApellido].forEach(input => {
                if (input) {
                    input.addEventListener("input", function () {
                        if (this.classList.contains("is-invalid")) {
                            const regexLetras = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;
                            if (this.value.trim() !== "" && regexLetras.test(this.value.trim())) {
                                this.classList.remove("is-invalid");
                                this.classList.add("is-valid");
                            }
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>