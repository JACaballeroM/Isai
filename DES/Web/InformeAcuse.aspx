<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeAcuse.aspx.cs" Inherits="InformeAcuse" Title="ISAI - Acuse de la Declaración" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
     
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 565px;
            width: 762px;
        }
        .style2
        {
            width: 762px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Acuse de la Declaración</h3>
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
        <td valign="top" class="style1">
            <rsweb:ReportViewer ID="rpvAcuseDeclaracion" runat="server" Font-Names="Verdana"
                Font-Size="8pt" Height="500px" Width="769px" ShowPrintButton="False">
                <LocalReport ReportPath="ReportDesign\AcuseDeclaracion.rdlc" EnableHyperlinks="False">
                </LocalReport>
            </rsweb:ReportViewer>
      </td>
        </tr>
        <tr>
        <td class="style2">
        
        
            <div style="float: right">
                <asp:Button ID="lnkVolverDeclaraciones" Visible="true" runat="server" CausesValidation="False"
                    OnClick="lnkVolverDeclaraciones_Click" Text="Volver a Declaraciones" />
            </div>
            </td>
        </tr>
    </table>
</asp:Content>
