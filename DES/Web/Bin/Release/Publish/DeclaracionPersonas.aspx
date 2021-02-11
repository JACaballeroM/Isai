<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="DeclaracionPersonas.aspx.cs" Inherits="DeclaracionPersonas" Title="ISAI - Personas" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc2" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
<%@ Register Src="UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmarcion"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <span class="TextoNavegacionUrl"><b>Estas en:</b></span> <span class="TextoNavegacionUrlSub">
        <b>Declaracion ISAI</b></span> <span class="TextoNavegacionUrl"><b>></b></span>
    <span class="TextoNavegacionUrlSub"><b>Declaración</b></span> <span class="TextoNavegacionUrl">
        <b>></b></span> <span class="TextoNavegacionUrl"><b>Participantes</b></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderDContenido" runat="Server">

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdd" />
            <asp:PostBackTrigger ControlID="btnEditar" />
            <asp:PostBackTrigger ControlID="btnEliminar" />
            <asp:PostBackTrigger ControlID="btnVer" />
            <asp:PostBackTrigger ControlID="btnVolver" />
        </Triggers>
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/plus.gif" OnClick="btnAdd_Click"
                            Style="width: 16px" ToolTip="Nuevo participante" />
                        &nbsp;
                        <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/Images/edit_p.gif" OnClick="btnEditar_Click"
                            Width="16px" Enabled="false" ToolTip="Editar participante" />
                        &nbsp;
                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/trash_p.gif"
                            Enabled="false" ToolTip="Eliminar participante" />
                        <cc1:ModalPopupExtender ID="btnEliminar_ModalPopupExtender" runat="server" BackgroundCssClass="PanelModalBackground"
                            DynamicServicePath="" Enabled="True" TargetControlID="btnEliminar" PopupControlID="pnlEliminarModal"
                            DropShadow="True">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlEliminarModal" Style="width: 300px; display: none;" runat="server" SkinID="Modal">
                            <div class="DivPaddingBotton TextCenterMiddle">
                                <uc4:ModalConfirmarcion ID="EliminarModalConfirmacion" runat="server" TextoConfirmacion="Desea continuar con la eliminación."
                                    TextoTitulo="Eliminar" OnConfirmClick="btnEliminarModal_Click" />
                            </div>
                        </asp:Panel>
                        &nbsp;
                        <asp:ImageButton ID="btnVer" runat="server" ImageUrl="~/Images/zoom-in_p.gif" AlternateText="Visualizar Persona"
                            Enabled="false" OnClick="btnVer_Click" ToolTip="Visualizar participante" />
                    </td>
                    <td align="right" class="TextoProgreso">
                        <uc3:Progreso ID="Progreso1" runat="server" Mensaje="Cargando datos..." />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="gridViewPersonas" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        PagerSettings-Mode="Numeric" CssClass="Grid TextLeftTop" GridLines="Horizontal"
                        DataKeyNames="IDPERSONA,CODTIPOPERSONA,IDPARTICIPANTES,IDDIRECCION"
                        OnSelectedIndexChanged="gridViewPersonas_SelectedIndexChanged" EmptyDataText="Declaración sin participantes"
                        AllowSorting="True" OnSorting="gridViewPersonas_Sorting" OnPageIndexChanging="gridViewPersonas_PageIndexChanging">
                        <FooterStyle CssClass="GridFooter Titulo2" />
                        <RowStyle CssClass="GridRow TextoNormal" />
                        <EmptyDataRowStyle CssClass="GridEmptyDataRow TextoNormal" />
                        <Columns>
                            <asp:BoundField HeaderText="Tipo" DataField="TIPOPERSONA" SortExpression="TIPOPERSONA">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Rol" DataField="ROL" SortExpression="ROL" />
                            <asp:BoundField HeaderText="Nombre y apellidos" DataField="NOMBREAPELLIDOS" SortExpression="NOMBREAPELLIDOS">
                                <ItemStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Rfc" DataField="RFC" SortExpression="RFC">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Curp" DataField="CURP" SortExpression="CURP">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Ife" DataField="CLAVEIFE" SortExpression="CLAVEIFE">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="%" DataField="PORCTRANSMISION"
                                SortExpression="PORCTRANSMISION">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Dec">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckbPasivo" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                        Text="Seleccionar" />
                                    <itemstyle width="80px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="GridPager TextoNormal" />
                        <SelectedRowStyle CssClass="GridRowSelected TextoNormal" />
                        <HeaderStyle CssClass="GridHeader Titulo2" />
                        <EditRowStyle CssClass="GridEditRow  TextoNormal" />
                        <AlternatingRowStyle CssClass="GridAlternatingRow TextoNormal" />
                    </asp:GridView>
                    </td>
                </tr>
            </table>
            
            <div class="DivPaddingBotton TextRigthBottom">
                <asp:HiddenField ID="HiddenOperacionPadre" runat="server" />
                <asp:HiddenField ID="HiddenIdPersona" runat="server" />
                <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenCodActoJur" runat="server" />
                <asp:HiddenField ID="HiddenOperacion" runat="server" />
                <asp:HiddenField ID="HiddenCodTipoPersona" runat="server" />
                
                <br />
                <asp:Button ID="btnVolver" runat="server" CssClass="boton" OnClick="lnkVolverDeclaracion_Click"
                    Text="Volver a la declaracion"></asp:Button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
