<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="Declaracion.aspx.cs" Inherits="Declaracion" Title="ISAI - Declaración"
    EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc1" %>
<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc2" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmarcion" TagPrefix="uc4" %>
<%@ Register Src="UserControls/Reduccion.ascx" TagName="Reduccion" TagPrefix="uc5" %>
<%@ Register Src="UserControls/Exencion.ascx" TagName="Exencion" TagPrefix="uc6" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc7" %>
<%@ Register Src="UserControls/ModalInfoCorreo.ascx" TagName="ModalInfoCorreo" TagPrefix="uc8" %>
<%@ Register Src="UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<%@ Register Src="~/UserControls/Participantes.ascx" TagName="Participantes" TagPrefix="uc12" %>
<asp:Content ID="ContentDHead" ContentPlaceHolderID="ContentPlaceHolderDHead" runat="Server">
    <script src="<%= this.ResolveUrl("~/JS/documental.js") %>" type="text/javascript"></script>
    <script type="text/javascript">

        function cambiarFoco(controlId) {
            eval("document.getElementById('" + controlId + "').focus()");
        }

        //Funcionalidad para cambiar de color el impuesto declarado si es diferente al calculado

        //referencias a los textboxes
        var impuestoCalculado = '<%= lblImpuestoCalculadoDato.ClientID%>';
        var impuestoDeclarado = '<%= txtImpuestoNotarioDato.ClientID%>';

        //esta funcion se llama cada vez que se completa la pulsacion de una tecla
        function compararImpuestos(e) {
            var Code = e.keyCode;
            //0-9 y .
            if ((Code >= 48 && Code <= 57) || Code == 46) {
                //referencias los elementos html generados
                var calculatedTaxInput = document.getElementById(impuestoCalculado);
                var declaredTaxInput = document.getElementById(impuestoDeclarado);

                //conversion de string a float, previo reemplazo de caracteres no validos ('$', ',')
                var calculatedTaxString = calculatedTaxInput.innerText;
                var newCalculatedTaxString = calculatedTaxString.replace('$', '');
                calculatedTaxString = newCalculatedTaxString.replace(',', '');
                var calculatedTax = parseFloat(calculatedTaxString);

                //conversion de string a float, previo reemplazo de caracteres no validos ('$', ',')
                var declaredTaxString = declaredTaxInput.value;
                var newDeclaredTaxString = declaredTaxString.replace('$', '');
                declaredTaxString = newDeclaredTaxString.replace(',', '');
                var declaredTax = parseFloat(declaredTaxString);

                //si el valor es diferente el texto se pone en rojo, de lo contrario negro
                if (calculatedTax != declaredTax) {
                    declaredTaxInput.style.color = 'red';
                }
                else {
                    declaredTaxInput.style.color = 'black';
                }
                return true;
            }
            else {
                return false;
            }
        }

        function rellenar(quien, que, cuanto) {
            cadcero = '';
            for (i = 0; i < (cuanto - que.length) ; i++) {
                cadcero += '0';
            }
            quien.value = cadcero + que;
        }

        function ocultarPanelError(boton, panel) {
            if (panel.style.display == "block") {
                panel.style.display = "none";
                boton.value = "Mostrar Detalles";
            }
            else {
                panel.style.display = "block";
                boton.value = "Ocultar Detalles";
            }
        }

        function valorCat() {
            var valValor = document.getElementById("<%=cvValorCatastral.ClientID%>");
            ValidatorEnable(valValor, true);
        }


        function validar_datos() {
            this.document.forms[0].action = "https://www.egbs5.com.mx/egobierno/gdf/principal/indexgen.jsp";
            this.document.forms[0].method = "POST";
            this.document.forms[0].submit();
        }

        function Validar() {
            var unidadPrivativa = document.getElementById("<%=txtUnidadPrivativa.ClientID%>").value;
            var region = document.getElementById("<%=txtRegion.ClientID%>").value;
            var manzana = document.getElementById("<%=txtManzana.ClientID%>").value;
            var lote = document.getElementById("<%=txtLote.ClientID%>").value;
            var botonBorrar = document.getElementById("<%=btnBorrarCuenta.ClientID%>");
            if (unidadPrivativa.length > 0 && region.length > 0 && lote.length > 0 && manzana.length > 0) {
                var boton = document.getElementById("<%=btnValidarCuentaCatastral.ClientID%>");
                boton.click();
            }
            else {
                botonBorrar.disabled = true;
            }
        }

        function Fecha() {
            var now = new Date();
            return now.getDate();
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <span class="TextoNavegacionUrl"><b>Estas en:</b></span> <span class="TextoNavegacionUrlSub">
        <b>Declaracion ISAI</b></span> <span class="TextoNavegacionUrl"><b>&gt;</b></span>
    <span class="TextoNavegacionUrl"><b>Declaración</b></span>
</asp:Content>
<asp:Content ID="ContentContenido" ContentPlaceHolderID="ContentPlaceHolderDContenido"
    runat="Server">
    <asp:UpdatePanel ID="uppErrorTareas" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <cc1:ModalPopupExtender ID="mpeErrorTareas" runat="server" Enabled="True" TargetControlID="hlnErrorTareas"
                PopupControlID="panErrorTareas" DropShadow="false" BackgroundCssClass="PanelModalBackground" />
            <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="display: none" />
            <asp:Panel ID="panErrorTareas" runat="server" Style="display: none" SkinID="Modal">
                <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <cc1:ModalPopupExtender ID="modalErrorCuenta" runat="server" Enabled="True" PopupControlID="pnlErrorCuenta"
                BackgroundCssClass="PanelModalBackground" TargetControlID="HiddenFieldErrorCuenta"
                DropShadow="False" />
            <asp:HiddenField runat="server" ID="HiddenFieldErrorCuenta" />
            <asp:Panel ID="pnlErrorCuenta" SkinID="Modal" Style="width: 400px; display: none;"
                runat="server">
                <uc7:ModalInfo ID="modalInfoError" runat="server" OnConfirmClick="ModalInfoErrorCuenta_Ok_Click"
                    TextoInformacion="No existe la Cuenta Catastral" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppInfo" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoInfo" runat="server" AssociatedUpdatePanelID="uppInfo"
                DisplayAfter="0" />
            <cc1:ModalPopupExtender ID="extenderPnlInfoTokenModal" runat="server" Enabled="True"
                TargetControlID="HiddenTokenModal" PopupControlID="pnlTokenModal" BackgroundCssClass="PanelModalBackground"
                DropShadow="True" />
            <asp:HiddenField runat="server" ID="HiddenTokenModal" />
            <asp:Panel ID="pnlTokenModal" SkinID="Modal" Style="width: 280px; display: none;"
                runat="server">
                <uc7:ModalInfo ID="ModalInfoToken" runat="server" OnConfirmClick="ModalInfoToken_Ok_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppInfoPres" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoInfoPres" runat="server" AssociatedUpdatePanelID="uppInfoPres"
                DisplayAfter="0" />
            <cc1:ModalPopupExtender ID="extenderPnlInfoPres" runat="server" Enabled="True" TargetControlID="HiddenPres"
                PopupControlID="pnlInfoPres" BackgroundCssClass="PanelModalBackground" DropShadow="True" />
            <asp:HiddenField runat="server" ID="HiddenPres" />
            <asp:Panel ID="pnlInfoPres" SkinID="Modal" Style="width: 329px; display: none;" runat="server">
                <uc7:ModalInfo ID="ModalInfoPres" runat="server" TextoInformacion="La presente declaración se tiene por recibida en el entendido de que subsisten las facultades de comprobación con que cuenta la autoridad fiscal."
                    OnConfirmClick="ModalInfoPres_Ok_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppInfoFechaPago" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <uc3:Progreso ID="Progreso5" runat="server" AssociatedUpdatePanelID="uppInfoFechaPago"
                DisplayAfter="0" />
            <cc1:ModalPopupExtender ID="extenderPnlInfoFechaPago" runat="server" Enabled="True"
                TargetControlID="HiddenInfoFecha" PopupControlID="pnlInfoFecha" BackgroundCssClass="PanelModalBackground"
                DropShadow="True" />
            <asp:HiddenField runat="server" ID="HiddenInfoFecha" />
            <asp:Panel ID="pnlInfoFecha" SkinID="Modal" Style="width: 380px; display: none;"
                runat="server">
                <uc7:ModalInfo ID="ModalInfoFechaPago" runat="server" OnConfirmClick="ModalInfoFechaPago_Ok_Click" TextoInformacion="La fecha de pago debe estar comprendida entre la fecha de obtención de la línea de captura y la actual" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:UpdatePanel ID="UpdatePanelDeclaracion" runat="server" RenderMode="Inline" UpdateMode="Always">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnValidarCuentaCatastral" />

            <asp:AsyncPostBackTrigger ControlID="txtValorCatastral" />
        </Triggers>
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoPanelDeclaracion" runat="server" AssociatedUpdatePanelID="UpdatePanelDeclaracion"
                DisplayAfter="0" />
            <div class="DivPaddingBotton">
                <asp:ValidationSummary ID="ValidationSummaryGestionPersonas" runat="server" ValidationGroup="GestionPersonas"
                    HeaderText="Errores de validación:" />
                <asp:ValidationSummary ID="ValidationSummaryValidarCuentaCatastral" runat="server"
                    ValidationGroup="ValidarCuenta" HeaderText="Errores de validación:" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidacionDecision"
                    HeaderText="Por favor verifique lo siguiente:" />
                <asp:ValidationSummary ID="ValSumDecNormal" runat="server" ValidationGroup="DeclaracionNormal"
                    HeaderText="Por favor verifique lo siguiente:" />
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="ValorCatastral"
                    HeaderText="Por favor verifique lo siguiente:" />
                <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="txtFecha"
                    HeaderText="Por favor verifique lo siguiente:" />
                <asp:ValidationSummary ID="ValidationSummary6" runat="server" ValidationGroup="txtFecha2"
                    HeaderText="Por favor verifique lo siguiente:" />
                <asp:ValidationSummary ID="ValidationSummary7" runat="server" ValidationGroup="validacionDocBenFis"
                    HeaderText="Por favor verifique lo siguiente:" />
            </div>
            <div class="DivPaddingBotton" id="DivAvaluo" runat="server">
                <fieldset class="formulario">
                    <legend class="formulario">Detalle del avalúo</legend>
                    <asp:Panel ID="panelDetalleAvaluo" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblIdAvaluo" runat="server" SkinID="Titulo2" Text="Número Único"></asp:Label>
                                </td>
                                <td style="width: 30%">
                                    <asp:Label ID="lblIdAvaluoDato" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="TextLeftTop" style="width: 12%">
                                    <asp:Label ID="lblVigente" runat="server" SkinID="Titulo2" Text="Vigente"></asp:Label>
                                </td>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblVigenteDato" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblValorComercial" runat="server" SkinID="Titulo2" Text="Valor comercial"></asp:Label>
                                </td>
                                <td style="width: 30%">
                                    <asp:Label ID="lblValorComercialDato" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="width: 12%">
                                    <asp:Label ID="lblFecha" runat="server" SkinID="Titulo2" Text="Fecha"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFechaDato" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <div id="divValorRef" runat="server">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblValorRef" runat="server" SkinID="Titulo2" Text="Valor referido"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblValorRefDato" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="TextLeftTop">
                                        <asp:Label ID="lblFechaValRef" runat="server" SkinID="Titulo2" Text="Fecha valor referido"></asp:Label>
                                    </td>
                                    <td class="TextLeftTop">
                                        <asp:Label ID="lblFechaValRefDato" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </div>
                            <tr>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblCalle" runat="server" SkinID="Titulo2" Text="Calle"></asp:Label>
                                </td>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblCalleDato" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNoExt" runat="server" SkinID="Titulo2" Text="No. Exterior"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNoExtDato" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblNoInt" runat="server" SkinID="Titulo2" Text="No. Interior"></asp:Label>
                                </td>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblNoIntDato" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblColonia" runat="server" SkinID="Titulo2" Text="Colonia"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblColoniaDato" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblDelegacion" runat="server" SkinID="Titulo2" Text="Delegacion"></asp:Label>
                                </td>
                                <td class="TextLeftTop">
                                    <asp:Label ID="lblDelegacionDato" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCP" runat="server" SkinID="Titulo2" Text="C.P."></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCPDato" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </div>
            <div class="DivPaddingBotton">
                <asp:HiddenField runat="server" ID="HiddenModalCorreo" />
                <cc1:ModalPopupExtender ID="informar_ModalPopupExtender" runat="server" Enabled="True"
                    TargetControlID="HiddenModalCorreo" PopupControlID="pnlInfoCorreo" BackgroundCssClass="PanelModalBackground"
                    DropShadow="True" />
                <asp:Panel ID="pnlInfoCorreo" Style="display: none;" runat="server" SkinID="Modal"
                    Width="350">
                    <uc8:ModalInfoCorreo ID="ModalInfoCorreo" runat="server" OnConfirmClick="informar_ConfirmClick" />
                </asp:Panel>
                <asp:HiddenField runat="server" ID="HiddenValidarImporte" />
                <cc1:ModalPopupExtender ID="extenderPnlValidarImporte" runat="server" Enabled="True"
                    TargetControlID="HiddenValidarImporte" PopupControlID="pnlValidarImporte" BackgroundCssClass="PanelModalBackground"
                    DropShadow="True" />
                <asp:Panel ID="pnlValidarImporte" SkinID="Modal" Style="width: 400px; display: none;"
                    runat="server">
                    <uc7:ModalInfo ID="ModalInfoValidarImporte" runat="server" OnConfirmClick="ModalValidarImporte_Ok_Click" />
                </asp:Panel>
                <asp:HiddenField runat="server" ID="hiddenValBeneficios" />
                <cc1:ModalPopupExtender ID="extenderPnlValidarBeneficios" runat="server" Enabled="True"
                    TargetControlID="hiddenValBeneficios" PopupControlID="pnlValBeneficios" BackgroundCssClass="PanelModalBackground"
                    DropShadow="True" />
                <asp:Panel ID="pnlValBeneficios" SkinID="Modal" Style="width: 400px; display: none;"
                    runat="server">
                    <uc7:ModalInfo ID="modalBeneficios" runat="server" />
                </asp:Panel>
                <asp:HiddenField runat="server" ID="hidRolesParticipantes" />
                <asp:UpdatePanel ID="uppInformacionGestionBloqueos" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:Progreso ID="ProgresoInfoGestionBloqueos" runat="server" AssociatedUpdatePanelID="uppInformacionGestionBloqueos"
                            DisplayAfter="0" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderEliminar" runat="server" Enabled="True"
                            PopupControlID="pnlRolesParticipantes" TargetControlID="hlnRolesParticipantes"
                            BackgroundCssClass="PanelModalBackground " DropShadow="True">
                        </cc1:ModalPopupExtender>
                        <asp:HyperLink ID="hlnRolesParticipantes" runat="server" Style="display: none" />
                        <asp:Panel ID="pnlRolesParticipantes" runat="server" Style="width: 300px; display: none"
                            SkinID="Modal">
                            <uc4:ModalConfirmarcion ID="ModalConfirmarcionEliminar" runat="server" TextoConfirmacion="Esta acción conlleva seleccionar el nuevo rol de los participantes, ¿Desea cambiar la operación del acto jurídico?"
                                TextoTitulo="Advertencia" TextoLinkConfirmacion="Borrar Roles" VisibleTitulo="True"
                                OnConfirmClick="ProcesarRespuestaRolesParticipantes" />
                        </asp:Panel>
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderEliminarPar" runat="server" Enabled="True"
                            PopupControlID="pnlParticipantes2" TargetControlID="hlnParticipantes" BackgroundCssClass="PanelModalBackground "
                            DropShadow="True">
                        </cc1:ModalPopupExtender>
                        <asp:HyperLink ID="hlnParticipantes" runat="server" Style="display: none" />
                        <asp:Panel ID="pnlParticipantes2" runat="server" Style="width: 300px; display: none"
                            SkinID="Modal">
                            <uc4:ModalConfirmarcion ID="ModalConfirmarcionEliminarPar" runat="server" TextoConfirmacion="Esta acción conlleva eliminar los participantes, ¿Desea cambiar la operación del acto jurídico?"
                                TextoTitulo="Advertencia" TextoLinkConfirmacion="Borrar Participantes" VisibleTitulo="True"
                                OnConfirmClick="ProcesarRespuestaParticipantes" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--                Validadores Procedencia y Acto Juridico--%>
                <asp:RequiredFieldValidator ID="rfvActoJuridico" runat="server" ControlToValidate="ddlActoJuridicoDato"
                    Display="Dynamic" ErrorMessage="Debe seleccionar un Acto Jurídico." SetFocusOnError="True"
                    ValidationGroup="DeclaracionNormal" Font-Size="Smaller" ForeColor="White" Enabled="false"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPaisDato"
                    Display="Dynamic" ErrorMessage="Debe seleccionar una procedencia." SetFocusOnError="True"
                    ValidationGroup="DeclaracionNormal" Font-Size="Smaller" ForeColor="White" Enabled="false"></asp:RequiredFieldValidator>
                <asp:Panel ID="PanelDetalleDeclaracion" runat="server">
                    <table style="width: 100%" class="TextLeftMiddle">
                        <tr>
                            <td colspan="4">
                                <div class="DivPaddingBotton">
                                    <fieldset class="formulario">
                                        <legend class="formulario">Acto Jurídico</legend>
                                        <asp:Panel ID="pnlActoJuridico" runat="server">
                                            <table style="width: 100%">
                                                <tr id="trProcedencia" runat="server">
                                                    <td style="width: 20%">
                                                        <asp:Label ID="lblProcedencia" SkinID="Titulo2" runat="server" Text="Procedencia"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:Label ID="LblPaisDato" runat="server" Visible="False"></asp:Label>
                                                        <asp:DropDownList ID="ddlPaisDato" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%">
                                                        <asp:Label ID="lblActoJuridico" SkinID="Titulo2" runat="server" Text="Operación"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:Label ID="LblActoJuridicoDato" runat="server" Visible="False"></asp:Label>
                                                        <asp:DropDownList ID="ddlActoJuridicoDato" runat="server" Width="100%" OnSelectedIndexChanged="CambioActoJuridico"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trActoJuridico" runat="server" visible="false">
                                                    <td style="width: 100%" colspan="2">
                                                        <asp:Label ID="lblSinActosJuridicos" runat="server" Text="Sin acto jurídico."></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="DivPaddingBotton">
                                    <fieldset class="formulario">
                                        <legend class="formulario">Declaración</legend>
                                        <asp:UpdatePanel ID="updatePanelDeclaracionDoc" runat="server" RenderMode="Inline"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc3:Progreso ID="ProgresoDeclaracionDoc" runat="server" AssociatedUpdatePanelID="updatePanelDeclaracionDoc"
                                                    DisplayAfter="0" />
                                                <table class="TextLeftMiddle" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <asp:Label ID="lblDocJur" SkinID="Titulo2" class="TextLeftTop" runat="server" Text="Documento jurídico"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIdDoc" runat="server" CssClass="TextBoxNormal" MaxLength="100"
                                                                Style="display: none"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDoc" runat="server" ControlToValidate="txtIdDoc"
                                                                CssClass="TextoNormalError" Display="Dynamic" SetFocusOnError="True" ErrorMessage=""
                                                                ValidationGroup="validacionDocEsc" Enabled="True"></asp:RequiredFieldValidator>
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                <ContentTemplate>
                                                                    <uc3:Progreso ID="Progreso2" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
                                                                        DisplayAfter="0" />
                                                                    <cc1:ModalPopupExtender ID="extInfoDoc" runat="server" Enabled="True" TargetControlID="HiddenInfoDoc"
                                                                        PopupControlID="Panel3" BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                                                    <asp:HiddenField runat="server" ID="HiddenInfoDoc" />
                                                                    <asp:Panel ID="Panel3" SkinID="Modal" Style="width: 350px; display: none;" runat="server">
                                                                        <uc7:ModalInfo ID="ModalInfoDoc" runat="server" TextoInformacion="Se requiere adjuntar un documento jurídico a la declaración." />
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:UpdatePanel ID="uppADance" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btnDocumentos0" runat="server" AlternateText="Alta documento"
                                                                        Height="16px" ImageUrl="~/Images/plus.gif" Style="height: 16px" Width="16px"
                                                                        CausesValidation="False" Visible="true" ToolTip="Insertar documento" />&nbsp;
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:ImageButton ID="btnEditarDocumento" runat="server" AlternateText="Editar documento"
                                                                Height="16px" ImageUrl="~/Images/edit_p.gif" Style="height: 16px" Width="16px"
                                                                CausesValidation="False" Visible="true" ToolTip="Editar documento" Enabled="false" />&nbsp;
                                                            <asp:ImageButton ID="btnVerDoc" runat="server" ImageUrl="~/Images/search_p.gif" AlternateText="Visualizar documentación"
                                                                Enabled="false" CausesValidation="False" ToolTip="Visualizar documento" />&nbsp;
                                                            <asp:ImageButton ID="btnBorrarDocumento" runat="server" AlternateText="Borrar documentos"
                                                                Height="16px" ImageUrl="~/Images/trash_p.gif" Style="height: 16px" Width="16px"
                                                                CausesValidation="False" Visible="true" ToolTip="Eliminar documento" Enabled="false"
                                                                OnClick="btnBorrarDocumentos_Click" />
                                                            &nbsp;
                                                            <asp:HiddenField runat="server" ID="hidBorrarDoc" />
                                                            <cc1:ModalPopupExtender ID="ModalPopEliminarDoc" runat="server" Enabled="True" PopupControlID="pnlBorrarDoc"
                                                                TargetControlID="hidBorrarDoc" BackgroundCssClass="PanelModalBackground " DropShadow="True">
                                                            </cc1:ModalPopupExtender>
                                                            <asp:Panel ID="pnlBorrarDoc" runat="server" Style="width: 300px; display: none" SkinID="Modal">
                                                                <uc4:ModalConfirmarcion ID="ModalConfirmarcionBorrarDoc" runat="server" TextoTitulo="Advertencia"
                                                                    TextoLinkConfirmacion="Aceptar" VisibleTitulo="True" TextoConfirmacion="¿Desea eliminar el documento?"
                                                                    OnConfirmClick="ProcesarRespuestaBorrarDoc" />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblDescripcionDoc" runat="server" Text="  No hay documentos registrados"></asp:Label>
                                                            <asp:HiddenField ID="descrip" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFechaCausacion" runat="server" Text="Fecha causación" SkinID="Titulo2"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="updatePanelTxtFechaCausa" runat="server" RenderMode="Inline"
                                                                UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:HiddenField runat="server" ID="HiddenField1" />
                                                                    <uc3:Progreso ID="progreso1" runat="server" AssociatedUpdatePanelID="updatePanelTxtFechaCausa"
                                                                        DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando Datos..." />
                                                                    <asp:TextBox ID="txtFechaCausacion" runat="server" AutoPostBack="True" Width="85px"
                                                                        OnTextChanged="txtFechaCausacion_TextChanged"></asp:TextBox>
                                                                    <cc1:ModalPopupExtender ID="ModalPopupExtenderFecha" runat="server" Enabled="True"
                                                                        PopupControlID="PanelFEcha" TargetControlID="HiddenField1" BackgroundCssClass="PanelModalBackground "
                                                                        DropShadow="True">
                                                                    </cc1:ModalPopupExtender>
                                                                    <asp:Panel ID="PanelFEcha" runat="server" Style="width: 300px; display: none" SkinID="Modal">
                                                                        <uc4:ModalConfirmarcion ID="ModalActualizarFecha" runat="server" TextoTitulo="Advertencia"
                                                                            TextoLinkConfirmacion="Aceptar" VisibleTitulo="True" TextoConfirmacion="Esta acción conlleva actualizar el valor catastral, ¿Desea realizar la operación?"
                                                                            OnConfirmClick="ProcesarRespuestaActualizarFecha" />
                                                                    </asp:Panel>
                                                                    <asp:ImageButton runat="server" ID="btnFechaCausa" Visible="true" ImageUrl="~/images/calendario.png"
                                                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                                                    <asp:CompareValidator ID="cvFechaCausa" runat="server" ErrorMessage="Fecha errónea"
                                                                        ForeColor="Red" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaCausacion"
                                                                        Enabled="true" ValidationGroup="DeclaracionNormal">*</asp:CompareValidator>
                                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="La aplicación solamente permite hacer declaraciones de ISAI con fecha igual o posterior a 01/01/1972."
                                                                        ForeColor="Red" Type="Date" Operator="GreaterThanEqual" ValueToCompare="01/01/1972"
                                                                        ControlToValidate="txtFechaCausacion" Enabled="true" ValidationGroup="DeclaracionNormal">*</asp:CompareValidator>
                                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="La aplicación solamente permite hacer declaraciones de ISAI con fecha igual o posterior a 01/01/1972."
                                                                        ForeColor="Red" Type="Date" Operator="GreaterThanEqual" ValueToCompare="01/01/1972"
                                                                        ControlToValidate="txtFechaCausacion" Enabled="true" ValidationGroup="txtFecha">*</asp:CompareValidator>
                                                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="Fecha errónea"
                                                                        ForeColor="Red" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaCausacion"
                                                                        Enabled="true" ValidationGroup="txtFecha2">*</asp:CompareValidator>
                                                                    <cc1:CalendarExtender ID="txtFechacausa_CalendarExtender" runat="server" Enabled="True"
                                                                        PopupButtonID="btnFechaCausa" TargetControlID="txtFechaCausacion">
                                                                    </cc1:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:Label ID="lblFechaCausacionDato" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                        <td rowspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbReglasFallecimiento" Text="Reglas fallecimiento" runat="server"
                                                                            GroupName="Regla" Enabled="True" Checked="false" Visible="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rbReglasVigentes" runat="server" Checked="true" Enabled="True"
                                                                            GroupName="Regla" Text="Reglas vigentes" Visible="false" />
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <div id="divFechaPreventiva" runat="server" visible="false">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFechaPreve" runat="server" Text="Fecha preventiva" SkinID="Titulo2"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFechaPreveDato" runat="server" Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtFechaPrev" runat="server" Width="85px" SkinID="TextBoxObligatorio"></asp:TextBox>
                                                                    <asp:ImageButton runat="server" ID="btnFechaPrev" Visible="true" ImageUrl="~/images/calendario.png"
                                                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                                                    <asp:CompareValidator ID="cvFechaPrev" runat="server" ErrorMessage="Fecha errónea"
                                                                        ForeColor="Red" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaPrev"
                                                                        Enabled="true" ValidationGroup="txtFechaPrev">*</asp:CompareValidator>
                                                                    <cc1:CalendarExtender ID="txtFechaPrev_CalendarExtender" runat="server" Enabled="True"
                                                                        PopupButtonID="btnFechaPrev" TargetControlID="txtFechaPrev">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </div>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="updatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table class="TextLeftMiddle" style="width: 100%">
                                                    <tr>
                                                        <td colspan="4" valign="middle">
                                                            <hr style="background-color: #FFFFFF; color: #EBEBEB; height: -14px;" width="100%"
                                                                size="0.7em" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 26%">
                                                            <asp:Label ID="lblNumPresentacion" SkinID="Titulo2" runat="server" Text="Nº Presentación"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:Label ID="lblNumPresentacionDato" runat="server" Text="Declaración no presentada"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblNumPresentacionAnt" SkinID="Titulo2" runat="server" Text="Nº Presentación anterior"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:Label ID="lblNumPresentacionAntDato" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lblTipo" SkinID="Titulo2" runat="server" Text="Tipo"></asp:Label>
                                                        </td>
                                                        <td style="width: 35%">&nbsp;
                                                            <asp:Label ID="lblTipoDato" runat="server" Style="width: 20%"></asp:Label>
                                                        </td>
                                                        <td style="width: 8%">
                                                            <asp:Label ID="lblEstado" SkinID="Titulo2" runat="server" Text="Estado"></asp:Label>
                                                        </td>
                                                        <td style="width: 37%">&nbsp;
                                                            <asp:Label ID="lblEstadoDato" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblFechaPresentacion" runat="server" SkinID="Titulo2" Text="Fecha pres."></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:Label ID="lblFechaPresentacionDato" runat="server" Style="width: 20%" Text="Declaración no presentada"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblFechaEscritura" SkinID="Titulo2" runat="server" Text="Fecha escritura"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:Label ID="lblFechaEscrituraDato" runat="server" Style="width: 100%" Visible="false"
                                                                Text="Sin Especificar"></asp:Label>
                                                            <asp:TextBox ID="txtTipoDocumento" runat="server" Style="display: none;" AutoPostBack="true" />
                                                            <asp:TextBox ID="txtListaIdsFicheros" runat="server" Style="display: none;" AutoPostBack="true" />
                                                            <asp:TextBox ID="txtDocDigital" runat="server" Style="display: none;" OnTextChanged="txtDescripcion_TextChanged"
                                                                AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trValorDeuda" runat="server">
                                                        <td class="TextLeftTop">
                                                            <asp:Label ID="lblValorAdquisicion" runat="server" SkinID="Titulo2" Text="Valor adquisición"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                            <asp:Label ID="lblValorAdquisicionDolar" runat="server" Text="$"></asp:Label>
                                                            <asp:Label ID="lblValorAdquisicionDato" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtValorAdquisicionDato" runat="server" Style="width: 80%"></asp:TextBox><%--OnTextChanged="txtValorAdquisicionDato_TextChanged"--%>
                                                            <asp:CompareValidator ID="cvValorAdquisicionDato" runat="server" ControlToValidate="txtValorAdquisicionDato"
                                                                ErrorMessage="Valor incorrecto." Display="Dynamic" Operator="DataTypeCheck" SkinID="Red" Type="Currency"></asp:CompareValidator>
                                                        </td>
                                                        <td class="TextLeftTop">
                                                            <asp:Label ID="lblDeudas" runat="server" SkinID="Titulo2" Text="Deudas" Visible="False"></asp:Label>
                                                            <asp:CheckBox ID="chkbHabitacional" runat="server" Checked="true" Text="Habitacional" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDeudasDatos" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtDeudasDatos" Style="width: 80%" runat="server" AutoCompleteType="Disabled"
                                                                Visible="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr id="trLugar" runat="server" visible="false">
                                                        <td class="TextLeftTop">
                                                            <asp:Label ID="lblLugar" runat="server" SkinID="Titulo2" Text="Lugar"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblLugarDato" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtLugar" runat="server" Style="width: 100%" />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="DivPaddingBotton">
                                    <fieldset class="formulario">
                                        <legend class="formulario">Detalle del inmueble</legend>
                                        <asp:Panel ID="PanelDetalleInmueble" runat="server">
                                            <table style="width: 100%">
                                                <tr id="trRegion" runat="server">
                                                    <td style="width: 20%">
                                                        <asp:Label ID="lblRegion" runat="server" SkinID="Titulo2" Text="Región"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%">
                                                        <asp:Label ID="lblRegionDato" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%">
                                                        <asp:Label ID="lblManzana" runat="server" SkinID="Titulo2" Text="Manzana"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%">
                                                        <asp:Label ID="lblManzanaDato" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trLote" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblLote" runat="server" SkinID="Titulo2" Text="Lote"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLoteDato" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUnidadPrivativa" runat="server" SkinID="Titulo2" Text="Unidad Privativa"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblUnidadPrivativaDato" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCuenta" runat="server" SkinID="Titulo2" Text="Cuenta catastral"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="UpdatePanelCuentaJornada" runat="server" RenderMode="Inline"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <uc3:Progreso ID="ProgresoCuentaJornada" runat="server" AssociatedUpdatePanelID="UpdatePanelCuentaJornada"
                                                                    DisplayAfter="0" Mensaje="En proceso..." />
                                                                <asp:Label ID="lblCuentaDato" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtRegion" runat="server" MaxLength="3" Visible="false" Width="30px"
                                                                    SkinID="TextBoxObligatorio" Enabled="false" onChange="javascript:if(this.value!=''){rellenar(this,this.value,3);}Validar();"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="rfvRegion" runat="server" ControlToValidate="txtRegion" Enabled="false" ErrorMessage="Requerida una región"
                                                                        Display="Dynamic" ValidationGroup="ValidarCuenta" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtManzana" runat="server" MaxLength="3" Visible="false" Width="30px"
                                                                    SkinID="TextBoxObligatorio" Enabled="false" onChange="javascript:if(this.value!=''){rellenar(this,this.value,3);}Validar();"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="rfvManzana" runat="server" ControlToValidate="txtManzana" Enabled="false"
                                                                        SetFocusOnError="True" Display="Dynamic" ErrorMessage="Requerida una manzana"
                                                                        ValidationGroup="ValidarCuenta">*</asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtLote" runat="server" MaxLength="2" Visible="false" Width="20px"
                                                                    SkinID="TextBoxObligatorio" Enabled="false" onChange="javascript:if(this.value!=''){rellenar(this,this.value,2);}Validar();"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="rfvLote" runat="server" ControlToValidate="txtLote" SetFocusOnError="True"
                                                                        Enabled="false" ErrorMessage="Requerido un lote" ValidationGroup="ValidarCuenta"
                                                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtUnidadPrivativa" runat="server" Visible="false" MaxLength="3"
                                                                    Width="30px" SkinID="TextBoxObligatorio" Enabled="false" onChange="javascript:if(this.value!=''){rellenar(this,this.value,3);}Validar();"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="rfvUnidadPrivativa" runat="server" ControlToValidate="txtUnidadPrivativa"
                                                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido una Unidad" ValidationGroup="ValidarCuenta"
                                                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtCuentaCatastral" runat="server" Enabled="False" Visible="false"
                                                                    SkinID="TextBoxObligatorio"></asp:TextBox>
                                                                <asp:Button ID="btnBorrarCuenta" runat="server" Visible="False" Text="Borrar" Enabled="false"
                                                                    OnClick="btnBorrarCuenta_Click" />
                                                                <asp:Button ID="btnValidarCuentaCatastral" runat="server" Style="display: none;"
                                                                    OnClick="btnValidarCuentaCatastral_Click" ValidationGroup="ValidarCuenta" Text="Validar"
                                                                    Enabled="True" />


                                                                <cc1:ModalPopupExtender ID="extenderPnlInfoValidarCuentaCatastralModal" runat="server"
                                                                    Enabled="True" TargetControlID="HiddenValidarCuentaCatastralModal" PopupControlID="pnlInfoValidarCuentaCatastralModal"
                                                                    BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                                                <asp:Panel ID="pnlInfoValidarCuentaCatastralModal" SkinID="Modal" Style="width: 280px; display: none;"
                                                                    runat="server">
                                                                    <uc7:ModalInfo ID="ModalInfoValidarCuenta" runat="server" OnConfirmClick="ModalInfoValidarCuenta_Ok_Click" />
                                                                </asp:Panel>
                                                                <asp:HiddenField runat="server" ID="HiddenValidarCuentaCatastralModal" />
                                                                <cc1:ModalPopupExtender ID="extenderPnlInfoCuentaCatastralModal" runat="server" Enabled="True"
                                                                    TargetControlID="HiddenFieldCuentaCat" PopupControlID="PanelInfoCuentaCat" BackgroundCssClass="PanelModalBackground"
                                                                    DropShadow="True" />
                                                                <asp:HiddenField runat="server" ID="HiddenFieldCuentaCat" />
                                                                <asp:Panel ID="PanelInfoCuentaCat" SkinID="Modal" Style="width: 280px; display: none;"
                                                                    runat="server">
                                                                    <uc7:ModalInfo ID="ModalInfoCuentaCatastral" runat="server" OnConfirmClick="ModalInfoCuentaCatastral_Ok_Click" />
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblValorCatastral" runat="server" SkinID="Titulo2" Text="Valor catastral"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanelValorCatastral" runat="server" RenderMode="Inline"
                                                            UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblValorCatastralDato" runat="server" Visible="false"></asp:Label>
                                                                <asp:TextBox ID="txtValorCatastral" Width="88" runat="server" Enabled="false" Visible="false"
                                                                    OnTextChanged="txtValorCatastral_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                <asp:CompareValidator ID="cvValorCatastral" runat="server" Display="Dynamic" ErrorMessage="Valor catastral no es un número valido"
                                                                    Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"
                                                                    ToolTip="Valor Catastral no es un número valido" ControlToValidate="txtValorCatastral"
                                                                    SetFocusOnError="True">*</asp:CompareValidator>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtValorCatastral"
                                                                    Display="Dynamic" ErrorMessage="Debe especificar un Valor Catastral." SetFocusOnError="True"
                                                                    ValidationGroup="DeclaracionNormal" Font-Size="Smaller" ForeColor="White" Enabled="false">*</asp:RequiredFieldValidator>
                                                                <asp:HiddenField runat="server" ID="hidValorCatastral" />
                                                                <cc1:ModalPopupExtender ID="ModalPopupExtenderEliminarBF" runat="server" Enabled="True"
                                                                    PopupControlID="pnlValorCatastral" TargetControlID="hidValorCatastral" BackgroundCssClass="PanelModalBackground "
                                                                    DropShadow="True">
                                                                </cc1:ModalPopupExtender>
                                                                <asp:Panel ID="pnlValorCatastral" runat="server" Style="width: 300px; display: none"
                                                                    SkinID="Modal">
                                                                    <uc4:ModalConfirmarcion ID="ModalConfirmarcionEliminarBF" runat="server" TextoConfirmacion="Esta acción conlleva eliminar el beneficio fiscal indicado, ¿Desea realizar la operación?"
                                                                        TextoTitulo="Advertencia" TextoLinkConfirmacion="Aceptar" VisibleTitulo="True"
                                                                        OnConfirmClick="ProcesarRespuestaBorrarValorCatastral" />
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="pnlParticipantes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                    <ContentTemplate>
                                        <uc12:Participantes ID="ucParticipantes" runat="server" OnCargaControl="participantes"
                                            OnSujetoPasivoSeleccionado="SujetoPasivoChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div style="margin-bottom: 10px">
                                    <fieldset class="formulario">
                                        <legend class="formulario">Beneficios fiscales</legend>
                                        <div id="BotonesBeneficios" runat="server">
                                            <cc1:ModalPopupExtender ID="extenderPnlInfoBen" runat="server" Enabled="True" TargetControlID="HiddenInfoBen"
                                                PopupControlID="pnlInfoBen" BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                            <asp:HiddenField runat="server" ID="HiddenInfoBen" />
                                            <asp:Panel ID="pnlInfoBen" SkinID="Modal" Style="width: 320px; display: none;" runat="server">
                                                <uc7:ModalInfo ID="ModalInfoBen" runat="server" TextoInformacion="Debe introducir un valor catastral válido y seleccionar un acto jurídico." />
                                            </asp:Panel>
                                            <asp:ImageButton ID="imgBeneficiosOn" runat="server" AlternateText="Agregar un beneficio fiscal"
                                                Height="16px" ImageUrl="~/Images/plus.gif" Style="height: 16px" Width="18px"
                                                CausesValidation="False" Visible="true" ToolTip="Agregar un beneficio fiscal"
                                                OnClick="ConfigurarBeneficiosFiscales" />
                                            <asp:ImageButton ID="imgBeneficiosOff" runat="server" ImageUrl="~/Images/trash_p.gif"
                                                AlternateText="Visualizar documentación" Enabled="false" CausesValidation="False"
                                                ToolTip="Descartar beneficios fiscales" OnClick="ConfigurarBeneficiosFiscalesBorrar"
                                                Width="16px" />
                                            <asp:TextBox ID="txtValBeneficio" runat="server" Width="1" BorderStyle="None" BorderColor="White"
                                                ForeColor="White" Text="Continuar"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="valBeneficios" runat="server" ControlToValidate="txtValBeneficio"
                                                ErrorMessage="Seleccione un beneficio fiscal." Display="Dynamic" ValidationGroup="DeclaracionNormal"
                                                Enabled="False">*</asp:RequiredFieldValidator>
                                        </div>
                                        <div id="BeneficiosiscalesLeyend" runat="server">
                                            <asp:Label ID="Label1" runat="server" Text="No tiene beneficios fiscales"></asp:Label>
                                        </div>
                                        <div class="DivPaddingBotton" id="divBeneficiosFiscales" runat="server">
                                            <asp:Panel ID="pnlBeneficiosFiscales" runat="server">
                                                <table style="width: 100%">
                                                    <tr id="trExencion" runat="server">
                                                        <td id="tdExencion1" runat="server" style="width: 20%">
                                                            <asp:RadioButton ID="rbExencion" Text="Exención" runat="server" GroupName="BeneficioFiscal"
                                                                AutoPostBack="true" OnCheckedChanged="BeneficioFiscal_CheckedChanged" Enabled="True"
                                                                Checked="false" />
                                                            <asp:Label ID="lblExencion" runat="server" Text="Exención" SkinID="Titulo2" Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="tdExencion2" runat="server" style="width: 80%" colspan="4">
                                                            <uc6:Exencion ID="ucExencion" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trReduccion" runat="server">
                                                        <td id="tdReduccion1" runat="server" style="width: 20%; vertical-align: top;">
                                                            <asp:RadioButton ID="rbReduccion" Text="Reducción" runat="server" GroupName="BeneficioFiscal"
                                                                AutoPostBack="True" Enabled="true" Checked="false" OnCheckedChanged="BeneficioFiscal_CheckedChanged" />
                                                            <asp:Label ID="lblReduccion" runat="server" Text="Reducción" SkinID="Titulo2" Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="tdReduccion2" runat="server" style="width: 80%; vertical-align: top;" colspan="4">
                                                            <uc5:Reduccion ID="ucReduccion" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trTasaCero" runat="server">
                                                        <td style="width: 20%">
                                                            <asp:RadioButton ID="rbTasaCero" Text="Tasa 0%" runat="server" GroupName="BeneficioFiscal"
                                                                Enabled="true" AutoPostBack="True" OnCheckedChanged="BeneficioFiscal_CheckedChanged"
                                                                Checked="false" />
                                                        </td>
                                                        <td style="width: 25%"></td>
                                                        <td style="width: 5%"></td>
                                                        <td style="width: 50%" colspan="2"></td>
                                                    </tr>
                                                    <tr id="trSubsidio" runat="server">
                                                        <td style="width: 20%" id="tdSubsidio1" runat="server">
                                                            <asp:RadioButton ID="rbSubsidio" Text="Subsidio" runat="server" GroupName="BeneficioFiscal"
                                                                AutoPostBack="True" OnCheckedChanged="BeneficioFiscal_CheckedChanged" Checked="false"
                                                                Enabled="true" Visible="False" />
                                                            <asp:Label ID="lblSubsidio" runat="server" Text="Subsidio" SkinID="Titulo2" Visible="false"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%" id="tdSubsidio2" runat="server">
                                                            <asp:Label ID="LblPorcentajeSubsidio" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtPorcentajeSubsidio" runat="server" CssClass="TextBoxNormal" Style="width: 95%"
                                                                Enabled="False" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%" id="tdSubsidio3" runat="server">
                                                            <asp:Label ID="lblPorcentaje1" runat="server" Text="%" Visible="False"></asp:Label>
                                                            <asp:RangeValidator ID="RVSubsidio" runat="server" ControlToValidate="txtPorcentajeSubsidio"
                                                                CssClass="TextoNormalError" ErrorMessage="Porcentaje de Subsidio deber ser un número entre 1 y 100 y no tener más de dos decimales"
                                                                ForeColor="Blue" MaximumValue="100" SetFocusOnError="True" ToolTip="Porcentaje de Subsidio debe estar entre 0 y 100 y no tener más de dos decimales"
                                                                ValidationGroup="DeclaracionNormal" MinimumValue="0" Type="Currency" Display="Dynamic">!</asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trDisminucion" runat="server">
                                                        <td style="width: 20%" id="tdDisminucion1" runat="server">
                                                            <asp:RadioButton ID="rbDisminucion" Text="Disminución" runat="server" GroupName="BeneficioFiscal"
                                                                AutoPostBack="True" OnCheckedChanged="BeneficioFiscal_CheckedChanged" Checked="false" Visible="False" />
                                                            <asp:Label ID="lblDisminuscion" runat="server" Text="Disminución" SkinID="Titulo2"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%" id="tdDisminucion2" runat="server">
                                                            <asp:Label ID="LblPorcentajeDisminucíon" CssClass="TextoNormal" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtPorcentajeDisminucíon" runat="server" CssClass="TextBoxNormal"
                                                                Style="width: 95%" Enabled="False" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Label ID="lblPorcentaje2" class="TextoNormal" runat="server" Text="%" Visible="False"></asp:Label>
                                                            <asp:RangeValidator ID="RVDisminucion" runat="server" ControlToValidate="txtPorcentajeDisminucíon"
                                                                CssClass="TextoNormalError" ErrorMessage="Porcentaje de Disminución deber ser un número entre 0 y 100 y no tener más de dos decimales"
                                                                ForeColor="Blue" MaximumValue="100" SetFocusOnError="True" ToolTip="Valor del Porcentaje entre 1 y 100"
                                                                ValidationGroup="DeclaracionNormal" MinimumValue="0" Type="Currency" Display="Dynamic">!</asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCondonacion1" runat="server">
                                                        <td id="tdCondonacion11" runat="server" style="width: 20%">
                                                            <asp:RadioButton ID="rbCondonacion" Text="Condonación" runat="server" GroupName="BeneficioFiscal"
                                                                AutoPostBack="True" OnCheckedChanged="BeneficioFiscal_CheckedChanged" Checked="false" Visible="False" />
                                                            <asp:Label ID="lblcondonacion" runat="server" Text="Condonación" SkinID="Titulo2"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td id="tdCondonacion12" style="width: 25%" runat="server">
                                                            <asp:Label ID="LblPorcentajeCondonacion" CssClass="TextoNormal" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtPorcentajeCondonacion" runat="server" CssClass="TextBoxNormal"
                                                                Style="width: 95%" Enabled="False" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td id="tdCondonacion13" runat="server" style="width: 5%">
                                                            <asp:Label ID="lblPorcentaje3" class="TextoNormal" runat="server" Text="%" Visible="False"></asp:Label>
                                                            <asp:RangeValidator ID="RVCondonacion" runat="server" ControlToValidate="txtPorcentajeCondonacion"
                                                                CssClass="TextoNormalError" ErrorMessage="Porcentaje de Condonación deber ser un número entre 0 y 100 y no tener más de dos decimales"
                                                                ForeColor="Blue" MaximumValue="100" SetFocusOnError="True" ToolTip="Valor del Porcentaje entre 0 y 100"
                                                                ValidationGroup="DeclaracionNormal" MinimumValue="0" Type="Currency" Display="Dynamic">!</asp:RangeValidator>
                                                        </td>
                                                        <td style="width: 50%" colspan="2"></td>
                                                    </tr>
                                                    <tr id="trCondonacionJornada" runat="server">
                                                        <td style="width: 20%">
                                                            <asp:Label runat="server" SkinID="Titulo2" ID="lblCondonacionJornada" Text="Condonación" Visible="False" />
                                                        </td>
                                                        <td style="width: 80%" colspan="4">
                                                            <asp:UpdatePanel ID="UpdatePanelCondonacionJornada" runat="server" UpdateMode="Conditional"
                                                                RenderMode="Inline">
                                                                <ContentTemplate>
                                                                    <uc3:Progreso ID="ProgresoCondonacionJornada" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelCondonacionJornada" />
                                                                    <asp:Label ID="lblCondonacionJornadaDato" runat="server" Text="Sin condonación" Visible="False" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCondonacion2" runat="server">
                                                        <td style="width: 20%"></td>
                                                        <td style="width: 25%">
                                                            <asp:Label ID="LblFechaCondonacionDato" CssClass="TextoNormal" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtFechaCondonacion" runat="server" Enabled="False" Style="width: 95%" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 55%" colspan="3">
                                                            <asp:Label ID="LblFechaCondonacion" class="TextoNormal" runat="server" Text="Fecha" Visible="False"></asp:Label>
                                                            <asp:CompareValidator ID="cvFechaCondonacion" runat="server" Display="Dynamic" ErrorMessage="Fecha de Condonación no es válida"
                                                                Operator="DataTypeCheck" SkinID="Red" Type="Date" ValidationGroup="DeclaracionNormal"
                                                                ControlToValidate="txtFechaCondonacion">*</asp:CompareValidator>
                                                            <cc1:CalendarExtender ID="txtFechaCondonacion_CalendarExtender" runat="server" Enabled="True"
                                                                TargetControlID="txtFechaCondonacion">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCondonacion3" runat="server">
                                                        <td style="width: 20%"></td>
                                                        <td style="width: 65%" colspan="4">
                                                            <asp:Label ID="LblMotivoCondonacionDato" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtMotivoCondonacion" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                                                Style="width: 95%" MaxLength="200" TextMode="MultiLine" Visible="False"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 55%">
                                                            <asp:Label ID="LblMotivoCondonacion" runat="server" Text="Motivo" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSinBeneficio" runat="server" visible="false">
                                                        <td colspan="6" style="width: 100%">
                                                            <asp:Label ID="LblSinBeneficio" runat="server" Text="Sin Beneficios Fiscales"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trBeneficiosJustificacion2" runat="server" style="margin-top: 5px">
                                                        <td colspan="4" valign="middle">
                                                            <hr style="background-color: #FFFFFF; color: #EBEBEB; height: -14px;" width="100%"
                                                                size="0.7em" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trBeneficiosJustificacion" runat="server" style="margin-top: 5px">
                                                        <td colspan="6" style="width: 100%">
                                                            <asp:Label ID="Label2" runat="server" SkinID="Titulo2" Text="Documento que acredita el beneficio fiscal"></asp:Label>
                                                            <asp:ImageButton ID="btnDocBeneficios" runat="server" AlternateText="Seleccionar documento"
                                                                Height="16px" ImageUrl="~/Images/plus_p.gif" Style="height: 16px" Width="16px"
                                                                CausesValidation="False" ToolTip="Seleccionar documento" Enabled="false" />&nbsp;
                                                            <asp:ImageButton ID="btnEditarDocBeneficios" runat="server" AlternateText="Editar documento"
                                                                Height="16px" ImageUrl="~/Images/edit_p.gif" Style="height: 16px" Width="16px"
                                                                CausesValidation="False" Visible="true" ToolTip="Editar documento" Enabled="false" />&nbsp;
                                                            <asp:ImageButton ID="btnVerDocBeneficios" runat="server" ImageUrl="~/Images/search_p.gif"
                                                                AlternateText="Ver documento" Enabled="false" CausesValidation="False" ToolTip="Ver documento" />&nbsp;
                                                            <asp:ImageButton ID="btnEliminarDocBeneficios" runat="server" AlternateText="Borrar documentos"
                                                                Height="16px" ImageUrl="~/Images/trash_p.gif" Style="height: 16px" Width="16px"
                                                                CausesValidation="False" Visible="true" ToolTip="Eliminar documento" Enabled="false"
                                                                OnClick="btnBorrarDocBeneficios_Click" />
                                                            <asp:TextBox ID="txtTipoDocumentoBF" runat="server" Style="display: none;" AutoPostBack="true" />
                                                            <asp:TextBox ID="txtListaIdsFicherosBF" runat="server" Style="display: none;" AutoPostBack="true" />
                                                            <asp:TextBox ID="txtDocDigitalBF" runat="server" Style="display: none;" OnTextChanged="txtDescripcionBF_TextChanged"
                                                                AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trDesBeneficios" runat="server">
                                                        <td colspan="4">
                                                            <asp:Label ID="lblDescripcionDocBF" runat="server" Text="  No hay documentos registrados"></asp:Label>
                                                            <asp:HiddenField ID="descripBF" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="DivPaddingBotton">
                                    <cc1:ModalPopupExtender ID="extenderPnlInfoCalculoImpuestoModal" runat="server" Enabled="True"
                                        TargetControlID="hiddenPnlInfoCalculoImpuestoModal" PopupControlID="pnlInfoCalculoImpuestoModal"
                                        BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                    <asp:Panel ID="pnlInfoCalculoImpuestoModal" SkinID="Modal" Style="width: 280px; display: none;"
                                        runat="server">
                                        <uc7:ModalInfo ID="ModalInfoCalculo" runat="server" />
                                    </asp:Panel>
                                    <asp:HiddenField runat="server" ID="hiddenPnlInfoCalculoImpuestoModal" />
                                    <asp:UpdatePanel ID="updatePanelImpuesto" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnObtenerFutIsr" />
                                        <%--<asp:AsyncPostBackTrigger ControlID="btnObtenerLC" />--%>
                                        <%--<asp:PostBackTrigger ControlID="BImprirInformeISR" />--%>
                                        </Triggers>
                                        <ContentTemplate>
                                            <uc3:Progreso ID="ProgresoPanelImpuesto" Mensaje="Estamos trabajando..." runat="server"
                                                AssociatedUpdatePanelID="updatePanelImpuesto" DisplayAfter="0" DynamicLayout="false" />
                                            <fieldset class="formulario">
                                                <legend class="formulario">Impuesto</legend>
                                                <asp:ImageButton ID="btnCalcular" runat="server" ImageUrl="~/Images/statusbar.gif"
                                                    AlternateText="Calcular impuesto" Height="16px" OnClick="btnCalcular_Click" CausesValidation="true"
                                                    ValidationGroup="DeclaracionNormal" Width="16px" />

                                                <asp:Panel ID="pnlDetalleImpuesto" runat="server">

                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblTotalActoJur" runat="server" SkinID="Titulo2" Text="Total operación"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTotalActoJurDato" class="TextLeftMiddle" runat="server" Style="width: 100%"></asp:Label>
                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td>

                                                                <asp:Label ID="lblActualizacionImporte" runat="server" SkinID="Titulo2" Text="Actualización"></asp:Label>

                                                            </td>
                                                            <td>

                                                                <asp:Label ID="lblActualizacionImporteDato" runat="server"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBeneficioImporte" runat="server" SkinID="Titulo2" Text="Beneficio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblBeneficioImporteDato" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="tdlblRegargo" runat="server">
                                                                <asp:Label ID="lblPorcRegargoExTemp" SkinID="Titulo2" runat="server" Text="Extemporáneo"></asp:Label>
                                                            </td>
                                                            <td id="tdlblRegargoDato" runat="server">
                                                                <asp:Label ID="lblPorcRegargoExTempDato" runat="server" class="TextLeftMiddle" Style="width: 100%"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRecardoImporte" runat="server" SkinID="Titulo2" Text="Recargo"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRecargoImporteDato" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="lblImpuestoCalculado" SkinID="Titulo2" runat="server" Text="Imp. calculado"></asp:Label>
                                                            </td>
                                                            <td style="width: 30%">
                                                                <asp:Label ID="lblImpuestoCalculadoDato" class="TextLeftMiddle" runat="server" Style="width: 100%"></asp:Label>
                                                                <asp:Label ID="lblNotaReglaDatoDos" runat="server" Text="*" Visible="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 20%; margin-left: 40px;">
                                                                <asp:Label ID="lblCondonacionxFecha" runat="server" SkinID="Titulo2" Text="Condonacion" Visible="False"></asp:Label>
                                                            </td>
                                                            <td style="width: 30%">
                                                                <asp:Label ID="lblImpuestoNotarioDolar" runat="server" Text="$" Visible="false"></asp:Label>
                                                                <asp:Label ID="lblImpuestoNotarioDato" runat="server" Visible="False"></asp:Label>
                                                                <asp:Label ID="lblCondonacionxFechaDato" runat="server" Visible="False"></asp:Label>
                                                                <asp:Label ID="lblImpuestoPagadoAnterior" runat="server" Visible="False"></asp:Label>
                                                                <asp:TextBox ID="txtImpuestoNotarioDato" Style="width: 80%" runat="server" Visible="False"></asp:TextBox>
                                                                <asp:CompareValidator ID="cvImpuestoNotificado" runat="server" ControlToValidate="txtImpuestoNotarioDato"
                                                                    Display="Dynamic" ErrorMessage="Importe declarado no es un número valido" Operator="DataTypeCheck"
                                                                    SkinID="Red" Type="Double" ValidationGroup="DeclaracionNormal">*</asp:CompareValidator>
                                                                <asp:RangeValidator ID="RVImporteNotificado" runat="server" ControlToValidate="txtImpuestoNotarioDato"
                                                                    CssClass="TextoNormalError" Display="Dynamic" ErrorMessage="Importe declarado no puede ser negativo."
                                                                    ForeColor="Blue" MinimumValue="0" MaximumValue="999999999999999999999" SetFocusOnError="True"
                                                                    ToolTip="Importe declarado no puede ser negativo." Type="Double" ValidationGroup="DeclaracionNormal">!</asp:RangeValidator>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:CheckBox ID="CBCausaISR" runat="server" Text="Causa ISR" TextAlign="Left" AutoPostBack="True" CssClass="Titulo2" OnCheckedChanged="CBCausaISR_CheckedChanged" />
                                                            </td>
                                                            <td style="width: 30%">&nbsp;</td>
                                                            <td style="width: 20%; margin-left: 40px;">&nbsp;</td>
                                                            <td style="width: 30%">&nbsp;</td>
                                                        </tr>
                                                        <div id="DivISR" runat="server" class="DivPaddingBotton" visible="false">
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table>

                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <asp:Label ID="Label3" runat="server" SkinID="Titulo2" Text="Determinación del impuesto sobre la renta a favor del distrito federal."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lbla" runat="server" SkinID="Titulo3" Text="a. Monto de la operación."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txta" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="cvtxta" runat="server" ControlToValidate="txta" Display="None"  ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (a)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RVtxta" runat="server" ControlToValidate="txta" Display="None"  ErrorMessage="El importe debe de ser mayor a 0 (a)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblf" runat="server" SkinID="Titulo3" Text="f. Pago provisional conforme al artículo 126 de la LISR."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtf" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="cvtxtf" runat="server" ControlToValidate="txtf" Display="None"  ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (f)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="rvtxtf" runat="server" ControlToValidate="txtf" Display="None"  ErrorMessage="El importe debe de ser mayor a 0 (f)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblb" Width="70" runat="server" SkinID="Titulo3" Text="b. Deducciones autorizadas."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtb" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="cvtxtb" runat="server" ControlToValidate="txtb"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (b)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="rbtxtb" runat="server" ControlToValidate="txtb" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (b)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblg" runat="server" SkinID="Titulo3" Text="g. impuesto a pagar a la entidad federativa (e o f el menor)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtg" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="cvtxtg" runat="server" ControlToValidate="txtg"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (g)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="rvtxtg" runat="server" ControlToValidate="txtg" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (g)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblc" runat="server" SkinID="Titulo3" Text="c. Ganancias obtenidas."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtc" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="txtc"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (c)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtc" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (c)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblh" runat="server" SkinID="Titulo3" Text="h. Monto pagado (en la declaración que rectifica) con anterioridad."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txth" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="txth"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (h)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txth" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (h)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lbld" runat="server" SkinID="Titulo3" Text="d. Tasa 5%."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtd" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="txtd"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (d)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtd" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (d)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lbli" runat="server" SkinID="Titulo3" Text="i. Cantidad a cargo ((g-h) cuando g es mayor)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txti" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="txti"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (i)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txti" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (i)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lble" runat="server" SkinID="Titulo3" Text="e. Pago."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txte" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="txte"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (e)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txte" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (e)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblj" runat="server" SkinID="Titulo3" Text="j. Pago en exceso ((g-h) cuando h es mayor)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtj" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToValidate="txtj"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (j)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtj" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (j)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <asp:Label ID="Label4" runat="server" SkinID="Titulo2" Text="Pago del impuesto."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblA2"  runat="server" SkinID="Titulo3" Text="A. Impuesto sobre la renta(Anote el dato del campo g o i, según corresponda)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtA2" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="txtA2"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (A)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtA2" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (A)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblD2" runat="server" SkinID="Titulo3" Text="D. Multa por correción fiscal."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtD2" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToValidate="txtD2"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (D)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="txtD2" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (D)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblB2" runat="server" SkinID="Titulo3" Text="B. Parte actualizada del impuesto (se anotara la diferencia entre su impuesto y el mismo actualizado conforme lo que dispone la CFF)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtB2" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToValidate="txtB2"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (B)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtB2" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (B)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblE2" runat="server" SkinID="Titulo3" Text="E. Cantidad a pagar (A + B+ C+ D)."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtE2" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator13" runat="server" ControlToValidate="txtE2"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (E)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator10" runat="server" ControlToValidate="txtE2" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (E)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 45%">
                                                                                <asp:Label ID="lblC2" runat="server" SkinID="Titulo3" Text="C. Recargos."></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                                <asp:TextBox ID="txtC2" Width="70" runat="server" AutoCompleteType="Disabled" MaxLength="9"  ></asp:TextBox>
                                                                                <asp:CompareValidator ID="CompareValidator14" runat="server" ControlToValidate="txtC2"   Display="None" ErrorMessage="Solo se pueden ingresar numeros en los campos de isr (C)" Operator="DataTypeCheck" SkinID="Red" Type="Currency" ValidationGroup="DeclaracionNormal"></asp:CompareValidator>
                                                                                <asp:RangeValidator ID="RangeValidator11" runat="server" ControlToValidate="txtC2" Display="None" ErrorMessage="El importe debe de ser mayor a 0 (C)" Font-Size="8pt" MaximumValue="999999999" MinimumValue="1" ValidationGroup="DeclaracionNormal"></asp:RangeValidator>
                                                                            </td>
                                                                            <td style="width: 45%"></td>
                                                                            <td style="width: 5%"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan ="4">
                                                                                <asp:Button ID="btnObtenerFutIsr" runat="server" Text="Obtener informe ISR" Visible="false" OnClick="btnObtenerFutIsr_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </div>
                                                        <div id="DivLblRegla" class="DivPaddingBotton" runat="server" visible="false">
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Label ID="lblNotaReglaDato" runat="server" Style="width: 100%" class="TextLeftMiddle"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </div>
                                                    </table>
                                                </asp:Panel>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="DivPaddingBotton">
                                    <asp:UpdatePanel ID="updatePanel6" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="formulario">
                                                <legend class="formulario">Observaciones:</legend>
                                                <asp:TextBox Columns="101" Rows="3" runat="server" ID="txtAreaObservaciones" class="TextBoxNormal"
                                                    TextMode="MultiLine" />
                                                <asp:RegularExpressionValidator ID="revtxtAreaObservaciones" runat="server" ErrorMessage="Debe ingresar hasta un máximo de 500 carácteres"
                                                    ValidationExpression="^([\S\s]{0,500})$" ControlToValidate="txtAreaObservaciones"
                                                    Display="Dynamic" ValidationGroup="DeclaracionNormal"></asp:RegularExpressionValidator>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="divPago" runat="server" class="DivPaddingBotton">
                <asp:UpdatePanel ID="UpdatePanelDatosPresentacion" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:Progreso ID="ProgresoDatosPresentacion" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelDatosPresentacion" />
                        <fieldset class="formulario">
                            <legend class="formulario">Pago</legend>
                            <asp:HiddenField runat="server" ID="HiddenValidarFUTModal" />
                            <asp:HiddenField ID="HiddenPagoTelematico" runat="server" />
                            <asp:UpdatePanel ID="UpdatePanelLineaCaptura" runat="server" RenderMode="Inline"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc3:Progreso runat="server" ID="progresoUpdatePanelLineaCaptura" AssociatedUpdatePanelID="UpdatePanelLineaCaptura"
                                        DisplayAfter="0" DynamicLayout="false" Mensaje="Obteniendo línea de captura..." />
                                    <%--<asp:ImageButton ID="btnObtenerLinea" runat="server" ImageUrl="~/Images/Get_LC2.gif"
                                        AlternateText="Obtener linea de captura" Height="16px" OnClick="btnObtenerLinea_Click"
                                        Enabled="False" CausesValidation="False"/>--%>
                                    <asp:Button ID="btnObtenerLinea" runat="server" Text="Obtener Linea de Captura" OnClick="btnObtenerLinea_Click" Enabled="False" CausesValidation="False" />
                                    <%--                          <asp:Button ID="Button1"  runat="server" Text="Generar Comprobante de Pago" onclick="btnObtener_Click" CausesValidation="False"/>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:ImageButton ID="btnVerFUT" runat="server" ImageUrl="~/Images/zoom-in_p.gif"
                                AlternateText="Visualizar FUT" Height="16px" Enabled="False"
                                CausesValidation="False" Visible="False" />
                            <cc1:ModalPopupExtender ID="extenderPnlInfoFUTModal" runat="server" Enabled="True"
                                TargetControlID="HiddenValidarFUTModal" PopupControlID="pnlInfoValidarFUTModal"
                                BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                            <asp:Panel ID="pnlInfoValidarFUTModal" SkinID="Modal" Style="width: 280px; display: none;"
                                runat="server">
                                <uc7:ModalInfo ID="ModalInfoFUT" runat="server" OnConfirmClick="ModalInfoFUT_Ok_Click" />
                            </asp:Panel>
                            <asp:ImageButton ID="btnPagoTelematico" runat="server" ImageUrl="~/Images/cart_p.gif"
                                AlternateText="Presentar declaración" Height="16px" Enabled="False" CausesValidation="False"
                                OnClick="btnPagoTelematico_Click" ValidationGroup="pago" />
                            <cc1:ModalPopupExtender ID="extenderModalPopupPago" runat="server" DynamicServicePath=""
                                Enabled="True" TargetControlID="HiddenPagoTelematico" PopupControlID="pnlPagoTelematico"
                                CancelControlID="btnCancelar" BackgroundCssClass="PanelModalBackground" DropShadow="True">
                            </cc1:ModalPopupExtender>
                            <input id="id_s_transm" type="hidden" name="s_transm" runat="server" />
                            <input id="id_c_referencia" type="hidden" name="c_referencia" runat="server" />
                            <input id="id_val_1" type="hidden" name="val_1" runat="server" />
                            <input id="id_t_importe" type="hidden" name="t_importe" runat="server" />
                            <input id="id_val_2" type="hidden" name="val_2" runat="server" />
                            <input id="id_val_3" type="hidden" name="val_3" runat="server" />
                            <input id="id_val_4" type="hidden" name="val_4" runat="server" />
                            <input id="id_val_5" type="hidden" name="val_5" runat="server" />
                            <input id="id_val_6" type="hidden" name="val_6" runat="server" />
                            <input id="id_val_7" type="hidden" name="val_7" runat="server" />
                            <input id="id_val_8" type="hidden" name="val_8" runat="server" />
                            <input id="id_t_pago" type="hidden" name="t_pago" runat="server" />
                            <input id="id_servicio" type="hidden" name="t_servicio" runat="server" />
                            <input id="id_val_11" type="hidden" name="val_11" runat="server" />
                            <input id="id_val_12" type="hidden" name="val_12" runat="server" />
                            <input id="id_val_13" type="hidden" name="val_13" runat="server" />
                            <asp:Label ID="lblObtenerLinea" runat="server" Visible="False" Text="Siguiente paso: Obtener línea"
                                ForeColor="Blue"></asp:Label>
                            <asp:Label ID="lblInsertarLinea" runat="server" Visible="False" Text="Siguiente paso: Insertar línea"
                                ForeColor="Blue"></asp:Label>
                            <asp:Label ID="lblRealizarPago" runat="server" Visible="False" Text="Siguiente paso: Generar formato para pago"
                                ForeColor="Blue"></asp:Label>
                            <asp:Label ID="lblInsertarFecha" runat="server" Visible="False" Text="Siguiente paso: Insertar fecha causación"
                                ForeColor="Blue"></asp:Label>
                            <asp:Panel ID="pnlPagos" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblBanco" SkinID="Titulo2" runat="server" Text="Banco" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblBancoDato" runat="server" Style="width: 100%" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblSucursal" SkinID="Titulo2" runat="server" Text="Sucursal" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblSucursalDato" runat="server" Style="width: 100%" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblLinea" runat="server" SkinID="Titulo2" Text="Línea de captura"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblLineaDato" runat="server" Style="width: 100%"></asp:Label><asp:TextBox
                                                ID="txtLineaDato" Visible="false" runat="server" Style="width: 80%" SkinID="TextBoxObligatorio"></asp:TextBox><asp:RequiredFieldValidator
                                                    ID="rfvLineaCaptura" runat="server" ErrorMessage="Requerida una línea de captura"
                                                    Enabled="false" ValidationGroup="LineaCaptura" ControlToValidate="txtLineaDato"
                                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFechaLinea" runat="server" SkinID="Titulo2" Text="Fecha obtención"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblFechaLineaDato" runat="server"></asp:Label><asp:TextBox ID="txtFechaLineaCaptura"
                                                Visible="false" runat="server" SkinID="TextBoxObligatorio"></asp:TextBox><asp:RequiredFieldValidator
                                                    ID="rfvFechaLineaDato" runat="server" ErrorMessage="Requerida una fecha" ValidationGroup="LineaCaptura"
                                                    ControlToValidate="txtFechaLineaCaptura" Display="Dynamic" Enabled="false">*</asp:RequiredFieldValidator><asp:CompareValidator
                                                        ID="cvFechaLineaCaptura" runat="server" ErrorMessage="Fecha errónea" ForeColor="Blue"
                                                        Type="Date" ValidationGroup="LineaCaptura" Operator="DataTypeCheck" ControlToValidate="txtFechaLineaCaptura"
                                                        Display="Dynamic" Enabled="false">!</asp:CompareValidator><cc1:CalendarExtender ID="txtFechaLineaCaptura_CalendarExtender"
                                                            runat="server" Enabled="True" PopupButtonID="btnFechaLineaCaptura" TargetControlID="txtFechaLineaCaptura">
                                                        </cc1:CalendarExtender>
                                            <asp:ImageButton runat="server" ID="btnFechaLineaCaptura" Visible="false" ImageUrl="~/images/calendario.png"
                                                CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblEstadoPago" SkinID="Titulo2" runat="server" Text="Estado pago"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblEstadoPagoDato" runat="server" Style="width: 100%"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFechaPago" runat="server" SkinID="Titulo2" Text="Fecha causación"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:UpdatePanel ID="UpdatePanelFechaPago" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <uc3:Progreso ID="ProgresoFechaPago" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelFechaPago" />
                                                    <asp:Label ID="lblFechaPagoDato" runat="server" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtFechaPago" Visible="false" runat="server" SkinID="TextBoxNormal" BorderStyle="None"
                                                        OnTextChanged="txtFechaPago_TextChanged" AutoPostBack="true" Width="70px" ReadOnly="true"></asp:TextBox>
                                                    <asp:CompareValidator ID="cvFechaPago" runat="server"
                                                        ForeColor="Red" Type="Date" ValidationGroup="pago" Operator="DataTypeCheck" ControlToValidate="txtFechaPago"
                                                        Enabled="true"></asp:CompareValidator>
                                                    <asp:RangeValidator ID="rvFechaPago" runat="server"
                                                        ForeColor="Red" Type="Date" ValidationGroup="pago" ControlToValidate="txtFechaPago"
                                                        Enabled="true" MinimumValue="01/01/1900" MaximumValue="01/01/3000">*</asp:RangeValidator>
                                                    <asp:ImageButton runat="server" ID="btnFechaPago" Visible="false" ImageUrl="~/images/calendario.png"
                                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                                    <%--<cc1:CalendarExtender ID="txtFechaPago_CalendarExtender"
                                                        runat="server" Enabled="True" PopupButtonID="btnFechaPago" TargetControlID="txtFechaPago">
                                                    </cc1:CalendarExtender>--%>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>


                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 100%">
                                            <%-- <asp:Panel ID="PanelObtenerComprobante" runat="server" Visible = "false">--%>
                                            <%-- <asp:Button ID="btnObtener"  runat="server" Text="Generar Comprobante de Pago" onclick="btnObtener_Click"/>--%>
                                            <%-- </asp:Panel>--%>
                                            
                                        </td>

                                        <%-- <td colspan="2" style="width: 50%">
                                          </td> --%>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderCalc" runat="server" Enabled="True"
                PopupControlID="Panel2" TargetControlID="hidRolesParticipantes" BackgroundCssClass="PanelModalBackground "
                DropShadow="True">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel2" runat="server" Style="width: 300px; display: none" SkinID="Modal">
                <uc4:ModalConfirmarcion ID="ModalConfirmarcionCalc" runat="server" TextoTitulo="Advertencia"
                    VisibleTitulo="True" OnConfirmClick="ProcesarRespuestaCalc" />
            </asp:Panel>
            <div class="DivPaddingBotton" id="divDecision" runat="server">
                <asp:Panel ID="Ocultame" runat="server">
                    <fieldset class="formulario">
                        <legend class="formulario">Toma de Decisión</legend>
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:UpdatePanel ID="UpdatePanelDecision" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>


                                    <uc3:Progreso ID="ProgresoDecision" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelDecision" />
                                    <table style="width: 100%">
                                        <tr id="rowDecision" runat="server">
                                            <td style="width: 50%">
                                                <asp:RadioButtonList runat="server" ID="rbDecision" name="rbd" OnSelectedIndexChanged="decisionChanged"
                                                    AutoPostBack="true" Visible="false">
                                                    <asp:ListItem Value="0" Text="Aceptada"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Inconsistente"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Pendiente Documentación"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 50%" valign="middle">
                                                <asp:DropDownList ID="ddlMotivoRechazo" runat="server" Visible="false" OnSelectedIndexChanged="LogMotivo"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtMotivo" runat="server" Width="1" BorderStyle="None" BorderColor="White"
                                                    ForeColor="White"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDc" ControlToValidate="txtMotivo"
                                                    runat="server" ValidationGroup="ValidacionDecision" ErrorMessage="Debe tomar una decisión y si es el caso, un motivo de rechazo."
                                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%">
                                                <asp:Label ID="lblDecision" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </fieldset>
                </asp:Panel>

            </div>


            <div style="float: right">

                <!-- aqui omienza JAMG -->
                <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
             </asp:ScriptManager>--%>

                <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">--%>
                <%-- <ContentTemplate>
                                                 <asp:Button ID="btnObtener"  runat="server" Text="Generar Comprobante de Pago" onclick="btnObtener_Click"/>
                                              </ContentTemplate>--%>
                <%-- </asp:UpdatePanel>--%>
                <!-- Aqui termina JAMG -->

                <!-- Boton agregado pro JAMG -->
                <%--<asp:Button ID="btnObtener"  runat="server" Text="Generar Comprobante de Pago" Visible = "false" onclick="btnObtener_Click" CausesValidation="False"/>
                       
                --%>
                <asp:Button ID="bntAceptadaDeclaracion" runat="server" Visible="false" OnClick="bntAceptadaDeclaracion_Click"
                    CausesValidation="true" Text="Guardar decisión" ValidationGroup="ValidacionDecision" />
                &nbsp;<asp:Button ID="btnGuardar" runat="server" ValidationGroup="DeclaracionNormal"
                    OnClick="btnGuardar_Click" Text="Aceptar" />
                &nbsp;<cc1:ModalPopupExtender ID="btnGuardar_ModalPopupExtender" runat="server" Enabled="true"
                    TargetControlID="HiddenOperacion" PopupControlID="pnlGuardarModal" CancelControlID="lnkPnlGuardarModalCancel"
                    BackgroundCssClass="PanelModalBackground " DropShadow="False">
                </cc1:ModalPopupExtender>
                <asp:Button ID="lnkVolverDeclaraciones" Visible="true" runat="server" CausesValidation="False"
                    OnClick="lnkVolverDeclaraciones_Click" Text="Volver" />

            </div>


            <div>

                <asp:HiddenField ID="HiddenOperacion" runat="server" />
                <asp:HiddenField ID="HiddenTipoDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenIdDeclaracionPadre" runat="server" />
                <asp:HiddenField ID="HiddenGenerada" runat="server" />
                <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenEstadoDeclaracion" runat="server" Value="Borrador" />
                <asp:HiddenField ID="HiddenCodActoJur" runat="server" />
                <asp:HiddenField ID="HiddenValorCatastral" runat="server" Value="0" />
                <asp:HiddenField ID="HiddenCondonacionJornada" runat="server" />

                <asp:TextBox ID="txtIdDocumentoDigital" runat="server" Style="display: none;"></asp:TextBox>
                <asp:Panel ID="pnlGuardarModal" runat="server" Style="width: 400px; display: none;"
                    SkinID="Modal">
                    <table class="TablaCaja">
                        <tr class="TablaCabeceraCaja">
                            <td colspan="2" class="TextLeftMiddle ">Estado de la declaración
                                <uc3:Progreso ID="ProgresoGuardando" Mensaje="Validando declaración..." runat="server"
                                    AssociatedUpdatePanelID="UpdatePanelDeclaracion" DisplayAfter="0" DynamicLayout="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="TextLeftMiddle ">
                                <asp:RadioButton ID="rbModalGuardarBorrador" GroupName="rbGuardar" runat="server"
                                    Text="Borrador" Checked="True" text-align="Right" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="TextLeftMiddle ">
                                <asp:RadioButton ID="rbModalGuardarPendiente" GroupName="rbGuardar" runat="server"
                                    Text="Pdte. Presentar" text-align="Right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="TextCenterMiddle " style="width: 50%">
                                <asp:Button ID="lnkPnlGuardarModalOK" runat="server" OnClick="lnkPnlGuardarModalOK_Click"
                                    Text="Guardar" />
                                <asp:HiddenField ID="hiddenPnlGuardarModalOK_Extender" runat="server" />
                                <asp:TextBox ID="txtIdDocDig" runat="server" CssClass="TextBoxNormal" MaxLength="100"
                                    Style="display: none"></asp:TextBox><asp:RequiredFieldValidator ID="rfvDocDig" runat="server"
                                        ControlToValidate="txtIdDocDig" CssClass="TextoNormalError" Display="Dynamic"
                                        SetFocusOnError="True" ErrorMessage="Requerido un documento que acredite el beneficio fiscal."
                                        ValidationGroup="validacionDocBenFis" Enabled="false">*</asp:RequiredFieldValidator>
                                <cc1:ModalPopupExtender ID="extenderPnlGuardarModalOK_Extender" runat="server" Enabled="True"
                                    TargetControlID="hiddenPnlGuardarModalOK_Extender" PopupControlID="PanelInfoGuardarModal"
                                    BackgroundCssClass="PanelModalBackground" DropShadow="True">
                                </cc1:ModalPopupExtender>
                            </td>
                            <td class="TextCenterMiddle " style="width: 50%">
                                <asp:Button ID="lnkPnlGuardarModalCancel" runat="server" Text="Cancelar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>



                <asp:Panel ID="PanelInfoGuardarModal" Style="width: 280px; display: none;" runat="server"
                    SkinID="Modal">
                    <uc7:ModalInfo ID="ModalInfoGuardar" runat="server" OnConfirmClick="ModalInfoGuardar_Ok_Click" />
                </asp:Panel>
            </div>
        </ContentTemplate>


    </asp:UpdatePanel>

    <asp:UpdatePanel ID="pnlObtener" runat="server" RenderMode="Inline"
        UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnObtener" />

        </Triggers>
        <ContentTemplate>
            <asp:Button ID="btnObtener" runat="server" Text="Obtener formato de pago" OnClick="btnObtener_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:Panel ID="pnlPagoTelematico" runat="server" Style="width: 400px; display: none;"
        SkinID="Modal">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <uc3:Progreso runat="server" ID="progreso3" AssociatedUpdatePanelID="UpdatePanel3"
                    DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
                <table class="TablaCaja">
                    <tr class="TablaCabeceraCaja">
                        <td colspan="3">
                            <div style="float: left">
                                <asp:Label ID="lblTitloPanelModalPago" SkinID="None" runat="server" Text="Presentar declaración." />
                            </div>
                            <div style="float: right">
                                <asp:ImageButton runat="server" ID="imgCerrarPnlModalPago" SkinID="BotonBarraCerrar"
                                    ImageUrl="~/Images/x.gif" />
                            </div>
                        </td>
                    </tr>
                    <tr class="TablaCeldaCaja">
                        <td colspan="3">
                            <div>
                                <asp:RadioButton ID="RBTelematico" GroupName="Telematico" runat="server" Text="En línea"
                                    OnCheckedChanged="RBTelematico_OnCheckedChanged" AutoPostBack="true" Visible="false" /><%--</div><div style="float:left">--%>
                                <asp:DropDownList ID="ddlCatBanco" runat="server" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCatBanco_OnSelectedIndexChanged" Width="95%" Style="float: right">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr class="TablaCeldaCaja">
                        <td style="width: 100%">
                            <asp:RadioButton ID="rbCaja" AutoPostBack="true" GroupName="Telematico" runat="server"
                                Text="La declaración se remitirá a catastro." Checked="true" OnCheckedChanged="RBTelematico_OnCheckedChanged" />
                        </td>
                        <td style="width: 25%">
                            <asp:Label ID="lblModalPagoBanco" runat="server" Text="Banco" Visible="false"></asp:Label><asp:TextBox
                                ID="txtModalPagoBanco" runat="server" Width="92px" Visible="false"></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <asp:Label ID="lblModalPagoSucursal" runat="server" Text="Sucursal" Visible="false"></asp:Label><asp:TextBox
                                ID="txtModalPagoSucursal" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="TablaCeldaCaja">
                        <td style="width: 50%" class="TextCenterMiddle">
                            <asp:UpdatePanel ID="UpdatePanelPago" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <uc3:Progreso runat="server" ID="progreso4" AssociatedUpdatePanelID="UpdatePanelPago"
                                        DisplayAfter="0" DynamicLayout="false" Mensaje="Actualizando pago..." />
                                    <asp:Button ID="btnPagar" runat="server" OnClick="btnPagar_click" Text="Enviar" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2" style="width: 50%" class="TextCenterMiddle">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc2:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
