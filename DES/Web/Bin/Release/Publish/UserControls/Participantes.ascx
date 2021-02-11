<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Participantes.ascx.cs"
    Inherits="UserControls_Participantes" %>
<%@ Register Src="~/UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Src="~/UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="~/UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmarcion"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc5" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<fieldset class="formulario">
    <legend class="formulario">Participantes</legend>

    <asp:UpdatePanel ID="UpdatePanelParticipantes" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdd" />
            <asp:PostBackTrigger ControlID="btnEditar" />
            <asp:PostBackTrigger ControlID="btnVer" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="HiddenDeshabilitarBotones" />
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/plus.gif" 
                            Enabled="false" Style="width: 16px" ToolTip="Nuevo participante" Width="16px" />
                        <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/Images/edit_p.gif" OnClick="btnEditar_Click"
                            Width="16px" Enabled="false" ToolTip="Editar participante" />
                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/trash_p.gif"
                            Enabled="false" ToolTip="Eliminar participante" Height="16px" />
                        <cc1:ModalPopupExtender ID="btnEliminar_ModalPopupExtender" runat="server" BackgroundCssClass="PanelModalBackground"
                            DynamicServicePath="" Enabled="True" TargetControlID="btnEliminar" PopupControlID="pnlEliminarModal"
                            DropShadow="True">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlEliminarModal" Style="width: 300px; display: none;" runat="server"
                            SkinID="Modal">
                            <div class="DivPaddingBotton TextCenterMiddle">
                                <uc4:ModalConfirmarcion ID="EliminarModalConfirmacion" runat="server" TextoConfirmacion="Desea continuar con la eliminación."
                                    TextoTitulo="Eliminar" OnConfirmClick="btnEliminarModal_Click" />
                            </div>
                        </asp:Panel>
                        <asp:ImageButton ID="btnVer" runat="server" ImageUrl="~/Images/zoom-in_p.gif" AlternateText="Visualizar Persona"
                            Enabled="false" OnClick="btnVer_Click" ToolTip="Visualizar participante" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="uppInfoModal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>
                    <cc1:ModalPopupExtender ID="ccInfo" runat="server" Enabled="True" TargetControlID="HiddenInfo"
                        PopupControlID="pnlInfoModal" DropShadow="false" BackgroundCssClass="PanelModalBackground" />
                    <asp:HiddenField ID="HiddenInfo" runat="server" />
                    <asp:Panel ID="pnlInfoModal" runat="server" Width="27%" Style="display: none" SkinID="Modal">
                        <uc5:ModalInfo ID="ModalInfo" runat="server" />
                        <%--<asp:Label ID="lblNoCuentaCat" runat="server" Visible="False" ForeColor="#CC0000" ></asp:Label>--%>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="uppGridParticipantes" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc3:Progreso ID="ProgresoParticipantes" runat="server" AssociatedUpdatePanelID="uppGridParticipantes"
                                            DisplayAfter="0"  Mensaje="Espere..." DynamicLayout="true"/>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="gridViewPersonas" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    PagerSettings-Mode="Numeric" CssClass="Grid TextLeftTop" GridLines="Horizontal"
                                    OnRowDataBound="gridViewPersonas_RowDataBound" DataKeyNames="IDPERSONA,CODTIPOPERSONA,IDPARTICIPANTES,IDDIRECCION,PORCTRANSMISION"
                                    OnSelectedIndexChanged="gridViewPersonas_SelectedIndexChanged" EmptyDataText="Declaración sin participantes"
                                    AllowSorting="True" OnSorting="gridViewPersonas_Sorting" OnPageIndexChanging="gridViewPersonas_PageIndexChanging"
                                    OnDataBound="gridviewPersonas_DataBound">
                                    <FooterStyle CssClass="GridFooter Titulo2" />
                                    <RowStyle CssClass="GridRow TextoNormal" />
                                    <EmptyDataRowStyle CssClass="GridEmptyDataRow TextoNormal" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Tipo" DataField="TIPOPERSONA" SortExpression="TIPOPERSONA">
                                            <ItemStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Rol" DataField="ROL" SortExpression="ROL" Visible="false">
                                            <ItemStyle Width="40px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Rol" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRol" runat="server" Text='<%#Eval("ROL") %>' Visible="false"
                                                    Font-Size="8"></asp:Label>
                                                <asp:DropDownList ID="DropDownRol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRol_SelectedIndexChanged"
                                                    Font-Size="8" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="70px" />
                                        </asp:TemplateField>
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
                                        <asp:BoundField HeaderText="%" DataField="PORCTRANSMISION" DataFormatString="{0:P}"
                                            SortExpression="PORCTRANSMISION" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="55px">
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                                    Text="Seleccionar" Width="70px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="GridPager TextoNormal" />
                                    <SelectedRowStyle CssClass="GridRowSelected TextoNormal" />
                                    <HeaderStyle CssClass="GridHeader Titulo2" />
                                    <EditRowStyle CssClass="GridEditRow  TextoNormal" />
                                    <AlternatingRowStyle CssClass="GridAlternatingRow TextoNormal" />
                                </asp:GridView>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="DivPaddingBotton TextRigthBottom">
                <asp:HiddenField ID="HiddenOperacionPadre" runat="server" />
                <asp:HiddenField ID="HiddenIdPersona" runat="server" />
                <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenCodActoJur" runat="server" />
                <asp:HiddenField ID="HiddenOperacion" runat="server" />
                <asp:HiddenField ID="HiddenCodTipoPersona" runat="server" />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
</fieldset>
