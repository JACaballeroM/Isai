<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="Declaraciones.aspx.cs" Inherits="Declaraciones" Title="ISAI - Declaraciones" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmarcion"
    TagPrefix="uc4" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc5" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<asp:Content ID="ContentImagen" ContentPlaceHolderID="ContentPlaceHolderDImagen"
    runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="ContentRuta" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <uc5:Navegacion ID="Navegacion1" runat="server" />
</asp:Content>
<asp:Content ID="ContentContenido" ContentPlaceHolderID="ContentPlaceHolderDContenido"
    runat="Server">
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
    <div class="DivPaddingBotton TextLeftTop">
        <fieldset class="formulario">
            <legend class="formulario">Detalle del avalúo</legend>
            <asp:Panel ID="PanelDetalleAvaluo" runat="server">
                <table width="100%">
                    <tr>
                        <td width="20%">
                            <asp:Label ID="lblAvaluo" SkinID="Titulo2" runat="server" Text="Número Único"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:Label ID="lblAvaluoDato" runat="server"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:Label ID="lblCuenta" runat="server" SkinID="Titulo2" Text="Cuenta catastral"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:Label ID="lblCuentaDato" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFecha" runat="server" SkinID="Titulo2" Text="Fecha"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFechaDato" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblEstado" runat="server" SkinID="Titulo2" Text="Estado"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblEstadoDato" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblVigente" runat="server" SkinID="Titulo2" Text="Vigente"></asp:Label>
                        </td>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblVigenteDato" runat="server"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblCalle" runat="server" SkinID="Titulo2" Text="Calle"></asp:Label>
                        </td>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblCalleDato" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblNoExt" runat="server" SkinID="Titulo2" Text="No. Exterior"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblNoExtDato" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblNoInt" runat="server" SkinID="Titulo2" Text="No. Interior"></asp:Label>
                        </td>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblNoIntDato" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblColonia" runat="server" SkinID="Titulo2" Text="Colonia"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblColoniaDato" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblDelegacion" runat="server" SkinID="Titulo2" Text="Delegacion"></asp:Label>
                        </td>
                        <td class="TextLeftTop">
                            <asp:Label ID="lblDelegacionDato" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCP" runat="server" SkinID="Titulo2" Text="C.P."></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCPDato" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </div>
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
    <asp:UpdatePanel ID="UpdatePanelBotones" runat="server" RenderMode="Inline" UpdateMode="Always">
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoBotones" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBotones"
                Mensaje="Espere..." />
            <div class="DivPaddingBotton TextLeftTop">
                <asp:Label ID="lblTituloListadoDeclaraciones" SkinID="Titulo2" runat="server" Text="Declaraciones"></asp:Label>
                <asp:ImageButton ID="btnNuevo" runat="server" ImageUrl="~/Images/plus.gif" AlternateText="Nueva declaración"
                    CausesValidation="false" />
                &nbsp;
                <asp:ImageButton ID="btnModificar" runat="server" ImageUrl="~/Images/edit.gif" OnClick="btnModificar_Click"
                    AlternateText="Editar declaración" />
                &nbsp;
                <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/trash.gif" AlternateText="Eliminar declaración" />
                &nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="btnVer" runat="server" ImageUrl="~/Images/zoom-in.gif" OnClick="btnVer_Click"
                    AlternateText="Visualizar Declaración" />&nbsp;
                <asp:ImageButton ID="btnGenerarAcuse" runat="server" ImageUrl="~/Images/two-docs.gif"
                    AlternateText="Acuse de la declaración" OnClick="btnGenerarAcuse_Click" />
                &nbsp;
                <asp:ImageButton ID="btnGenerarJustificantes" runat="server" ImageUrl="~/Images/two-docs.gif"
                    AlternateText="Justificantes de la declaración" OnClick="btnGenerarJustificantes_Click" />
                &nbsp;
                <asp:ImageButton ID="btnComplementaria" runat="server" AlternateText="Nueva declaración complementaria"
                    ImageUrl="~/Images/clipboard.gif" OnClick="btnComplementaria_Click" />
                <cc1:ModalPopupExtender ID="btnNuevo_ModalPopupExtender" runat="server" Enabled="True"
                    TargetControlID="btnNuevo" BackgroundCssClass="PanelModalBackground " PopupControlID="PnlModalNuevo"
                    CancelControlID="btnModalCancelarNueva">
                </cc1:ModalPopupExtender>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderEliminar" runat="server" Enabled="True"
                    PopupControlID="PnlModalEliminar" TargetControlID="btnEliminar" BackgroundCssClass="PanelModalBackground "
                    DropShadow="True">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="PnlModalNuevo" runat="server" Style="width: 300px; display: none"
                    SkinID="Modal">
                    <table class="TablaCaja">
                        <tr class="TablaCabeceraCaja">
                            <td colspan="2">
                                <div style="float: left">
                                    <asp:Label ID="lblTextoTitulo" SkinID="None" runat="server" Text="Tipo Declaracion" />
                                </div>
                                <div style="float: right">
                                    <asp:ImageButton ID="btnModalNuevaX" runat="server" ImageUrl="~/Images/x.gif" SkinID="BotonBarraCerrar" />
                                </div>
                            </td>
                        </tr>
                        <tr class="TablaCeldaCaja">
                            <td colspan="2">
                                <asp:RadioButton ID="RBNormal" runat="server" AutoPostBack="false" Text="Normal"
                                    Checked="true" GroupName="TipoDec" />
                            </td>
                        </tr>
                        <tr class="TablaCeldaCaja">
                            <td colspan="2">
                                <asp:RadioButton ID="RBAnticipada" runat="server" AutoPostBack="false" Text="Anticipada"
                                    GroupName="TipoDec" />
                            </td>
                        </tr>
                        <tr class="TablaCeldaCaja">
                            <td style="width: 50%" class="TextCenterMiddle ">
                                <asp:UpdatePanel runat="server" ID="UpdatePanelConfirmarNueva" RenderMode="Inline"
                                    UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <uc3:Progreso runat="server" AssociatedUpdatePanelID="UpdatePanelConfirmarNueva"
                                            DisplayAfter="0" DynamicLayout="false" Mensaje="Nueva declaración..." />
                                        <asp:Button ID="btnModalConfirmarNueva" runat="server" OnClick="btnModalConfirmarNueva_Click"
                                            Text="Nueva" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 50%" class="TextCenterMiddle ">
                                <asp:Button ID="btnModalCancelarNueva" runat="server" Text="Cancelar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PnlModalEliminar" runat="server" Style="width: 300px; display: none"
                    SkinID="Modal">
                    <uc4:ModalConfirmarcion ID="ModalConfirmarcionEliminar" runat="server" TextoConfirmacion="¿Esta seguro de borrar la declaración seleccionada?"
                        TextoTitulo="Confirmación de borrado" TextoLinkConfirmacion="Borrar" VisibleTitulo="True"
                        OnConfirmClick="ModalConfirmarcionEliminar_OnConfirmClick" />
                </asp:Panel>
                <asp:HiddenField ID="HiddenBtnGenerarJustificantes" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoGrid" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelGrid" />
            <div class="DivPaddingBotton TextLeftTop">
                <asp:GridView ID="gridViewDeclaraciones" runat="server" AllowPaging="True" AllowSorting="True"
                    PagerSettings-Mode="Numeric" AutoGenerateColumns="False" CssClass="Grid TextLeftTop"
                    GridLines="Horizontal" OnRowDataBound="gridViewDeclaraciones_RowDataBound" OnSelectedIndexChanged="gridViewDeclaraciones_SelectedIndexChanged"
                    DataKeyNames="IDDECLARACION,CODESTADODECLARACION" EmptyDataText="Avalúo sin declaraciones"
                    OnPageIndexChanging="gridViewDeclaraciones_PageIndexChanging" OnSorting="gridViewDeclaraciones_Sorting"
                    OnPreRender="gridViewDeclaraciones_PreRender">
                    <FooterStyle CssClass="GridFooter Titulo2" />
                    <RowStyle CssClass="GridRow TextoNormal" />
                    <EmptyDataRowStyle CssClass="GridEmptyDataRow TextoNormal" />
                    <Columns>
                        <asp:BoundField HeaderText="Nº Dec." DataField="NUMPRESENTACION" SortExpression="NUMPRESENTACION">
                            <ItemStyle Width="80" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" SortExpression="TIPO">
                            <ItemStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Estado" DataField="ESTADO" SortExpression="ESTADO">
                            <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SUJETO" HeaderText="Sujeto" SortExpression="SUJETO" Visible="false">
                            <ItemStyle Width="110px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="%" SortExpression="PORCACTOTOTAL" DataField="PORCACTOTOTAL" DataFormatString="{0:p}" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Right"  Width="52px"/>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="F. Pres." DataField="FECHAPRESENTACION" DataFormatString="{0:dd-MM-yy}"
                            SortExpression="FECHAPRESENTACION" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="65px" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="F.LC." DataField="FECHALINEACAPTURA" DataFormatString="{0:dd-MM-yy}"
                            SortExpression="FECHALINEACAPTURA" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="60px" HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHAPAGO" DataFormatString="{0:dd-MM-yy}" HeaderText="F. Pago"
                            SortExpression="FECHAPAGO" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="60px"  HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="LC" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxLC" />
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pag." Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxPAG" />
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Siscor" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxSISCOR" Enabled="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                    Text="Sel" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="GridPager TextoNormal" />
                    <SelectedRowStyle CssClass="GridRowSelected TextoNormal" />
                    <HeaderStyle CssClass="GridHeader Titulo2" />
                    <EditRowStyle CssClass="GridEditRow  TextoNormal" />
                    <AlternatingRowStyle CssClass="GridAlternatingRow TextoNormal" />
                </asp:GridView>
                <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenIdAvaluo" runat="server" />
                <asp:ObjectDataSource ID="odsAvaluo" runat="server" EnablePaging="True" MaximumRowsParameterName="pageSize"
                    OldValuesParameterFormatString="original_{0}" SelectCountMethod="NumTotalObtenerDeclaracionesDeAvaluo"
                    SelectMethod="ObtenerDeclaracionesDeAvaluo" SortParameterName="SortExpression2"
                    StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenIdAvaluo" Name="idAvaluo" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" DefaultValue="" Name="idNotario"
                            PropertyName="Value" Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="odsAvaluoSF" runat="server" EnablePaging="True" MaximumRowsParameterName="pageSize"
                    OldValuesParameterFormatString="original_{0}" SelectCountMethod="NumTotalObtenerDeclaracionesDeAvaluoSF"
                    SelectMethod="ObtenerDeclaracionesDeAvaluoSF" SortParameterName="sortExpression2"
                    StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenIdAvaluo" Name="idAvaluo" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" DefaultValue="" Name="idNotario"
                            PropertyName="Value" Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="HiddenIdPersonaToken" runat="server" />
            </div>
            <br />
            <div class="TextRigthMiddle">
                <asp:Button ID="ButtonVolver" runat="server" OnClick="ButtonVolver_Click" Text="Bandeja de entrada" />
            </div>
            <br>
            <br></br>
            <br></br>
            </br>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="ContentMenuLateral" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal"
    runat="Server">
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
