<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalBuscarPeritos.ascx.cs"
    Inherits="UserControls_ModalBuscarPeritos" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc5" %>
<%@ Register Src="~/UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>

<style type="text/css">
    .style1
    {
        width: 1800px;
    }
    .style3
    {
        width: 587px;
    }
    .style4
    {
        width: 305px;
    }
</style>
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
    function numeroRegistro() {
      
    var registro = document.getElementById("<%=txtNumeroRegistro.ClientID%>").value;
    var Materno = document.getElementById("<%=txtPeritoApellidoMaterno.ClientID%>");
    var Paterno = document.getElementById("<%=txtPeritoApellidoPaterno.ClientID%>");
    var nombre = document.getElementById("<%=txtPeritoNombre.ClientID%>");
    var txt1 = document.getElementById("<%=TextBox1.ClientID%>");
    var valRegistro = document.getElementById("<%=rvtxtNumeroRegistro.ClientID%>");
    var valNombre = document.getElementById("<%=rvtxtPeritoNombre.ClientID%>");
    var valPaterno = document.getElementById("<%=rvtxtPeritoApellidoPaterno.ClientID%>");
    var valMaterno = document.getElementById("<%=rvtxtPeritoApellidoMaterno.ClientID%>");
    if (registro == "") {
        txt1.value = '';
        ValidatorEnable(valRegistro, false);
    }
    else {
        txt1.value = registro;
        if (Materno != null) {
            Materno.value = '';
            Paterno.value = '';
        }
        ValidatorEnable(valMaterno, false);
        
        ValidatorEnable(valPaterno, false);
        nombre.value = '';
        ValidatorEnable(valNombre, false);
        ValidatorEnable(valRegistro, true);

    }
    if (!valRegistro.isvalid) valRegistro.style.visibility = "hidden";
}


function nombre() {
    var registro = document.getElementById("<%=txtNumeroRegistro.ClientID%>");
    var Materno = document.getElementById("<%=txtPeritoApellidoMaterno.ClientID%>")
    var Paterno = document.getElementById("<%=txtPeritoApellidoPaterno.ClientID%>");
    var nombre = document.getElementById("<%=txtPeritoNombre.ClientID%>").value;
    var txt1 = document.getElementById("<%=TextBox1.ClientID%>");
    var valNombre = document.getElementById("<%=rvtxtPeritoNombre.ClientID%>");

    var valRegistro = document.getElementById("<%=rvtxtNumeroRegistro.ClientID%>");
    if (nombre == "") {

        if (Paterno != null && Materno != null) {
            if (Paterno.value == "" && Materno.value == "") {
                txt1.value = "";
            }
            ValidatorEnable(valNombre, false);
        }
        else {
            txt1.value = "";
            ValidatorEnable(valNombre, false);
        }
    }
    else {
        txt1.value = nombre;
        registro.value = "";
        ValidatorEnable(valRegistro, false);
        ValidatorEnable(valNombre, true);

    }
    if (!valNombre.isvalid) valNombre.style.visibility = "hidden";
}

