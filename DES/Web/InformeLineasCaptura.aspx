﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeLineasCaptura.aspx.cs" Inherits="InformeLineasCaptura" Title="ISAI - Informe Lineas de captura" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
    <p>
        &nbsp;</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe de líneas de captura</h3>
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
        <table>
            <tr>
                <td colspan="2">
                    <asp:HiddenField ID="HiddenNumNotario" runat="server" />
                    <asp:HiddenField ID="HiddenFechaIni" runat="server" />
                    <asp:HiddenField ID="HiddenFechaFin" runat="server" />
                    <asp:ValidationSummary ID="vsFiltro" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNumNotario" runat="server" Text="Nº Notario"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumeroNotario" runat="server" CssClass="TextBoxNormal" SkinID="TextBoxObligatorio"
                        Enabled="true" ></asp:TextBox>
                      <asp:CompareValidator ID="cvNumNotario" runat="server" 
                        ControlToValidate="txtNumeroNotario" Display="Dynamic" 
                        ErrorMessage="Nº Notario erróneo" ForeColor="Blue" 
                        Operator="DataTypeCheck" Type="Integer">!</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFechas" runat="server" Text="Fechas"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" 
                        MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFecheaInicio" runat="server" ErrorMessage="Requerida una fecha"
                         ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                            ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea" ForeColor="Blue"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaIni" Display="Dynamic">!</asp:CompareValidator><cc1:CalendarExtender
                                ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFechaIni">
                            </cc1:CalendarExtender>
                    <span class="TextoNormal">-</span>
                    <asp:TextBox ID="txtFechaFin" runat="server" SkinID="TextBoxObligatorio" 
                        MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ErrorMessage="Requerida una fecha"
                        ControlToValidate="txtFechaFin" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                            ID="cvFechaFin" runat="server" ErrorMessage="Fecha errónea" Operator="DataTypeCheck"
                            Type="Date" ControlToValidate="txtFechaFin" Display="Dynamic">!</asp:CompareValidator><asp:CompareValidator
                                ID="cvRangoFechas" runat="server" ErrorMessage="Rango entre fechas erróneo" ForeColor="Blue"
                                ControlToValidate="txtFechaIni" Type="Date" ControlToCompare="txtFechaFin" Operator="LessThan"
                                Display="Dynamic">!</asp:CompareValidator>
                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtFechaFin">
                    </cc1:CalendarExtender>
                    <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                        OnClick="btnBuscar_Click" CausesValidation="true" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <rsweb:ReportViewer ID="rpvLineasCaptura" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="400px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfLineasCaptura.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
