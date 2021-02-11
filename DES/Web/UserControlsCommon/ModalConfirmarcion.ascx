<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalConfirmarcion.ascx.cs"
    Inherits="UserControlsCommon_ModalConfirmacion" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
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
<table class="TablaCaja">
    <tr id="trCabecera" runat="server" class="TablaCabeceraCaja">
        <td colspan="3">
            <div style="float: left">
                <asp:Label ID="lblTextoTitulo" runat="server" SkinID="None">Confirmación</asp:Label>
            </div>
            <div runat="server" style="float: right">
                <asp:ImageButton ID="imgCancelar" runat="server" ImageUrl="~/Images/x.gif" OnClick="btnCancelar_Click" />
            </div>
        </td>
    </tr>
    <tr class="TablaCeldaCaja">
        <td style="width: 48px">
            <asp:Image runat="server" ID="imgAlert" ImageUrl="~/Images/alert.jpg" AlternateText="Se necesita confirmación" />
        </td>
        <td>
            <asp:Label ID="lblTextoConfirmacion" runat="server">¿Desea confirmar la acción?</asp:Label>
        </td>
    </tr>
    <tr id="trImpDeclarado" runat="server" visible="false">
        <td>
        </td>
        <td>
            <asp:TextBox ID="txtImpDeclarado" runat="server"></asp:TextBox>
            <asp:CompareValidator ID="cvImpuestoNotificado" runat="server" ControlToValidate="txtImpDeclarado"
                Display="Dynamic" ErrorMessage="Importe declarado no es un número valido" Operator="DataTypeCheck"
                SkinID="Red" Type="Double" ValidationGroup="ValidarImp">*</asp:CompareValidator>
            <asp:RangeValidator ID="RVImporteNotificado" runat="server" ControlToValidate="txtImpDeclarado"
                CssClass="TextoNormalError" Display="Dynamic" ErrorMessage="Importe declarado no puede ser negativo."
                ForeColor="Blue" MinimumValue="0" MaximumValue="999999999999999999999" SetFocusOnError="True"
                ToolTip="Importe declarado no puede ser negativo." Type="Double" ValidationGroup="ValidarImp">!</asp:RangeValidator>
            <asp:ValidationSummary ID="ValSumDecNormal" runat="server" ValidationGroup="ValidarImp" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table class="TablaCaja">
                <tr>
                    <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <td style="width: 50%; text-align: center">
                                <uc1:Progreso ID="ProgresoConfirmando" Mensaje="En progreso..." runat="server" AssociatedUpdatePanelID="UpdatePanelBusqueda"
                                    DisplayAfter="0" DynamicLayout="false" />
                                <asp:Button ID="btnConfirmar" runat="server" OnClick="btnConfirmar_Click" CausesValidation="false"
                                    ToolTip="Pulsando aceptará la acción" Text="Aceptar" ValidationGroup="ValidarImp">
                                </asp:Button>
                            </td>
                            <td style="width: 50%; text-align: center">
                                <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" CausesValidation="false"
                                    ToolTip="Pulsando cancelará la acción" Text="Cancelar"></asp:Button>
                            </td>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </tr>
            </table>
        </td>
    </tr>
</table>
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