function paterno() {
    var registro = document.getElementById("<%=txtNumeroRegistro.ClientID%>");
    var Materno = document.getElementById("<%=txtPeritoApellidoMaterno.ClientID%>").value;
    var Paterno = document.getElementById("<%=txtPeritoApellidoPaterno.ClientID%>").value;
    var nombre = document.getElementById("<%=txtPeritoNombre.ClientID%>").value;
    var txt1 = document.getElementById("<%=TextBox1.ClientID%>");
    var valPaterno = document.getElementById("<%=rvtxtPeritoApellidoPaterno.ClientID%>");

    var valRegistro = document.getElementById("<%=rvtxtNumeroRegistro.ClientID%>");
    if (Paterno == "") {
        if (nombre == "" && Materno == "") {
            txt1.value = "";
        }
        ValidatorEnable(valPaterno, false);
    }
    else {
        txt1.value = Paterno;
        registro.value = "";
        ValidatorEnable(valRegistro, false);
        ValidatorEnable(valPaterno, true);


    }
    if (!valPaterno.isvalid) valPaterno.style.visibility = "hidden";
}
function materno() {
    var registro = document.getElementById("<%=txtNumeroRegistro.ClientID%>");
    var Materno = document.getElementById("<%=txtPeritoApellidoMaterno.ClientID%>").value;
    var Paterno = document.getElementById("<%=txtPeritoApellidoPaterno.ClientID%>").value;
    var nombre = document.getElementById("<%=txtPeritoNombre.ClientID%>").value;
    var txt1 = document.getElementById("<%=TextBox1.ClientID%>");
    var valMaterno = document.getElementById("<%=rvtxtPeritoApellidoMaterno.ClientID%>");

    var valRegistro = document.getElementById("<%=rvtxtNumeroRegistro.ClientID%>");
    if (Materno == "") {
        if (nombre == "" && Paterno == "") {
            txt1.value = "";
        }
        ValidatorEnable(valMaterno, false);
    }
    else {
        txt1.value = Materno;
        registro.value = "";
        ValidatorEnable(valRegistro, false);
        ValidatorEnable(valMaterno, true);

    }
    if (!valMaterno.isvalid) valMaterno.style.visibility = "hidden";
}


