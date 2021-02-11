<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reduccion.ascx.cs" Inherits="UserControls_Reduccion"
    EnableTheming="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ModalBuscarReducciones.ascx" TagName="ModalBuscarReducciones"
    TagPrefix="uc5" %>
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
<asp:UpdatePanel ID="updatePanelReduccion" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <asp:Label ID="lblArticulo" runat="server" Text="Debe de seleccionar una reducción con el buscador"></asp:Label>
                </td>
                <td style="width: 16px" rowspan="2" class="TextRigthBottom">
                    <asp:ImageButton ID="btnBuscarReducciones" runat="server" ImageUrl="~/Images/search.gif"
                        ImageAlign="Middle" AlternateText="Buscar datos de reducciones" OnClick="btnBuscarReducciones_Click" />
                    <cc1:ModalPopupExtender ID="extenderModalPopupBtnBuscarReducciones" runat="server"
                        BackgroundCssClass="PanelModalBackground" Enabled="True" TargetControlID="HiddenReducciones"
                        PopupControlID="pnlBuscarReducciones" DropShadow="True">
                    </cc1:ModalPopupExtender>
                </td>
            </tr>
            <tr id="trDescripcion" runat="Server" visible="false">
                <td style="height: auto">
                    <span runat="server" id="lblDescripcion" class="TextoNormal">Descripcion</span>
                    <%--<asp:Label ID="lblDescripcion" runat="server" Text="Descripcion" Enabled="false"></asp:Label>--%>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlBuscarReducciones" Style="display: none; width: 500px" runat="server"
            SkinID="Modal">
            <uc5:ModalBuscarReducciones ID="modalBuscarReduccionesid" runat="server" OnEventClick="modalBuscadorReducciones_EventClick" />
        </asp:Panel>
        <asp:HiddenField ID="HiddenReducciones" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
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
