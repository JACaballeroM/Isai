<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Exencion.ascx.cs" Inherits="UserControls_Exencion"
    EnableTheming="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ModalBuscarExenciones.ascx" TagName="ModalBuscarExencines"
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
<asp:UpdatePanel ID="updatePanelExencion" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <asp:Label ID="lblArticulo" runat="server" Text="Debe de seleccionar una exención con el buscador"></asp:Label>
                </td>
                <td style="width: 16px" rowspan="2" class="TextRigthBottom">
                    <asp:ImageButton ID="btnBuscarExenciones" runat="server" ImageUrl="~/Images/search.gif"
                        ImageAlign="Middle" AlternateText="Buscar datos de exenciones" OnClick="btnBuscarExenciones_Click" />
                    <cc1:ModalPopupExtender ID="extenderModalPopupBtnBuscarExenciones" runat="server"
                        BackgroundCssClass="PanelModalBackground" Enabled="True" TargetControlID="HiddenExenciones"
                        PopupControlID="pnlBuscarExenciones" DropShadow="True">
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
        <asp:Panel ID="pnlBuscarExenciones" Style="display: none; width: 500px" runat="server"
            SkinID="Modal">
            <uc5:ModalBuscarExencines ID="ModalBuscarExencinesid" runat="server" OnEventClick="modalBuscadorExenciones_EventClick"/>
        </asp:Panel>
        <asp:HiddenField ID="HiddenExenciones" runat="server" />
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

