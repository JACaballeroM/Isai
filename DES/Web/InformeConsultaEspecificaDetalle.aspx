<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeConsultaEspecificaDetalle.aspx.cs" Inherits="InformeConsultaEspecificaDetalle"
    Title="ISAI - Informe Consulta Específica Detalle" %>

<%@ Register Src="UserControls/Persona.ascx" TagName="Persona" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="progreso" TagPrefix="uc3" %>

<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe de consulta específica</h3>
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
            <asp:Panel ID="panel1" runat="server">
                <asp:Label ID="LblFiltroAvaluos" SkinID="Titulo2" runat="server" Text="Detalle consulta específica"></asp:Label>
                <br />
            </asp:Panel>
            <table>
        <tr>
        <td style="Height:565px" valign="top">
    <rsweb:ReportViewer ID="rpvConsultaEspecifica" runat="server" Font-Names="Verdana"
        Font-Size="8pt" Height="500px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfConsultaEspecificaDetalle.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    </td>
        </tr>
        <tr>
        <td>
            <div style="float: right">
                <asp:Button ID="btnVolver" Visible="true" runat="server" CausesValidation="False"
                    OnClick="btnVolver_Click" Text="Volver" />
            </div>
            </td>
        </tr>
    </table>
</asp:Content>
