<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true" 
    CodeFile="InformeNotariosTotales.aspx.cs" Inherits="InformeNotariosTotales" Title="ISAI - Informe Notarios" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe de Notarios</h3>
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
        <asp:Label ID="lblTituloPanel" class="TextLeftTop" SkinID="Titulo2" runat="server"
            Text="Filtro de avalúos"></asp:Label>
        <table class="TextLeftMiddle">
            <tr>
                <td>
                    <asp:HiddenField ID="HiddenNotario" runat="server" />
                    <asp:Label ID="lblNumNotario" runat="server" Text="Nº Notario"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblNumNotarioDato" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFechas" runat="server" Text="Fechas"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFechaIniDato" runat="server" Text=""></asp:Label>
                    <span class="TextoNormal">- </span>
                    <asp:Label ID="lblFechaFinDato" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <table>
        <tr>
        <td style="Height:565px" valign="top">
             <rsweb:ReportViewer ID="rpvNotarios" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="530px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfNotariosTotales.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSetReports_EXCEP_LOG" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="DataSetReportsTableAdapters.EXCEP_LOGTableAdapter" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
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


