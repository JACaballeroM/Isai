<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeValores.aspx.cs" Inherits="InformeValores" Title="ISAI - Informe Valores Unitarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4
        {
            width: 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe de valores unitarios</h3>
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
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label ID="lblTituloPanel" class="TextLeftTop" SkinID="Titulo2" runat="server"
            Text="Filtro de avalúos"></asp:Label>
        <br />
        <table>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="vsValores" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style4">
                    <asp:Label ID="lblNumSociedad" runat="server" Text="Nº Sociedad"></asp:Label>
                    &nbsp;&nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtNumSociedad" runat="server" EnableTheming="True" Width="106px"></asp:TextBox>
                    <asp:CompareValidator ID="cvNumeroSociedad" runat="server" 
                        ControlToValidate="txtNumSociedad" Display="Dynamic" 
                        ErrorMessage="Nº sociedad errónea" ForeColor="Blue" 
                        Operator="DataTypeCheck">!</asp:CompareValidator>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="style4">
                    <asp:Label ID="lblNumPerito" runat="server" Text="Nº Perito"></asp:Label>
                    &nbsp;&nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtNumPerito" runat="server" EnableTheming="True" Width="106px"></asp:TextBox>
                    <asp:CompareValidator ID="cvNumPerito" runat="server" 
                        ControlToValidate="txtNumPerito" Display="Dynamic" 
                        ErrorMessage="Nº Perito erróneo" ForeColor="Blue" Operator="DataTypeCheck">!</asp:CompareValidator>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="style4">
                    <asp:Label ID="lblColoniaNominal" runat="server" Text="Colonia nominal"></asp:Label>
                    &nbsp;&nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtColoniaNominal" runat="server" EnableTheming="True" Width="106px"></asp:TextBox>
                    <asp:CompareValidator ID="cvColoniaNominal" runat="server" 
                        ControlToValidate="txtColoniaCatastral" Display="Dynamic" 
                        ErrorMessage="Colonia nominal errónea" ForeColor="Blue" 
                        Operator="DataTypeCheck">!</asp:CompareValidator>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="style4">
                    <asp:Label ID="lblColoniaCatastral" runat="server" Text="Colonia catastral"></asp:Label>
                    &nbsp;&nbsp;
                </td>
                <td>
                    &nbsp;<asp:TextBox ID="txtColoniaCatastral" runat="server" EnableTheming="True" Width="106px"></asp:TextBox>
                    <asp:CompareValidator ID="cvColoniaCatastral" runat="server" 
                        ControlToValidate="txtColoniaCatastral" Display="Dynamic" 
                        ErrorMessage="Colonia catastral errónea" ForeColor="Blue" 
                        Operator="DataTypeCheck" Type="Integer">!</asp:CompareValidator>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblTipoValorUnitario" runat="server" CssClass="Titulo2" 
            Text="Tipo valor unitario"></asp:Label>
        <asp:Panel ID="pnlValores" runat="server" SkinID="Detalle" Width="319px">
            <asp:RadioButton ID="rbRenta" runat="server" Checked="True" 
                CssClass="TextoNormal" GroupName="rbValores" 
                Text="Valores unitarios de renta" />
            &nbsp;
            <asp:RadioButton ID="rbMercado" runat="server" CssClass="TextoNormal" 
                GroupName="rbValores" Text="Valores unitarios de mercado" />
        </asp:Panel>
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <table>
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
                    <span class="TextoNormal">- </span>
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
                </td>
                <td rowspan="3">
                    <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                        OnClick="btnBuscar_Click" CausesValidation="true" />
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <rsweb:ReportViewer ID="rpvValores" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="400px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfValores.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
