<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="NumeroPresentacion.aspx.cs" Inherits="NumeroPresentacion" Title="ISAI - Obtener Número de Presentación" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc4" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc5" %>
<%@ Register Src="UserControls/ModalBuscarPeritos.ascx" TagName="ModalBuscarPeritos"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <uc4:Navegacion ID="Navegacion1" runat="server" />
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
            <cc1:ModalPopupExtender ID="mpeErrorTareas" runat="server" Enabled="True" TargetControlID="hlnErrorTareas"
                PopupControlID="panErrorTareas" DropShadow="false" BackgroundCssClass="PanelModalBackground" />
            <asp:HyperLink ID="hlnErrorTareas" runat="server" Style="display: none" />
            <asp:Panel ID="panErrorTareas" runat="server" Style="display: none" SkinID="Modal">
                <uc11:ModalInfoExcepcion ID="errorTareas" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelNumPresentacion" runat="server" RenderMode="Inline"
        UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtNumeroDeclaracion" />
        </Triggers>
        <ContentTemplate>
            <div class="DivPaddingBotton" id="DivAvaluo" runat="server">
                <fieldset class="formulario">
                    <legend class="formulario">Generar número presentación</legend>
                    <asp:Panel ID="panelNumPresentacion" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:ValidationSummary ID="vsFiltroAvaluos" runat="server" ValidationGroup="FiltroAvaluos" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <asp:Label ID="lblTipoDeclaracion" runat="server" SkinID="Titulo2" Text="Tipo Declaración"></asp:Label>
                                </td>
                                <td class="style4">
                                    <asp:DropDownList ID="ddlTipoDeclaracion" runat="server" CssClass="TextBoxNormal"
                                        OnSelectedIndexChanged="ddlTipoDeclaracion_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <asp:Label ID="lblCuentaCatastral" runat="server" SkinID="Titulo2" Text="Cuenta Catastral"></asp:Label>
                                </td>
                                <td class="style4">
                                    <asp:TextBox ID="txtRegion" runat="server" Enabled="true" MaxLength="3" Width="30px"
                                        SkinID="TextBoxObligatorio" onblur="rellenar(this,this.value,3);" onkeypress="return validaAlfanumerico(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ControlToValidate="txtRegion"
                                        SetFocusOnError="True" Enabled="true" ErrorMessage="Requerida una región" ValidationGroup="FiltroAvaluos"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtManzana" runat="server" Enabled="true" MaxLength="3" Width="30px"
                                        SkinID="TextBoxObligatorio" onblur="rellenar(this,this.value,3);" onkeypress="return validaAlfanumerico(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvManzana" runat="server" ControlToValidate="txtManzana"
                                        SetFocusOnError="True" Enabled="true" ErrorMessage="Requerida una manzana" ValidationGroup="FiltroAvaluos"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLote" runat="server" Enabled="true" MaxLength="2" Width="20px"
                                        onblur="javascript:if(this.value.length!=0){rellenar(this,this.value,2);}" onkeypress="return validaAlfanumerico(event)"></asp:TextBox>
                                    <asp:TextBox ID="txtUnidadPrivativa" runat="server" Enabled="true" MaxLength="3"
                                        Width="30px" onblur="javascript:if(this.value.length!=0){rellenar(this,this.value,3);}"
                                        onkeypress="return validaAlfanumerico(event)"></asp:TextBox>
                                    <asp:TextBox ID="txtCuenta" runat="server" Enabled="False" Visible="false" SkinID="TextBoxObligatorio"></asp:TextBox>
                                </td>
                                <td align="right">
                                 <asp:UpdatePanel ID="UpdatePanelBuscar" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                 <uc3:Progreso ID="Progreso1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBuscar" />
                                    <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" OnClick="btnBuscar_Click"
                                        ImageUrl="~/Images/search.gif" ValidationGroup="FiltroAvaluos" />
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <asp:Label ID="lblNumRegSoci" runat="server" SkinID="Titulo2" Text="Nº Registro del perito o sociedad"></asp:Label>
                                </td>
                                <td class="style4">
                                    <asp:UpdatePanel ID="UpdatePanelPeritos" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtNumRegSoci" runat="server" Enabled="true"></asp:TextBox>
                                            <asp:CompareValidator ID="cvNumeroPeritos" runat="server" ControlToValidate="txtNumRegSoci"
                                                ErrorMessage="Número de registro del perito erróneo" ForeColor="Blue" Operator="DataTypeCheck"
                                                SetFocusOnError="True" ValidationGroup="FiltroAvaluos">!</asp:CompareValidator>
                                            <asp:ImageButton ID="btnPeritos" runat="server" CausesValidation="False" ImageUrl="~/Images/user.gif"
                                                OnClick="btnPeritos_Click" ToolTip="Buscar Peritos" />
                                            <cc1:ModalPopupExtender ID="btnPeritos_ModalPopupExtender" runat="server" DynamicServicePath=""
                                                Enabled="True" TargetControlID="btnPeritoHidden" PopupControlID="PnlModalBuscarPerito"
                                                BackgroundCssClass="PanelModalBackground" DropShadow="true">
                                            </cc1:ModalPopupExtender>
                                            <asp:HiddenField runat="server" ID="btnPeritoHidden" />
                                            <asp:Panel runat="server" ID="PnlModalBuscarPerito" Style="width: 700px; display: none"
                                                SkinID="Modal">
                                                <uc6:ModalBuscarPeritos ID="ModalBuscarPeritos1" runat="server" OnConfirmClick="buscarPerito_ConfirmClick" />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                    <asp:Label ID="lblNumeroDeclaracion" runat="server" SkinID="Titulo2" Text="Número Declaración"></asp:Label>
                                </td>
                                <td class="style4">
                                    <asp:TextBox ID="txtNumeroDeclaracion" runat="server" Enabled="false" SkinID="TextBoxObligatorio"
                                        MaxLength="15"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNumeroDeclaracion"
                                            runat="server" ControlToValidate="txtNumeroDeclaracion" SetFocusOnError="True"
                                            Enabled="false" ErrorMessage="Requerido un número de declaración" ValidationGroup="FiltroAvaluos"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revNumPres" runat="server" ErrorMessage="Formato nº declaración erroneo. El nº  debe tener el formato I-aaaa-zzzzzz (Ej: I-2010-777)"
                                        ControlToValidate="txtNumeroDeclaracion" Display="Dynamic" Enabled="false" ValidationExpression="^I-[0-9]{4}-[0-9]{1,9}(\s)*$"
                                        SetFocusOnError="True" ValidationGroup="FiltroAvaluos" ForeColor="Blue">!</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">
                                </td>
                                <td class="style4">
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanelGenerar" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                            <uc3:Progreso ID="ProgresoGenerar" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelGenerar" Mensaje="Generando número de presentación..."/>
                                            <asp:Button ID="btnGenerar" runat="server" Text="Generar" ValidationGroup="FiltroAvaluos"
                                                OnClick="btnGenerar_Click" Enabled="false" />
                                            <cc1:ModalPopupExtender ID="extenderPnlInfoGenerarModal" runat="server" Enabled="True"
                                                TargetControlID="hiddenPnlInfoGenerarModal" PopupControlID="pnlInfoGenerarModal"
                                                BackgroundCssClass="PanelModalBackground" DropShadow="True" />
                                            <asp:Panel ID="pnlInfoGenerarModal" SkinID="Modal" Style="width: 280px; display: none;"
                                                runat="server">
                                                <uc5:ModalInfo ID="ModalInfoGenerar" runat="server" OnConfirmClick="ModalInfoGenerar_Ok_Click" />
                                            </asp:Panel>
                                            <asp:HiddenField runat="server" ID="hiddenPnlInfoGenerarModal" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </div>
            <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <ContentTemplate>

            
                    <asp:HiddenField runat="server" ID="HiddenIdPersonaToken" />
                    <asp:HiddenField runat="server" ID="HiddenTokenModal" />
                    <cc1:ModalPopupExtender ID="extenderPnlInfoTokenModal" runat="server" Enabled="True"
                        TargetControlID="HiddenTokenModal" PopupControlID="pnlTokenModal" BackgroundCssClass="PanelModalBackground"
                        DropShadow="True" />
                    <asp:Panel ID="pnlTokenModal" SkinID="Modal" Style="width: 280px; display: none;"
                        runat="server">
                        <uc5:ModalInfo ID="ModalInfoToken" runat="server" OnConfirmClick="ModalInfoToken_Ok_Click" />
                    </asp:Panel>
                    <asp:ObjectDataSource ID="odsPorCuentaCatastral" runat="server" SelectMethod="ObtenerAvaluosPorCuentaObtnNumNotario"
                        TypeName="DseDeclaracionIsaiPagger" EnablePaging="True" MaximumRowsParameterName="pageSize"
                        SelectCountMethod="NumtTotalObtenerAvaluosPorCuentaObtnNumNotario" StartRowIndexParameterName="indice"
                        SortParameterName="SortExpression" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtCuenta" Name="cuentaCatastral" PropertyName="Text"
                                Type="String" />
                            <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="txtNumRegSoci" Name="registro" PropertyName="Text"
                                Type="String" />
                            <asp:Parameter Name="vigente" Type="String" DefaultValue="T" />
                            <asp:Parameter Name="pageSize" Type="Int32" />
                            <asp:Parameter Name="indice" Type="Int32" />
                            <asp:Parameter Name="SortExpression" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <div class="DivPaddingBotton TextLeftTop">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblTituloListado" SkinID="Titulo2" class="TextLeftTop" runat="server"
                                        Text="Listado de avalúos"></asp:Label>
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gridViewAvaluos" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="Horizontal" EmptyDataText="No hay avaluos para la cuenta catastral introducida"
                            DataKeyNames="IDAVALUO,REGION,MANZANA,LOTE,UNIDADPRIVATIVA" OnSelectedIndexChanged="gridViewAvaluos_SelectedIndexChanged"
                            OnSorting="gridViewAvaluos_Sorting" OnRowDataBound="gridViewAvaluos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="IDAVALUO" DataField="IDAVALUO" Visible="False"></asp:BoundField>
                                <asp:BoundField HeaderText="Nº Único" DataField="NUMEROUNICO" SortExpression="NUMEROUNICO"
                                    ItemStyle-Width="100">
                                    <ItemStyle Width="125px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Cuenta cat." DataField="CUENTACATASTRAL" SortExpression="CUENTACATASTRAL" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField HeaderText="Fecha pres." DataField="FECHA_DOCDIGITAL" DataFormatString="{0:dd-MM-yy}"
                                    SortExpression="FECHA_DOCDIGITAL" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="FECHAVALORREFERIDO" HeaderText="FECHAVALORREFERIDO" Visible="False" />
                                <asp:BoundField DataField="CODESTADOAVALUO" HeaderText="CODESTADOAVALUO" Visible="False" />
                                <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" />
                                <asp:BoundField DataField="REGISTRO_PERITO" HeaderText="Perito" SortExpression="REGISTRO_PERITO"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REGISTRO_SOCIEDAD" HeaderText="Sociedad" SortExpression="REGISTRO_Sociedad"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NOMBRE_NOTARIO" HeaderText="NOMBRE_NOTARIO" Visible="False" />
                                <asp:BoundField DataField="VIGENTE" HeaderText="VIGENTE" Visible="False" />
                                <asp:BoundField DataField="CODTIPOTRAMITE" HeaderText="CODTIPOTRAMITE" Visible="False" />
                                <asp:BoundField DataField="TIPO" HeaderText="TIPO" SortExpression="TIPO" Visible="False" />
                                <asp:BoundField DataField="REGION" HeaderText="REGION" SortExpression="REGION" Visible="False" />
                                <asp:BoundField DataField="MANZANA" HeaderText="MANZANA" SortExpression="MANZANA"
                                    Visible="False" />
                                <asp:BoundField DataField="LOTE" HeaderText="LOTE" SortExpression="LOTE" Visible="False" />
                                <asp:BoundField DataField="UNIDADPRIVATIVA" HeaderText="UNIDADPRIVATIVA" Visible="False" />
                                <asp:BoundField DataField="VALORCOMERCIAL" HeaderText="VALORCOMERCIAL" Visible="False" />
                                <asp:BoundField DataField="VALORCATASTRAL" HeaderText="VALORCATASTRAL" Visible="False" />
                                <asp:BoundField DataField="VALORREFERIDO" HeaderText="VALORREFERIDO" Visible="False" />
                                <asp:BoundField DataField="OBJETO" HeaderText="OBJETO" Visible="False" />
                                <asp:BoundField DataField="PROPOSITO" HeaderText="PROPOSITO" Visible="False" />
                                <asp:BoundField DataField="IDPERSONAPERITO" HeaderText="IDPERSONAPERITO" Visible="False" />
                                <asp:BoundField DataField="IDPERSONANOTARIO" HeaderText="IDPERSONANOTARIO" Visible="False" />
                                <asp:BoundField DataField="ROWS_TOTAL" HeaderText="ROWS_TOTAL" Visible="False" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                            Text="Sel" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div style="text-align: right">
                        <asp:Label ID="lblCount" runat="server"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="ContentPlaceHolderDHead">
    <style type="text/css">
        .style4
        {
            width: 261px;
        }
        .style5
        {
            width: 288px;
        }
    </style>
</asp:Content>
