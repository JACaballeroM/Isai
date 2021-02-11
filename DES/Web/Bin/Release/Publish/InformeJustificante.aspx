<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeJustificante.aspx.cs" Inherits="InformeJustificante" Title="ISAI - Justificante de la Declaración" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Justificante de la Declaración</h3>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderContentBase" runat="Server">
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
        <cc1:modalpopupextender ID="mpeErrorTareas" runat="server"  Enabled="True" 
        TargetControlID="hlnErrorTareas" PopupControlID="panErrorTareas" 
        Dropshadow="false" BackgroundCssClass="PanelModalBackground" />
        <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="Display:none" />
        <asp:Panel ID="panErrorTareas" runat="server" Style="Display:none" SkinID="Modal">
            <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" /> 
        </asp:Panel>
    </ContentTemplate> 
</asp:UpdatePanel>
    <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
    <asp:HiddenField ID="HiddenIdAvaluo" runat="server" />
    <table>
        <tr>
        <td style="Height:565px" valign="top">
            <rsweb:ReportViewer ID="rpvJustificanteDeclaracion" runat="server" Font-Names="Verdana"
                Font-Size="8pt" Height="500px" Width="100%" ShowPrintButton="False">
                <LocalReport ReportPath="ReportDesign\InfJustificanteDeclaracion.rdlc" EnableHyperlinks="False">
                </LocalReport>
            </rsweb:ReportViewer>
      </td>
        </tr>
        <tr>
        <td>
        
        
            <div style="float: right">
                <asp:Button ID="lnkVolverDeclaraciones" Visible="true" runat="server" CausesValidation="False"
                    OnClick="lnkVolverDeclaraciones_Click" Text="Volver a Declaraciones" />
            </div>
            </td>
        </tr>
    </table>
</asp:Content>
