<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeConsultaEspecifica.aspx.cs" Inherits="InformeConsultaEspecifica"
    Title="ISAI - Informe Consulta Específica" %>

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
        function rellenar(quien, que, cuanto) {
            cadcero = '';
            for (i = 0; i < (cuanto - que.length); i++) {
                cadcero += '0';
            }
            quien.value = cadcero + que;
        }
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
    <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBuscar" />
        </Triggers>
        <ContentTemplate>
            <uc3:Progreso ID="progreso1" runat="server" AssociatedUpdatePanelID="UpdatePanelBusqueda"
                                                                        DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando Informe..." />
            <asp:Panel ID="panel1" runat="server">
                <asp:Label ID="LblFiltroAvaluos" SkinID="Titulo2" runat="server" Text="Filtro de avalúos"></asp:Label>
                <table class="TextLeftMiddle">
                    <tr>
                        <td align="left" colspan="3">
                         <asp:HiddenField ID="HiddenRegion" runat="server" />
                         <asp:HiddenField ID="HiddenManzana" runat="server" />
                         <asp:HiddenField ID="HiddenLote" runat="server" />
                         <asp:HiddenField ID="HiddenUnidad" runat="server" />
                         <asp:HiddenField ID="HiddenSujeto" runat="server" />
                         <asp:HiddenField ID="HiddenCalle" runat="server" />
                         <asp:HiddenField ID="HiddenInt" runat="server" />
                            <asp:ValidationSummary ID="vsFiltroAvaluos" runat="server" ValidationGroup="FiltroAvaluos" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbCuenta" runat="server" AutoPostBack="True" Checked="true"
                                CssClass="Titulo2" GroupName="rbBusquedaGroup" OnCheckedChanged="rbBusquedaGroup_CheckedChanged"
                                Text="Búsqueda por cuenta catastral" />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;<td class="style1">
                                <asp:TextBox ID="txtRegion" runat="server" MaxLength="3" SkinID="TextBoxObligatorio"
                                    Width="30px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ControlToValidate="txtRegion"
                                    Display="Dynamic" Enabled="true" ErrorMessage="Requerida una región" SetFocusOnError="True" ValidationGroup="FiltroAvaluos">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtManzana" runat="server" MaxLength="3" SkinID="TextBoxObligatorio"
                                    Width="30px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvManzana" runat="server" ControlToValidate="txtManzana"
                                    Display="Dynamic" Enabled="true" ErrorMessage="Requerida una manzana" SetFocusOnError="True" ValidationGroup="FiltroAvaluos">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtLote" runat="server" MaxLength="2" SkinID="TextBoxObligatorio"
                                    Width="20px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,2);}" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLote" runat="server" ControlToValidate="txtLote" ValidationGroup="FiltroAvaluos"
                                    Display="Dynamic" Enabled="true" ErrorMessage="Requerido un lote" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtUnidadPrivativa" runat="server" MaxLength="3" SkinID="TextBoxObligatorio"
                                    Width="30px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUnidadPrivativa" runat="server" ControlToValidate="txtUnidadPrivativa" ValidationGroup="FiltroAvaluos"
                                    Display="Dynamic" Enabled="true" ErrorMessage="Requerido un condominio" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                               </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbPersonas" runat="server" AutoPostBack="True" Checked="false"
                                CssClass="Titulo2" GroupName="rbBusquedaGroup" OnCheckedChanged="rbBusquedaGroup_CheckedChanged"
                                Text="Búsqueda por adquiriente" />
                        </td>
                        <td>
                            <asp:Label ID="lblSujeto" runat="server" Text="Sujeto:"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtSujeto" runat="server" Enabled="false"
                                    Width="173px" SkinID="TextBoxObligatorio"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="rfvtxtSujeto" runat="server" ControlToValidate="txtSujeto" ValidationGroup="FiltroAvaluos"
                                    Display="Dynamic" Enabled="false" ErrorMessage="Requerido sujeto" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="cvSujeto" runat="server" ControlToValidate="txtSujeto"
                                        CssClass="TextoNormalError" Enabled="false" ValidationExpression=".{3,}" Display="Dynamic" SetFocusOnError="True"
                                        ToolTip="Formato incorrecto" ValidationGroup="FiltroAvaluos" ErrorMessage="El sujeto debe tener como mínimo tres carácteres">*</asp:RegularExpressionValidator>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbUbicacion" runat="server" AutoPostBack="True" Checked="false"
                                CssClass="Titulo2" GroupName="rbBusquedaGroup" OnCheckedChanged="rbBusquedaGroup_CheckedChanged"
                                Text="Búsqueda por ubicación" />
                        </td>
                        <td>
                            <asp:Label ID="lblCalle" runat="server" Text="Calle y Nº Ext.:"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtCalle" runat="server" Enabled="False" MaxLength="200" 
                                    Width="129px"></asp:TextBox>
                                <asp:Label ID="lblNumInt" runat="server" Text="Nº Int.:"></asp:Label>
                                <asp:TextBox ID="txtNumInt" runat="server" Enabled="False" MaxLength="200" 
                                    Width="117px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                                    OnClick="btnBuscar_Click" Width="16px" ValidationGroup="FiltroAvaluos" CausesValidation="true"/>
                                &nbsp;
                            </td>
                        </td>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <rsweb:ReportViewer ID="rpvConsultaEspecifica" runat="server" Font-Names="Verdana"
        Font-Size="8pt" Height="500px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfConsultaEspecifica.rdlc" 
            EnableHyperlinks="True">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