</script>
<div class="DivPaddingBotton TextCenterMiddle">
    <asp:HiddenField ID="hiddenPnlGuardarModalOK_Extender" runat="server" />
    <cc1:ModalPopupExtender ID="extenderPnlGuardarModalOK_Extender" runat="server" Enabled="True"
        TargetControlID="hiddenPnlGuardarModalOK_Extender" PopupControlID="PanelInfoGuardarModal"
        BackgroundCssClass="PanelModalBackground" DropShadow="True">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PanelInfoGuardarModal" Style="width: 280px; display: none;" runat="server"
        SkinID="Modal">
        <uc4:ModalInfo ID="ModalInfoGuardar" runat="server" />
    </asp:Panel>
    <table class="TablaCabeceraCaja" style="width: 100%">
        <tr>
            <td style="width: 50%">
                <%--#28  Cambia el text--%>
                <asp:Label ID="lblBuscarPersonaModalFiltroTitulo" runat="server" class="TextLeftTop"
                    Text="Perito o Sociedad" SkinID="None"></asp:Label>
            </td>
            <td style="width: 50%">
                <asp:ImageButton ID="btnBuscarPersonaModalCancelar" runat="server" SkinID="BotonBarraCerrar"
                    ImageAlign="Right" ImageUrl="~/Images/x.gif" OnClick="btnBuscarPersonaModalCancelar_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="pnlBucarPersonasModalFiltros" runat="server" SkinID="Detalle" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="TextLeftMiddle" style="width: 100%">
                <%-- #28--%>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblBusquedaPor" runat="server" SkinID="Titulo2" 
                            Text="Búsqueda por:  "></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" Style="display: none"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTxt" runat="server" ValidationGroup="FiltroPersonas"
                            ErrorMessage="Al menos debe indicar uno de los campos" ControlToValidate="TextBox1">*
                        </asp:RequiredFieldValidator>
                    </td>
                    <td align="left" rowspan="2" class="style1">
                        <asp:ValidationSummary ID="FiltroPersonasID" runat="server" ValidationGroup="FiltroPersonas"
                            Height="16px" />
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlOpcionBusqueda" runat="server" OnSelectedIndexChanged="ddlOpcionBusqueda_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Selected="True" Text="Clave perito" Value="perito"></asp:ListItem>
                            <asp:ListItem Text="Clave sociedad" Value="sociedad"></asp:ListItem>
                        </asp:DropDownList> <asp:Label ID="lblPerito" runat="server" SkinID="Titulo2" Text="Clave Perito" Visible="false"></asp:Label>
                    </td>
                    <%--end#28 borrar label registro--%>
                    <td class="style3">
                    </td>
                    <td class="TextRigthBottom" rowspan="7" style="width: 20px">
                        <asp:ImageButton ID="btnBuscarPersonaModalFiltrar" runat="server" ImageAlign="Right"
                            ImageUrl="~/Images/search.gif" OnClick="btnBuscarPersonaModalFiltrar_Click" ValidationGroup="FiltroPersonas" />
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblRegistro" runat="server" SkinID="Titulo2" Text="Registro"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtNumeroRegistro" runat="server" 
                            Style="width: 98%" onkeyup="javascript:numeroRegistro();"></asp:TextBox><%--OnTextChanged="txtNumeroRegistro_TextChanged"--%>
                    </td>
                    <td >
                        <asp:RegularExpressionValidator ID="rvtxtNumeroRegistro" runat="server" ControlToValidate="txtNumeroRegistro"
                            ValidationExpression="[0-9a-zA-Z-|á|é|í|ó|ú|ñ|Ñ|Á|É|Í|Ó|Ú| ]{2,}" ValidationGroup="FiltroPersonas" Enabled="false">Mínimo dos carácteres</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblBuscarPersonaModalNombre" runat="server" SkinID="Titulo2" Text="Nombre"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtPeritoNombre" runat="server" Style="width: 98%" onkeyup="javascript:nombre();"
                           ></asp:TextBox><%--OnTextChanged="txtPeritoNombre_TextChanged"--%>
                    </td>
                    <td >
                        <asp:RegularExpressionValidator ID="rvtxtPeritoNombre" runat="server" ControlToValidate="txtPeritoNombre"
                            ValidationExpression="[0-9a-zA-Z-|á|é|í|ó|ú|ñ|Ñ|Á|É|Í|Ó|Ú| ]{2,}" ValidationGroup="FiltroPersonas"  Enabled="false">Mínimo dos carácteres</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblPeritoApellidoPaterno" runat="server" SkinID="Titulo2" Text="Apellido Paterno"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtPeritoApellidoPaterno" runat="server"  Style="width: 98%"
                            onkeyup="javascript:paterno();"></asp:TextBox><%--OnTextChanged="txtPeritoApellidoPaterno_TextChanged"--%>
                    </td>
                    <td >
                        <asp:RegularExpressionValidator ID="rvtxtPeritoApellidoPaterno" runat="server" ControlToValidate="txtPeritoApellidoPaterno"
                            ValidationExpression="[0-9a-zA-Z-|á|é|í|ó|ú|ñ|Ñ|Á|É|Í|Ó|Ú| ]{2,}" ValidationGroup="FiltroPersonas" Enabled="false" >Mínimo dos carácteres</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblPeritoApellidoMaterno" runat="server" SkinID="Titulo2" Text="Apellido Materno"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtPeritoApellidoMaterno" runat="server"  Style="width: 98%" onkeyup="javascript:materno();"
                            > </asp:TextBox><%--OnTextChanged="txtPeritoApellidoMaterno_TextChanged"--%>
                    </td>
                    <td >
                        <asp:RegularExpressionValidator ID="rvtxtPeritoApellidoMaterno" runat="server" ControlToValidate="txtPeritoApellidoMaterno"
                            ValidationExpression="[0-9a-zA-Z-|á|é|í|ó|ú|ñ|Ñ|Á|É|Í|Ó|Ú| ]{2,}" ValidationGroup="FiltroPersonas" Enabled="false" >Mínimo dos carácteres</asp:RegularExpressionValidator>
                    </td>
                </tr>
                 <div id="dicRFC" runat="server" visible="false">
                <tr>
                    <td class="style4">
                        <%--#28 visible a false --%>
                        <asp:Label ID="lblPeritoRFC" runat="server" SkinID="Titulo2" Text="RFC" Visible="false"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtPeritoRFC" runat="server" Style="width: 95%" Visible="false"
                            OnTextChanged="txtPeritoRFC_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblPeritoCURP" runat="server" SkinID="Titulo2" Text="CURP" Visible="false"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtPeritoCURP" runat="server" Style="width: 95%" Visible="false"
                            OnTextChanged="txtPeritoCURP_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lblPeritoIFE" runat="server" SkinID="Titulo2" Text="IFE" Visible="false"></asp:Label>
                    </td>
                    <td class="style1" >
                        <asp:TextBox ID="txtPeritoIFE" runat="server" Style="width: 95%" Visible="false"
                            OnTextChanged="txtPeritoIFE_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                </div>
            </table>
        </ContentTemplate>
  <%--      <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtNumeroRegistro" EventName ="TextChanged"/>
              <asp:AsyncPostBackTrigger ControlID="txtPeritoApellidoPaterno" EventName ="TextChanged"/>
                <asp:AsyncPostBackTrigger ControlID="txtPeritoApellidoMaterno" EventName ="TextChanged"/>
                  <asp:AsyncPostBackTrigger ControlID="txtPeritoNombre" EventName ="TextChanged"/>
        </Triggers>--%>
    </asp:UpdatePanel>
