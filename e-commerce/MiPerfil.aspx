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
    
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-10">
                
                <h2 class="fw-bold mb-4">Configuración de Perfil</h2>

                <%-- cartel de éxito (Oculto por defecto desde C#). Se muestra después del Post/Redirect/Get,
                     leyendo el mensaje flash de Session["mensajePerfil"] --%>
                <div id="pnlExito" runat="server" visible="false" class="alert alert-success alert-dismissible fade show d-flex align-items-center shadow-sm mb-4" role="alert">
                    <%-- icono svg --%>
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-check-circle-fill me-3" viewBox="0 0 16 16">
                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                    </svg>
                    <div>
                        <strong>¡Excelente!</strong> <asp:Label ID="lblMensajeExito" runat="server" />
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
                               
                                    <input type="file" id="txtImagen" runat="server" ClientIDMode="Static" class="form-control form-control-sm" accept="image/*" onchange="previsualizar(this);" />
                                    <asp:Label ID="lblErrorImagen" runat="server" CssClass="invalid-feedback" />
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
                                        <%-- Bootstrap solo muestra el invalid-feedback si el TextBox de arriba tiene is-invalid --%>
                                        <asp:Label ID="lblErrorNombre" runat="server" CssClass="invalid-feedback" />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label fw-bold">Apellido <span class="text-danger">*</span></label>
                                        <asp:TextBox runat="server" ID="txtApellido" CssClass="form-control" ClientIDMode="Static"/>
                                        <asp:Label ID="lblErrorApellido" runat="server" CssClass="invalid-feedback" />
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
        // Mismas reglas que ValidarNombreOApellido y ValidarImagen en MiPerfil.aspx.cs.
        var REGEX_LETRAS = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;
        var EXTENSIONES_PERMITIDAS = [".jpg", ".jpeg", ".png"];
        var TAMANIO_MAXIMO_BYTES = 2 * 1024 * 1024;

        // Devuelve el mensaje de error del nombre o apellido, o null si está bien.
        function errorNombreOApellido(etiqueta, valor) {
            if (valor === "") return "El " + etiqueta + " es obligatorio.";
            if (!REGEX_LETRAS.test(valor)) return "El " + etiqueta + " solo puede tener letras y espacios.";
            if (valor.length > 50) return "El " + etiqueta + " no puede superar los 50 caracteres.";
            return null;
        }

        // Devuelve el mensaje de error de la imagen elegida, o null si no hay imagen o está bien.
        function errorImagen(input) {
            if (!input.files || input.files.length === 0) return null;   // la imagen es opcional

            var archivo = input.files[0];
            var nombre = archivo.name.toLowerCase();
            var extension = nombre.lastIndexOf(".") >= 0 ? nombre.substring(nombre.lastIndexOf(".")) : "";

            if (EXTENSIONES_PERMITIDAS.indexOf(extension) < 0) return "Formato de imagen no permitido. Solo se aceptan archivos .jpg, .jpeg o .png.";
            if (archivo.size > TAMANIO_MAXIMO_BYTES) return "La imagen es demasiado grande. El tamaño máximo permitido es 2 MB.";
            return null;
        }

        // Pinta el campo de rojo o verde y escribe el mensaje en su invalid-feedback (con textContent, nunca innerHTML).
        function mostrarResultado(input, mensaje) {
            var feedback = input.parentNode.querySelector(".invalid-feedback");

            if (mensaje) {
                feedback.textContent = mensaje;
                input.classList.add("is-invalid");
                input.classList.remove("is-valid");
                return false;
            }

            input.classList.remove("is-invalid");
            return true;
        }

        function evaluarNombre() {
            var input = document.getElementById("txtNombre");
            var ok = mostrarResultado(input, errorNombreOApellido("nombre", input.value.trim()));
            if (ok) input.classList.add("is-valid");
            return ok;
        }

        function evaluarApellido() {
            var input = document.getElementById("txtApellido");
            var ok = mostrarResultado(input, errorNombreOApellido("apellido", input.value.trim()));
            if (ok) input.classList.add("is-valid");
            return ok;
        }

        function evaluarImagen() {
            var input = document.getElementById("txtImagen");
            return mostrarResultado(input, errorImagen(input));
        }

        // Al elegir un archivo: lo valido y, si está bien, muestro la vista previa.
        function previsualizar(input) {
            if (!evaluarImagen()) {
                return;
            }

            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgNuevoPerfil.ClientID %>').src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        // Se ejecuta al presionar Guardar (OnClientClick). Si devuelve false, se cancela el postback.
        function validar() {
            // evalúo los tres (no corto en el primero) para que se marquen todos los errores juntos
            var nombreOk = evaluarNombre();
            var apellidoOk = evaluarApellido();
            var imagenOk = evaluarImagen();

            if (!nombreOk) {
                document.getElementById("txtNombre").focus();
            } else if (!apellidoOk) {
                document.getElementById("txtApellido").focus();
            }

            return nombreOk && apellidoOk && imagenOk;
        }

        // Mientras escribe, solo re-evalúo si el campo ya estaba en rojo (para no marcarlo antes de tiempo).
        document.addEventListener("input", function (e) {
            if (!e.target.classList.contains("is-invalid")) return;

            if (e.target.id === "txtNombre") evaluarNombre();
            if (e.target.id === "txtApellido") evaluarApellido();
        });
    </script>
</asp:Content>