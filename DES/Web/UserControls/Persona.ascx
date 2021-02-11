<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Persona.ascx.cs" Inherits="ISAI_UserControls_Persona" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>

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

    function ValidadorAlMenosUno(sender, args) {
        // Realizamos las comprobaciones que creamos necesarias para finalmente:
       
        var vTextBoxRFC = document.getElementById("<%=txtRFCDato.ClientID%>").value;
        var vTextBoxCURP = document.getElementById("<%=txtCURPDato.ClientID%>").value;
        var vTextBoxIFE = document.getElementById("<%=txtIFEDato.ClientID%>").value;
        if (ValidatorTrim(vTextBoxRFC).length > 0 || ValidatorTrim(vTextBoxCURP).length > 0 || ValidatorTrim(vTextBoxIFE).length > 0) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
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
<div class="DivPaddingBotton TextoCenterTop">
    <asp:Label ID="lblTipoPersonaTitulo" runat="server" Text="Tipo de persona" CssClass="Titulo2"></asp:Label>
    <asp:Panel ID="pnlTipoPersona" runat="server">
        <asp:RadioButton ID="rbTipoPersonaFisica" runat="server" GroupName="rbTipoPersona"
            Text="Fisica" Checked="true" AutoPostBack="True" OnCheckedChanged="rbTipoPersonaMoral_CheckedChanged"
            CssClass="TextoNormal" />
        <asp:RadioButton ID="rbTipoPersonaMoral" runat="server" GroupName="rbTipoPersona"
            Text="Moral" AutoPostBack="True" OnCheckedChanged="rbTipoPersonaMoral_CheckedChanged"
            CssClass="TextoNormal" />
    </asp:Panel>
</div>
<div class="DivPaddingBotton">
    <asp:Label ID="lblPersonaTitulo" runat="server" Text="Detalle persona" CssClass="Titulo2"></asp:Label>
    <asp:Panel ID="pnlPersona" runat="server">
        <asp:ValidationSummary ID="vsPersona" runat="server" ValidationGroup="ValidarDeclaracionPersona" />
  
        <table  width="100%">
            <tr>
                <td width="20%">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="Titulo2"></asp:Label>
                </td>
                <td  width="80%" colspan="3">
                    <asp:Label ID="lblNombreDato" runat="server" CssClass="TextoNormal" Visible="False" ></asp:Label>
                    <asp:TextBox ID="txtNombreDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase"  MaxLength="100" Width="90.5%"></asp:TextBox>
                </td>
             
            </tr>
            <tr id="trApellidos" runat="server">
                <td width="20%">
                    <asp:Label ID="lblApellidoPaterno" runat="server" Text="Apellido paterno" CssClass="Titulo2"></asp:Label>
                </td>
                <td width="30%">
                    <asp:Label ID="lblApellidoPaternoDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtApellidoPaternoDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase"
                        MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApPaterno" runat="server" ControlToValidate="txtApellidoPaternoDato"
                        CssClass="TextoNormalError" Display="Dynamic" SetFocusOnError="True" ToolTip="Apellido obligatorio"
                        ValidationGroup="ValidarDeclaracionPersona" ErrorMessage="Apellido paterno campo obligatorio">*</asp:RequiredFieldValidator>
                </td>
                <td width="20%">
                    <asp:Label ID="lblApellidoMaterno" runat="server" Text="Apellido materno" CssClass="Titulo2"></asp:Label>
                </td >
                <td width="30%">
                    <asp:Label ID="lblApellidoMaternoDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtApellidoMaternoDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase"
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="20%" id="tdActividadTitulo" runat="server">
                    <asp:Label ID="lblActividad" runat="server" Text="Actividad" CssClass="Titulo2"></asp:Label>
                </td>
                <td width="30%" id="tdActividadLblTxt" runat="server">
                    <asp:Label ID="lblActividadDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtActividadDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase" MaxLength="50"></asp:TextBox>
                </td>
                <td width="20%">
                    <asp:Label ID="lblRFC" runat="server" CssClass="Titulo2" Text="RFC"></asp:Label>
                </td>
                <td width="30%">
                    <asp:Label ID="lblRFCDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtRFCDato" runat="server" CssClass="TextBoxNormal" MaxLength="100"></asp:TextBox>
                             <asp:CustomValidator ID="cvAlMenosUno" runat="server" ClientValidationFunction="ValidadorAlMenosUno"
                        ErrorMessage="Debe introducir datos en al menos un campo. (CURP / RFC / IFE)"
                        ValidationGroup="ValidarDeclaracionPersona" Enabled="true"
                        EnableClientScript="true" SetFocusOnError="False" 
            ValidateEmptyText="True" >*</asp:CustomValidator>
                       <asp:RequiredFieldValidator ID="rfvRfc" runat="server" ControlToValidate="txtRFCDato"
                        CssClass="TextoNormalError" Display="Dynamic" SetFocusOnError="True" ToolTip="RFC obligatorio"
                        ValidationGroup="ValidarDeclaracionPersona" ErrorMessage="RFC campo obligatorio" Enabled="false">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rfvRFCExp" runat="server" ControlToValidate="txtRFCDato"
                        CssClass="TextoNormalError" ValidationExpression="[A-Z]{3,4}[0-9]{6}[A-Z0-9]{3}\s*"
                        Display="Dynamic" SetFocusOnError="True" ToolTip="Formato incorrecto" ValidationGroup="ValidarDeclaracionPersona"
                        ErrorMessage="El RFC debe contener 3 o 4 letras mayúsculas, 6 número y 3 caracteres alfanúmericos">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td width="20%" id="tdCurpTitulo" runat="server">
                    <asp:Label ID="lblCURP" runat="server" CssClass="Titulo2" Text="CURP"></asp:Label>
                </td>
                <td width="30%" id="tdCurpLblTxt" runat="server">
                    <asp:Label ID="lblCURPDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtCURPDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase" MaxLength="100" ></asp:TextBox>
                    <%--#31--%>
                    <%--  <asp:RequiredFieldValidator ID="rfvCurp" runat="server" ControlToValidate="txtCURPDato"
                        CssClass="TextoNormalError" Display="Dynamic" SetFocusOnError="True" ToolTip="CURP obligatorio"
                        ValidationGroup="ValidarDeclaracionPersona" ErrorMessage="CURP campo obligatorio">*</asp:RequiredFieldValidator>--%>
                    <asp:RegularExpressionValidator ID="rfvCURPExp" runat="server" ControlToValidate="txtCURPDato"
                        CssClass="TextoNormalError" ValidationExpression="[A-Z]{4}[0-9]{6}[HM]{1}[A-Z]{5}[A-Z0-9]{1}[0-9]{1}\s*"
                        Display="Dynamic" SetFocusOnError="True" ToolTip="Formato incorrecto" ValidationGroup="ValidarDeclaracionPersona"
                        ErrorMessage="El curp debe contener 4 letras mayúsculas, 6 números, H o M, 5 letras mayúsculas, 1 alfanumérico y 1 número">*</asp:RegularExpressionValidator>
                </td>
                <td width="20%" id="tdIfeTitulo" runat="server">
                    <asp:Label ID="lblIFE" runat="server" CssClass="Titulo2" Text="IFE"></asp:Label>
                </td>
                <td width="30%" id="tdIfeLblTxt" runat="server">
                    <asp:Label ID="lblIFEDato" runat="server" CssClass="TextoNormal" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtIFEDato" runat="server" CssClass="TextBoxNormal" style="text-transform:uppercase" MaxLength="100" ></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rfvIFEExp" runat="server" ControlToValidate="txtIFEDato"
                        ValidationExpression="[A-Z]{6}[0-9]{8}[HM]{1}[0-9]{1}[A-Z0-9]{2}\s*" CssClass="TextoNormalError"
                        Display="Dynamic" SetFocusOnError="True" ToolTip="Formato incorrecto" ValidationGroup="ValidarDeclaracionPersona"
                        ErrorMessage="La clave ife debe contener 6 letras mayúsculas, 8 números, H o M, 1 número y 2 afanuméricos">*</asp:RegularExpressionValidator>
                 
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
