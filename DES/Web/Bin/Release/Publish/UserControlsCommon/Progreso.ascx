<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Progreso.ascx.cs" Inherits="UserControlsCommon_Progreso" %>  
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
   <script language="javascript" type="text/javascript">
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

var prm = Sys.WebForms.PageRequestManager.getInstance();
function CancelAsyncPostBack() {
    if (prm.get_isInAsyncPostBack()) {
      prm.abortPostBack();
    }
}
// -->
</script>

<asp:UpdateProgress ID="uprCargando" runat="server">
    <ProgressTemplate>    
        <div id="progressBackgroundFilter"></div>                   
        <div id="processMessage">
            <asp:Label ID="lblMensaje" runat="server"  Text="Cargando datos..."></asp:Label><br/><br/>
            <asp:Image ID="imagenUpr" runat="server" ImageUrl="~/images/actualizando.gif" />
            <asp:Button  ID="btnCancelarUpdateProgress"  runat="server" Text="Cancelar" OnClientClick="javascript:CancelAsyncPostBack();" />                    
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
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