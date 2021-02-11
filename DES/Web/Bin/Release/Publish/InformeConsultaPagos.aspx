<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterBase.master" AutoEventWireup="true"
    CodeFile="InformeConsultaPagos.aspx.cs" Inherits="InformeConsultaPagos" Title="ISAI - Informe Consulta Específica" %>

<%@ Register Src="UserControls/Persona.ascx" TagName="Persona" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="progreso" TagPrefix="uc3" %>

<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 366px;
        }
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMenuCabecera"
    runat="Server">

    <script src="<%= this.ResolveUrl("~/JS/funciones.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        //función javascript que llama a otra funcion para validar si estan todos los campos de la cuenta catastral
        function customValidateCuentaCatastral(source, arguments) {
            arguments.IsValid = camposObligatoriosCuentaCatastral('<%= txtRegion.ClientID %>', '<%= txtManzana.ClientID %>', '<%= txtLote.ClientID %>', '<%= txtUnidadPrivativa.ClientID %>');
        }

        function prueba() {
            var Code = "";
        }

        function rellenar(quien, que, cuanto) {
            cadcero = '';
            for (i = 0; i < (cuanto - que.length); i++) {
                cadcero += '0';
            }
            quien.value = cadcero + que;
        }
        //esta funcion se llama cada vez que se completa la pulsacion de una tecla

        function AceptarDigitosPunto(e) {
            var Code = e.keyCode;
            //0-9 y .
            if ((Code >= 48 && Code <= 57) || Code == 46) {
                return true;
            }
            else {
                return false;
            }
        }

        function AceptarDigitos(e) {
            var Code = e.keyCode;
            //0-9
            if (Code >= 48 && Code <= 57) {
                return true;
            }
            else {
                return false;
            }
        }

        function comprobarValidadores() {
            var region = document.getElementById("<%=txtRegion.ClientID%>").value;
            var manzana = document.getElementById("<%=txtManzana.ClientID%>").value;
            var lote = document.getElementById("<%=txtLote.ClientID%>").value;
            var unidadprivativa = document.getElementById("<%=txtUnidadPrivativa.ClientID%>").value;
            var valregion = document.getElementById("<%=cvRegion.ClientID%>");
            var valmanzana = document.getElementById("<%=cvManzana.ClientID%>");
            var vallote = document.getElementById("<%=cvLote.ClientID%>");
            var valunidadprivativa = document.getElementById("<%=cvCuentaCatastral.ClientID%>");
            if (region == "" && manzana == "" && lote == "" && unidadprivativa == "") {
                ValidatorEnable(valregion, false);
                ValidatorEnable(valmanzana, false);
                ValidatorEnable(vallote, false);
                ValidatorEnable(valunidadprivativa, false);
                document.getElementById("<%=btnBuscar.ClientID%>").focus();
            }
            else if (unidadprivativa != "") {
                ValidatorEnable(valregion, true);
                ValidatorEnable(valmanzana, true);
                ValidatorEnable(vallote, true);
                ValidatorEnable(valunidadprivativa, true);
            }
            else if (lote != "") {
                ValidatorEnable(valregion, true);
                ValidatorEnable(valmanzana, true);
                ValidatorEnable(vallote, true);
                ValidatorEnable(valunidadprivativa, false);
               
            }
            else if (region != "" || manzana != "") {
                ValidatorEnable(valregion, true);
                ValidatorEnable(valmanzana, true);
                ValidatorEnable(vallote, false);
                ValidatorEnable(valunidadprivativa, false);
               
            }
            
          
        }
    
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMenuGlobal" runat="Server">
    <h3>
        ISAI - Informe de consulta de pagos</h3>
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
    <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBuscar" />
             <asp:AsyncPostBackTrigger ControlID="txtRegion" />
            <asp:AsyncPostBackTrigger ControlID="txtManzana" />
            <asp:AsyncPostBackTrigger ControlID="txtLote" />
            <asp:AsyncPostBackTrigger ControlID="txtUnidadPrivativa" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panel1" runat="server">
                <asp:Label ID="LblFiltroAvaluos" SkinID="Titulo2" runat="server" Text="Filtro de avalúos"></asp:Label>
                <table class="TextLeftMiddle">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:ValidationSummary ID="vsFiltroPagos" runat="server" ValidationGroup="FiltroPagos" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRegion" runat="server" Text="Cuenta catastral:"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtRegion" runat="server" MaxLength="3" Width="30px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);};comprobarValidadores();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="cvRegion" runat="server" ControlToValidate="txtRegion" ForeColor="Red"
                                    ValidationGroup="FiltroPagos" Display="Dynamic" Enabled="false" >*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtManzana" runat="server" MaxLength="3" Width="30px"  onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);};comprobarValidadores();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="cvManzana" runat="server" ControlToValidate="txtManzana"
                                    ForeColor="Red" ValidationGroup="FiltroPagos" Display="Dynamic" Enabled="false" >*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtLote" runat="server" MaxLength="2" Width="20px" onblur="javascript:if(this.value!=''){rellenar(this,this.value,2);};comprobarValidadores();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="cvLote" runat="server" 
                                    ControlToValidate="txtLote" ForeColor="Red"
                                    ValidationGroup="FiltroPagos" Display="Dynamic" 
                                     Enabled="False">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtUnidadPrivativa" runat="server" MaxLength="3" Width="30px" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);};comprobarValidadores();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="cvCuentaCatastral" runat="server" ControlToValidate="txtUnidadPrivativa"
                                    ForeColor="Red" ValidationGroup="FiltroPagos" Display="Dynamic" 
                                    Enabled="False">*</asp:RequiredFieldValidator>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNumDeclaracion" runat="server" Text="Número Declaración:"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtNumDeclaracion" runat="server" Enabled="true" Width="102px"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblLineaCaptura" runat="server" Text="Línea de Captura:"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtLineaCaptura" runat="server" MaxLength="200" Enabled="true" Width="180px"></asp:TextBox>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblTipoValorUnitario" runat="server" CssClass="Titulo2" Text="Tipo de pago"></asp:Label>
                            <asp:Panel ID="pnlValores" runat="server" SkinID="Detalle" Width="453px">
                                <asp:RadioButton ID="rbConformados" runat="server"  Checked="true"
                                    CssClass="Titulo2" GroupName="rbBusquedaGroup" Text="Pagos efectuados" />
                                &nbsp;
                                <asp:RadioButton ID="rbVencidos" runat="server"  Checked="false"
                                    CssClass="Titulo2" GroupName="rbBusquedaGroup" Text="Pagos vencidos" />
                                     &nbsp;
                                <asp:RadioButton ID="rbSiscor" runat="server" 
                                    Checked="false" CssClass="Titulo2" GroupName="rbBusquedaGroup" 
                                    Text="Pagos recibidos en Siscor" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFechas" runat="server" Text="Fechas"></asp:Label>
                            <td class="style1">
                                <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFecheaInicio" runat="server" ErrorMessage="Requerida una fecha"
                                    ValidationGroup="FiltroPagos" ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                                        ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea" ForeColor="Blue"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFechaIni" Display="Dynamic"
                                        ValidationGroup="FiltroPagos">!</asp:CompareValidator><cc1:CalendarExtender ID="txtFechaIni_CalendarExtender"
                                            runat="server" Enabled="True" TargetControlID="txtFechaIni">
                                        </cc1:CalendarExtender>
                                <span class="TextoNormal">-</span>
                                <asp:TextBox ID="txtFechaFin" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ErrorMessage="Requerida una fecha"
                                    ControlToValidate="txtFechaFin" Display="Dynamic" ValidationGroup="FiltroPagos">*</asp:RequiredFieldValidator><asp:CompareValidator
                                        ID="cvFechaFin" runat="server" ErrorMessage="Fecha errónea" Operator="DataTypeCheck"
                                        Type="Date" ControlToValidate="txtFechaFin" Display="Dynamic" ValidationGroup="FiltroPagos">!</asp:CompareValidator><asp:CompareValidator
                                            ID="cvRangoFechas" runat="server" ErrorMessage="Rango entre fechas erróneo" ForeColor="Blue"
                                            ControlToValidate="txtFechaIni" Type="Date" ControlToCompare="txtFechaFin" Operator="LessThan"
                                            Display="Dynamic" ValidationGroup="FiltroPagos">!</asp:CompareValidator>
                                <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtFechaFin">
                                </cc1:CalendarExtender>
                                <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" ImageUrl="~/Images/search.gif"
                                    OnClick="btnBuscar_Click"   CausesValidation="true" ValidationGroup="FiltroPagos" />
                            </td>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <rsweb:ReportViewer ID="rpvPagosCon" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="500px" Width="100%" ShowPrintButton="False">
        <LocalReport ReportPath="ReportDesign\InfPagosCon.rdlc" EnableHyperlinks="True">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
