<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalInfoCorreo.ascx.cs" Inherits="UserControls_ModalInfoCorreo" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc1" %>
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
<table class="TablaCaja" Style="width: 350px;">
    <tr id="trCabecera" runat="server" class="TablaCabeceraCaja">
        <td >
            <div style="float: left">
                <asp:Label ID="lblTextoTitulo" runat="server" SkinID="None">Información</asp:Label>               
            </div>
        </td>
    </tr>
    <tr class="TablaCeldaCaja">
        <td>
            <asp:Label ID="lblTextoInformacion" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <table class="TablaCaja">
                <tr>                
                    <td style="text-align: center">
                     <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
        <uc1:Progreso ID="ProgresoConfirmando" Mensaje="En progreso..." runat="server" AssociatedUpdatePanelID="UpdatePanelBusqueda" DisplayAfter="0" DynamicLayout="false"  />
                        <asp:Button ID="btnAceptar" runat="server" 
                OnClick="btnAceptar_Click" CausesValidation="false"
                            ToolTip="Pulsando aceptará la acción" Text="Aceptar">
                        </asp:Button>
                        </ContentTemplate>
                        </asp:UpdatePanel>
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
