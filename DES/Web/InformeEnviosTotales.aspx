<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeEnviosTotales.aspx.cs" Inherits="InformeEnviosTotales" Title="ISAI - Informe Envios Totales" %>

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
        ISAI - Informe de envíos totales</h3>
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
        <asp:Label ID="LblFiltroAvaluos"  SkinID="Titulo2" runat="server" Text="Filtro de avalúos"></asp:Label>
        <table>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="vsFiltro" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFechas" runat="server" Text="Año"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAño" runat="server" EnableTheming="True" SkinID="TextBoxObligatorio"
                        Width="60px" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAño" runat="server" ControlToValidate="txtAño"
                        Display="Dynamic" ErrorMessage="Año obligatorio" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvAño" runat="server" ControlToValidate="txtAño" 
                        Display="Dynamic" ErrorMessage="Año erróneo" ForeColor="Blue" 
                        Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True">!</asp:CompareValidator>
                    &nbsp;<asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                        OnClick="btnBuscar_Click" Width="16px" CausesValidation="true" />
                </td>
               
            </tr>
        </table>
        <br />
    </asp:Panel>
    <rsweb:ReportViewer ID="rpvEnviosTotales" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="500px" Width="100%" SizeToReportContent="True" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfEnviosTotal.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
