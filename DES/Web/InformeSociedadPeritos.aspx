<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="InformeSociedadPeritos.aspx.cs" Inherits="InformeSociedadPeritos"
    Title="ISAI - Informe Sociedad / Peritos" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 51px;
            height: 100px;
        }
        .style2
        {
            height: 146px;
        }
        .style3
        {
            height: 100px;
        }
        .style4
        {
            height: 100px;
            width: 112px;
        }
        
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe Sociedad / Peritos
    </h3>
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
            <cc1:ModalPopupExtender ID="mpeErrorTareas" runat="server" Enabled="True" TargetControlID="hlnErrorTareas"
                PopupControlID="panErrorTareas" DropShadow="false" BackgroundCssClass="PanelModalBackground" />
            <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="display: none" />
            <asp:Panel ID="panErrorTareas" runat="server" Style="display: none" SkinID="Modal">
                <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="Panel2" runat="server">
        <asp:Label ID="lblTituloPanel" class="TextLeftTop" SkinID="Titulo2" runat="server"
            Text="Filtro de avalúos"></asp:Label>
        <table class="TextLeftMiddle">
            <tr>
                <td colspan="2">
                    <asp:HiddenField ID="HiddenTipoEntidad" runat="server" />
                    <asp:HiddenField ID="HiddenEstadoDeclaracion" runat="server" />
                    <asp:HiddenField ID="HiddenFechaIni" runat="server" />
                    <asp:HiddenField ID="HiddenFechaFin" runat="server" />
                    <asp:HiddenField ID="HiddenTipoFecha" runat="server" />
                    <asp:ValidationSummary ID="vsFiltro" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset class="formulario">
                        <legend class="formulario">Tipo entidad</legend>
                        <asp:Panel ID="pnlPeritoSociedad" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbPerito" runat="server" CssClass="TextoNormal" GroupName="rbPeritoSociedad"
                                            Text="Perito" Checked="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbSociedad" runat="server" CssClass="TextoNormal" GroupName="rbPeritoSociedad"
                                            Text="Sociedad" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="DivPaddingBotton">
                                <fieldset class="formulario">
                                    <legend class="formulario">Estado declaraciones</legend>
                                    <asp:Panel ID="PanelPagadosEnviados" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbEnviados" runat="server" Checked="True" CssClass="TextoNormal"
                                                        GroupName="rbEnviadosPagados" Text="Todos los enviados" OnCheckedChanged="rbEnviados_CheckedChanged"
                                                        AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbPagados" runat="server" CssClass="TextoNormal" GroupName="rbEnviadosPagados"
                                                        Text="Todos los pagados" OnCheckedChanged="rbPagados_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="DivPaddingBotton">
                                <fieldset class="formulario">
                                    <legend class="formulario">Fechas</legend>
                                    <asp:Panel ID="Panel3" runat="server" Width="337px">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFechas" runat="server" Text="Desde "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFecheaInicio" runat="server" ErrorMessage="Requerida una fecha"
                                                        ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea"
                                                        ForeColor="Blue" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaIni"
                                                        Display="Dynamic">!</asp:CompareValidator><cc1:CalendarExtender ID="txtFechaIni_CalendarExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtFechaIni">
                                                        </cc1:CalendarExtender>
                                                    <asp:Label ID="Label1" runat="server" Text="hasta "></asp:Label>
                                                    <asp:TextBox ID="txtFechaFin" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
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
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="rbFechaAvaluo" runat="server" CssClass="TextoNormal" GroupName="rbFecha"
                                                        Text="Fecha avalúo" Checked="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:RadioButton ID="rbFechaPres" runat="server" CssClass="TextoNormal" GroupName="rbFecha"
                                                        Text="Fecha presentación" />
                                                </td>
                                            </tr>
                                            <div id="divFechaPago" runat="server" visible="false">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButton ID="rbFechaPago" runat="server" CssClass="TextoNormal" GroupName="rbFecha"
                                                            Text="Fecha pago" />
                                                    </td>
                                                </tr>
                                            </div>
                                        </table>
                                    </asp:Panel>
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="TextRigthBottom">
                    <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                        OnClick="btnBuscar_Click" CausesValidation="true" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <rsweb:ReportViewer ID="rpvPeritosSociedades" Visible="false" runat="server" Font-Names="Verdana"
        Font-Size="8pt" Height="400px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfSociedad.rdlc" EnableHyperlinks="true">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
