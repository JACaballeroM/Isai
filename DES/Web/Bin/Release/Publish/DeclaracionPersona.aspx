<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="DeclaracionPersona.aspx.cs" Inherits="PersonaDeclaracion" Title="ISAI - Persona" %>

<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc1" %>
<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Persona.ascx" TagName="Persona" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControls/ModalDireccion.ascx" TagName="ModalDirecciones" TagPrefix="uc6" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc7" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/DireccionControl.ascx" TagPrefix="uc1" TagName="DireccionControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <span class="TextoNavegacionUrl"><b>Estas en:</b></span> <span class="TextoNavegacionUrlSub">
        <b>Declaracion ISAI</b></span> <span class="TextoNavegacionUrl"><b>></b></span>
    <span class="TextoNavegacionUrlSub"><b>Declaración</b></span> <span class="TextoNavegacionUrl">
        <b>></b></span> <span class="TextoNavegacionUrlSub"><b>Participantes</b></span>
    <span class="TextoNavegacionUrl"><b>></b></span> <span class="TextoNavegacionUrl"><b>
        Detalle Participante<br />
    </b></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderDContenido" runat="Server">

    <script src="<%= this.ResolveUrl("~/JS/direccion.js") %>" type="text/javascript"></script>

    <script src="<%= this.ResolveUrl("~/JS/contribuyentes.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
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
    </script>

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
    <asp:UpdatePanel ID="UpdatePanelPersona" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
            <asp:PostBackTrigger ControlID="btnImgVolverPersonas" />
            <asp:PostBackTrigger ControlID="btnVolverPersonas" />
        </Triggers>
        <ContentTemplate>
            <uc3:Progreso ID="progresoPersona" runat="server" AssociatedUpdatePanelID="UpdatePanelPersona"
                          DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
            <table>
                <tr>
                    <td colspan="3" align="left">
                        <asp:ValidationSummary ID="vsRegistroLicencias" runat="server" ValidationGroup="RegistroLicencias" />
                    </td>
                </tr>
            </table>
            <fieldset class="formulario">
                <legend class="formulario">Ficha de persona</legend>
               
                <!--Inicio BuscarPersonasModal-->
                <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Images/search.gif" ImageAlign="Middle"
                    AlternateText="Buscar datos de persona" CausesValidation="False" visible="false"/>
                <asp:TextBox ID="txtDatosPersona" runat="server" Style="display: none;" OnTextChanged="txtDatosPersona_TextChanged" Visible="false"
                    AutoPostBack="true" />
             
                <!--Fin BuscarPersonasModal-->
                <asp:Panel ID="pnlFichaPersona" runat="server">
                    <asp:HiddenField ID="HiddenOperacion" runat="server" />
                    <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                    <asp:HiddenField ID="HiddenIdPersona" runat="server" />
                    <asp:HiddenField ID="HiddenCodActoJur" runat="server" />
                    <asp:HiddenField ID="HiddenOperacionPadre" runat="server" />
                    <asp:HiddenField ID="HiddenIdDireccion" runat="server" />
                    <uc3:Persona ID="persona" runat="server" />
                    <asp:Label ID="lblParticipacion" runat="server" Text="Participación" CssClass="Titulo2"></asp:Label>
                    &nbsp;
                    <asp:Panel ID="pnlParticipacion" runat="server">
                        <table width="100%">
                            <tr>
                                <td width="20%">
                                    <asp:Label ID="lblParticipacionRol" runat="server" SkinID="Titulo2" Text="Rol"></asp:Label>
                                </td>
                                <td width="30%">
                                    <asp:Label ID="lblParticipacionDato" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddllblParticipacionDato" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="" ID="Req_ID"  CssClass="TextoNormalError" SetFocusOnError="True"
                                        ValidationGroup="ValidarDeclaracionPersona" runat="server" ControlToValidate="ddllblParticipacionDato"
                                        Text="*" ToolTip="Rol obligatorio" ErrorMessage="Rol campo obligatorio"></asp:RequiredFieldValidator>
                                </td>
                                <td width="20%">
                                    <asp:Label ID="lblPorcentaje" runat="server" SkinID="Titulo2" Text="Porcentaje"></asp:Label>
                                </td>
                                <td width="30%">
                                    <asp:Label ID="lblPorcentajeDato" runat="server"  Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtPorcentajeDato" runat="server" MaxLength="5"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPorcentajeDato"
                                        CssClass="TextoNormalError" ErrorMessage="Porcentaje campo obligatorio" SetFocusOnError="True"
                                        ToolTip="Porcentaje obligatorio" ValidationGroup="ValidarDeclaracionPersona">*</asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPorcentajeDato"
                                        CssClass="TextoNormalError" ErrorMessage="Porcentaje fuera de rango" ForeColor="Blue"
                                        MaximumValue="100" SetFocusOnError="True" ToolTip="Valor del Porcentaje entre 0 y 100"
                                        ValidationGroup="ValidarDeclaracionPersona" MinimumValue="0" Type="Double">!</asp:RangeValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </fieldset>
            <br />
            <uc1:DireccionControl runat="server" ID="DireccionControl" />
            <%--<asp:UpdatePanel ID="UpdatePanelDomicilio" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnActualizar" />
                    <asp:AsyncPostBackTrigger ControlID="txtEspDireccion" EventName="TextChanged" />
                </Triggers>
                <ContentTemplate>
                    <uc3:Progreso ID="progresoDomicilio" runat="server" AssociatedUpdatePanelID="UpdatePanelDomicilio"
                                  DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
                    <input type="hidden" runat="server" id="hidXmlDireccion" />
                    <div class="DivPaddingBotton TextLeftMiddle">
                        <cc1:CollapsiblePanelExtender ID="domicilio_CollapsiblePanelExtender" runat="server"
                            CollapseControlID="imgCollapseDomicilio" Collapsed="True" CollapsedImage="~/Images/label.gif"
                            ExpandControlID="imgCollapseDomicilio" ExpandedImage="~/Images/label_open.gif"
                            ImageControlID="imgCollapseDomicilio" SuppressPostBack="True" TargetControlID="PanelDomicilio"
                            CollapsedText="Expandir" ExpandedText="Colapsar">
                        </cc1:CollapsiblePanelExtender>
                        <fieldset class="formulario">
                            <legend class="formulario">
                                <asp:ImageButton ID="imgCollapseDomicilio" runat="server" CausesValidation="False"
                                    ImageUrl="~/Images/label.gif" />
                                &nbsp;Domicilio
                                <asp:TextBox ID="txtEspDireccion" runat="server" OnTextChanged="txtEspDireccion_ValueChanged"
                                    Style="display: none;" AutoPostBack="true" />
                                &nbsp;
                                <asp:ImageButton ID="btnActualizar" runat="server" ImageUrl="~/Images/edit.gif" AlternateText="Actualizar domicilio"
                                    Enabled="false" CausesValidation="False" />
                            </legend>
                            <asp:Panel ID="PanelDomicilio" runat="server">
                                <table class="TextLeftMiddle" style="width: 100%;">
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblDelegacion" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Delegación"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblDelegacionDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblColonia" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Colonia"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblColoniaDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblTipoVia" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Tipo de Vía"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblTipoViaDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblVia" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Vía"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblViaDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblNumeroExterior" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Nº Exterior"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblNumeroExteriorDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblCodigoPostal" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Cod. Postal"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblCodigoPostalDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblTipoAsentamiento" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Tipo Asentamiento"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblTipoAsentamientoDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblTipoLocalidad" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Tipo Localidad"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblTipoLocalidadDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEntreCalle1" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Entre Calle 1"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblEntreCalle1Dato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEntreCalle2" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Entre Calle 2"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblEntreCalle2Dato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblAndador" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Andador"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblAndadorDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEdificio" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Edificio"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEdificioDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblSeccion" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Sección"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblSeccionDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEntrada" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Entrada"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblEntradaDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblNumeroInterior" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Nº Interior"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblNumeroInteriorDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblTelefono" class="TextLeftTop" SkinID="Titulo2" runat="server" Text="Teléfono"></asp:Label>
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblTelefonoDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblIndicacionesAdicionales" class="TextLeftTop" SkinID="Titulo2" runat="server"
                                                Text="Ind. Adicionales"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblIndicacionesAdicionalesDato" class="TextLeftTop" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                     
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <asp:Panel ID="botones" runat="server">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnGuardar" runat="server" ValidationGroup="ValidarDeclaracionPersona"
                                OnClick="btnGuardar_Click" Text="Aceptar" />
                            <cc1:ModalPopupExtender ID="extenderPnlInfoGuardarModal" runat="server" Enabled="True"
                                TargetControlID="HiddenGuardarModal" PopupControlID="pnlInfoGuardarModal" BackgroundCssClass="PanelModalBackground"
                                DropShadow="True" />
                            <asp:Panel ID="pnlInfoGuardarModal" SkinID="Modal" Style="width: 280px; display: none;"
                                runat="server">
                                <uc7:ModalInfo ID="ModalInfoGuardar" runat="server" OnConfirmClick="ModalInfoGuardar_Ok_Click" />
                            </asp:Panel>
                            <asp:HiddenField runat="server" ID="HiddenGuardarModal" />
                            <asp:Button ID="btnImgVolverPersonas" Visible="true" runat="server" CausesValidation="False"
                                OnClick="btnImgVolverPersonas_Click" Text="Cancelar" />
                            <asp:Button ID="btnVolverPersonas" runat="server" Visible="false" OnClick="btnVolverPersonas_Click"
                                Text="Volver a declaración" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblErrores" width="100%">
        <tr>
            <td>
                <div id="capaResumenError" style="display: none">
                    <textarea id="txtResumenError" name="txtResumenError" class="Titulo2" readonly="readonly"
                        cols="87" rows="10" style="border-width: 0px; border-style: none; overflow: hidden;"></textarea>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc2:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
