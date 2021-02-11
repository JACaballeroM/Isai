<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalErrorObligatorios.ascx.cs" Inherits="UserControls_ModalErrorObligatorios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
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
<table cellpadding="0" cellspacing="0">
    <tr id="trCabecera" runat="server" class="TablaCabeceraCaja">
        <td class="TextLeftTop" colspan="2">
            <asp:Label ID="lblTextoTitulo" runat="server" SkinID="None" Style="float: left;">Error</asp:Label>
        </td>
    </tr>
    <tr>
        <td style="padding: 10px;">
            <asp:Image runat="server" ID="imgAlert" ImageUrl="~/Images/error.gif" Width="48px"
                Height="48px" AlternateText="Se necesita confirmación" />
        </td>
        <td align="center" style="width: 175px; padding: 10px;">
            <asp:Label ID="lblTextoInformacion" runat="server">Faltan campos obligatorios</asp:Label>
        </td>
    </tr>
    <tr>
        <td style="padding-left: 5px; padding-right: 5px; padding-bottom: 5px; text-align: center;"
            colspan="2">
            <table  style="width:100%;">
                <tr>
                    <td style="width:50%; text-align:center">
                        <asp:LinkButton ID="lnkCancelar" runat="server" OnClick="lnkCancelar_Click" CausesValidation="false"
                            >Ok</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
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
