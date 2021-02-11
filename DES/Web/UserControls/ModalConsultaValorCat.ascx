<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalConsultaValorCat.ascx.cs"
    Inherits="UserControls_ModaConsultaValorCatl" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc7" %>

<asp:Panel ID="pnlSolicitarCuentaPredialModal" Style="display: block; width: 375px"
    runat="server">
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

        function rellenar(quien, que, cuanto) {
            cadcero = '';
            for (i = 0; i < (cuanto - que.length); i++) {
                cadcero += '0';
            }
            quien.value = cadcero + que;
        }
    </script>
    <div class="DivPaddingBotton TextCenterMiddle">
        <table style="width: 100%" class="TablaCabeceraCaja">
            <tr>
                <td style="width: 100%">
                    <div style="float: left;">
                        <asp:Label ID="lblConsultaValorCatastralModalTitulo" runat="server" SkinID="None"
                            class="TextLeftTop" Text="Consulta valor catastral"></asp:Label>
                    </div>
                    <div style="float: right;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/x.gif" ImageAlign="NotSet"
                            OnClick="btnCerrar_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="DivPaddingBotton TextLeftMiddle">
        <asp:UpdatePanel ID="uppCuenta" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 100%" colspan="2">
                            <asp:ValidationSummary ID="vsFiltroSCP" runat="server" ValidationGroup="FiltroSCP" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblCuenta" SkinID="Titulo2" class="TextLeftTop" runat="server" Text="Cuenta catastral"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;<asp:TextBox ID="txtRegion" runat="server" Enabled="true" MaxLength="3" Width="30px"
                                SkinID="TextBoxObligatorio" onblur="rellenar(this,this.value,3);"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="rfvRegion" runat="server" ControlToValidate="txtRegion" SetFocusOnError="True"
                                    Enabled="true" ErrorMessage="Requerida una región" ValidationGroup="FiltroSCP"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            &nbsp;<asp:TextBox ID="txtManzana" runat="server" Enabled="true" MaxLength="3" Width="30px"
                                SkinID="TextBoxObligatorio" onblur="rellenar(this,this.value,3);">
                            </asp:TextBox><asp:RequiredFieldValidator ID="rfvManzana" runat="server" ControlToValidate="txtManzana"
                                SetFocusOnError="True" Enabled="true" ErrorMessage="Requerida una manzana" ValidationGroup="FiltroSCP"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                            &nbsp;<asp:TextBox ID="txtLote" runat="server" Enabled="true" MaxLength="2" Width="20px"
                                SkinID="TextBoxObligatorio" onblur="javascript:if(this.value.length!=0){rellenar(this,this.value,2);}"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="rfvLote" runat="server" ControlToValidate="txtLote" SetFocusOnError="True"
                                    Enabled="true" ErrorMessage="Requerido un lote" ValidationGroup="FiltroSCP" Display="Dynamic">*</asp:RequiredFieldValidator>
                            &nbsp;<asp:TextBox ID="txtUnidad" runat="server" Enabled="true" MaxLength="3" Width="30px"
                                SkinID="TextBoxObligatorio" onblur="javascript:if(this.value.length!=0){rellenar(this,this.value,3);}"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLote" SetFocusOnError="True"
                                    Enabled="true" ErrorMessage="Requerida la unidad privativa" ValidationGroup="FiltroSCP"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <hr />
        <asp:UpdatePanel ID="uppDatos" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
             <uc3:Progreso ID="Progreso1" runat="server" AssociatedUpdatePanelID="uppDatos"
                                    DisplayAfter="0" Mensaje="Espere ..."/>
                <table width="100%">
                    <tr>
                        <td style="width: 50%" colspan="2">
                            <asp:RadioButton ID="rbFecha" runat="server" Checked="true" Text="Fecha específica"
                                Enabled="true" AutoPostBack="true" OnCheckedChanged="rbl_OnSelectedIndexChanged"
                                GroupName="rbGroup" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%" align="right">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha:"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtFecha" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"
                                Enabled="true"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" Enabled="true"
                                TargetControlID="txtFecha" PopupButtonID="btnFecha">
                            </cc1:CalendarExtender>
                            <asp:ImageButton runat="server" ID="btnFecha" ImageUrl="~/images/calendario.png"
                                CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha"
                                Enabled="true" />
                            <asp:RequiredFieldValidator ID="rfvF1" runat="server" ControlToValidate="txtFecha"
                                SetFocusOnError="True" Enabled="true" ErrorMessage="Requerida fecha" ValidationGroup="FiltroSCP"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="dateValidator" runat="server" Type="Date" Operator="DataTypeCheck"
                                ControlToValidate="txtFecha" ErrorMessage="Fecha incorrecta" ValidationGroup="FiltroSCP"
                                SetFocusOnError="True" Display="Dynamic">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RadioButton ID="rbNormal" runat="server" Checked="false" Text="Tipo declaración normal/complementaria"
                                Enabled="true" AutoPostBack="true" OnCheckedChanged="rbl_OnSelectedIndexChanged"
                                GroupName="rbGroup" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" align="right">
                            <asp:Label ID="Label3" runat="server" Text="Fecha causación:"></asp:Label>
                            &nbsp;
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtFecha2" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"
                                Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="false" TargetControlID="txtFecha2"
                                PopupButtonID="btnFecha2">
                            </cc1:CalendarExtender>
                            <asp:ImageButton runat="server" ID="btnFecha2" ImageUrl="~/images/calendario.png"
                                CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha"
                                Enabled="false" />
                            <asp:RequiredFieldValidator ID="rfvF2" runat="server" ControlToValidate="txtFecha2"
                                SetFocusOnError="True" Enabled="false" ErrorMessage="Requerida fecha" ValidationGroup="FiltroSCP"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="dateValidator2" runat="server" Type="Date" Operator="DataTypeCheck"
                                ControlToValidate="txtFecha2" ErrorMessage="Fecha incorrecta" ValidationGroup="FiltroSCP"
                                Display="Dynamic" SetFocusOnError="True">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RadioButton ID="rbAnticipada" runat="server" Checked="false" Text="Tipo declaración anticipada"
                                Enabled="true" AutoPostBack="true" OnCheckedChanged="rbl_OnSelectedIndexChanged"
                                GroupName="rbGroup" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Fecha actual:"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblFechaActual" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelBotones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="float: right">
                                <uc3:Progreso ID="ProgresouppCuenta" runat="server" AssociatedUpdatePanelID="UpdatePanelBotones"
                                    DisplayAfter="0" />
                                <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Obtener Valor Catastral"
                                    Enabled="true" OnClick="btnAceptar_Click" ValidationGroup="FiltroSCP" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelar" runat="server" CssClass="boton" OnClick="btnCancelar_Click"
                                    CausesValidation="false" Text="Cerrar" />
                                <asp:HiddenField runat="server" ID="hiddenValorCat" />
                                <cc1:ModalPopupExtender ID="extenderPnlValorCat" runat="server" Enabled="True" TargetControlID="hiddenValorCat"
                                    PopupControlID="pnlValorCat" BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                <asp:Panel ID="pnlValorCat" SkinID="Modal" Style="width: 480px;display: none;" runat="server"> 
                                    <uc7:ModalInfo ID="modalValorCat" runat="server"  OnConfirmClick="ModalInfoValCat_Ok_Click"/>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<asp:UpdatePanel ID="uppErrorTareas" runat="server" UpdateMode="Conditional" RenderMode="Inline"> 
    <ContentTemplate> 
        <cc1:modalpopupextender ID="mpeErrorTareas" runat="server"  Enabled="True" 
        TargetControlID="hlnErrorTareas" PopupControlID="panErrorTareas" 
        Dropshadow="false" BackgroundCssClass="PanelModalBackground" />
        <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="Display:none" />
        <asp:Panel ID="panErrorTareas" runat="server" Style="Display:none" SkinID="Modal">
            <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" /> 
        </asp:Panel>
    </ContentTemplate> 
</asp:UpdatePanel>