</div>
<div class="DivPaddingBotton TextLeftMiddle">
    <asp:Label ID="lblBuscarPersonaModalFiltroResultado" runat="server" SkinID="Titulo2"
        class="TextLeftTop" Text="Resultado"></asp:Label>
    <asp:ObjectDataSource ID="odsPorBusquedaPerito" runat="server"
        SelectMethod="ObtenerBusquedaPeritos" TypeName="DsePeritosPagger" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtNumeroRegistro" Name="registro" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtPeritoNombre" Name="nombre" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtPeritoApellidoPaterno" Name="apellidoPaterno"
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPeritoApellidoMaterno" Name="apellidoMaterno"
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPeritoRFC" Name="rfc" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPeritoCURP" Name="curp" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtPeritoIFE" Name="claveife" PropertyName="Text"
                Type="String" />
            </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsPorBusquedaSociedad" runat="server"
        MaximumRowsParameterName="" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="ObtenerBusquedaSociedades" StartRowIndexParameterName="" 
        TypeName="DsePeritosPagger">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtNumeroRegistro" Name="registro" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtPeritoNombre" Name="nombre" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtPeritoRFC" Name="rfc" PropertyName="Text" Type="String" />
          
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscarPersonaModalFiltrar" />
            
        </Triggers>
        <ContentTemplate>
            <asp:GridView ID="gridViewPersonas" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" DataKeyNames="REGISTRO" GridLines="Horizontal"
                OnSelectedIndexChanged="gridViewPersonas_SelectedIndexChanged" 
                PageSize="8" OnPageIndexChanging="gridViewPersonas_PageIndexChanging"
                OnSorting="gridViewPersonas_Sorting" 
                EmptyDataText="No hay registros para mostrar"
                DataSourceID="odsPorBusquedaPerito">
                <Columns>
                
                    <asp:BoundField DataField="REGISTRO" HeaderText="Registro" SortExpression="REGISTRO">
                        <ItemStyle Width="25%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre y apellidos" 
                        SortExpression="NombreCompleto">
                        <ItemStyle Width="70%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" Visible="false">
                    </asp:BoundField>
                    <asp:BoundField DataField="CURP" HeaderText="CURP" SortExpression="CURP" Visible="false">
                    </asp:BoundField>
                    <asp:BoundField DataField="CLAVEIFE" HeaderText="IFE" SortExpression="CLAVEIFE" Visible="false">
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                Text="Sel" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gridViewSociedades" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" DataKeyNames="REGISTRO" GridLines="Horizontal"
                OnSelectedIndexChanged="gridViewSociedades_SelectedIndexChanged" 
                PageSize="8" OnPageIndexChanging="gridViewSociedades_PageIndexChanging"
                OnSorting="gridViewSociedades_Sorting" 
                EmptyDataText="No hay registros para mostrar" 
                DataSourceID="odsPorBusquedaSociedad" Visible="False">
                <Columns>
                
                    <asp:BoundField DataField="REGISTRO" HeaderText="Registro" 
                        SortExpression="REGISTRO">
                    </asp:BoundField>
                    <asp:BoundField DataField="RAZONSOCIAL" HeaderText="Razón Social" 
                        SortExpression="RAZONSOCIAL">
                    </asp:BoundField>
                                          <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                Text="Sel" />
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div>
    <table width="100%">
        <tr class="TablaCeldaCaja">
            <td>
                <asp:UpdatePanel ID="updatePanelBotones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="display: inline; float: right">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click"
                                Enabled="false" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</div>
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
