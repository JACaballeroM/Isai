<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalBuscarExenciones.ascx.cs"
    Inherits="UserControls_ModalBuscarExenciones" %>
<%@ Register Src="../UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion" TagPrefix="uc11" %>
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
<table class="TablaCaja">
    <tr class="TablaCabeceraCaja">
        <td colspan="2">
            <div style="float: left">
                <asp:Label ID="lblTextoTitulo" runat="server" SkinID="None" Text="Buscador de exenciones" />
            </div>
            <div style="float: right;">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/x.gif" ImageAlign="NotSet"
                    OnClick="btnCerrarBuscadorExenciones_Click" /></div>
        </td>
    </tr>
    <tr class="TablaCeldaCaja">
        <td colspan="2">
            <%--<asp:Label ID="lblTituloFiltro" runat="server" Text="Filtro de exenciones" SkinID="Titulo2"></asp:Label>--%>
            <fieldset class="formulario">
            <legend class="formulario">Filtro de exenciones</legend>
            <asp:Panel ID="pnlFiltro" runat="server" >
                <asp:UpdatePanel ID="updatePanelBuscadorExenciones" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:Progreso ID="ProgresoupdatePanelBuscadorExenciones" runat="server" AssociatedUpdatePanelID="updatePanelBuscadorExenciones"
                            DisplayAfter="0" DynamicLayout="false" Mensaje="Buscando exenciones..." />
                        <table style="width: 99%">
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:ValidationSummary ID="vsFiltroArticulo" runat="server" ValidationGroup="FiltroArticulo" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">
                                    <asp:Label ID="lblArticulo" runat="server" Text="Artículo" SkinID="Titulo2"></asp:Label>
                                </td>
                                <td style="width: 85%">
                                    <asp:TextBox Style="width: 99%" ID="txtArticulo" runat="server">
                                    </asp:TextBox><asp:CompareValidator
                                        ID="cvArticulo" runat="server" ErrorMessage="Artículo erróneo" 
                                    ForeColor="Blue" Type="Integer" ValidationGroup="FiltroArticulo" Operator="DataTypeCheck"
                                        ControlToValidate="txtArticulo" Display="Dynamic">!</asp:CompareValidator>
                                </td>
                                <td rowspan="2" style="width: 5%" class="TextRigthBottom">
                                    <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Images/search.gif" OnClick="btnBuscarExencionesModalFiltrar_Click"
                                    ValidationGroup="FiltroArticulo" CausesValidation="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripción" SkinID="Titulo2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox Style="width: 99%" ID="txtDescripcion" runat="server">
                                    </asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            </fieldset>
        <%--    <asp:Label ID="lblTituloResultado" runat="server" SkinID="Titulo2" class="TextLeftTop"
                Text="Resultado"></asp:Label>--%>
                 <fieldset class="formulario">
            <legend class="formulario">Resultado</legend>
            <asp:Panel ID="pnlResultado" runat="server">
                <asp:UpdatePanel ID="updatePanelGridExenciones" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:Progreso ID="ProgresoUpadatePanelExenciones" runat="server" AssociatedUpdatePanelID="updatePanelGridExenciones"
                            DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
                        <asp:ObjectDataSource ID="odsExencionesPorArticuloDescripcion" runat="server" SelectMethod="ObtenerBusquedaExencionesPorArticuloDescripcion"
                            TypeName="DseExencionesPagger" EnablePaging="True" MaximumRowsParameterName="pageSize"
                            SelectCountMethod="NumTotalFilasBuscarExencionesPorArticuloDescripcion" SortParameterName="SortExpression"
                            StartRowIndexParameterName="indice" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="HiddenAnio" Name="anio" PropertyName="Value" Type="Int32" />
                                <asp:ControlParameter ControlID="txtArticulo" Name="articulo" PropertyName="Text"
                                    Type="String" />
                                <asp:ControlParameter ControlID="txtDescripcion" Name="descripcion" PropertyName="Text"
                                    Type="String" />
                                <asp:Parameter Name="pageSize" Type="Int32" />
                                <asp:Parameter Name="indice" Type="Int32" />
                                <asp:Parameter Name="SortExpression" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="HiddenAnio" runat="server" />
                        <br />
                        <asp:GridView ID="gridViewExenciones" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            PagerSettings-Mode="Numeric" GridLines="Horizontal" OnSelectedIndexChanged="gridViewExenciones_SelectedIndexChanged"
                            DataKeyNames="IDEXENCION,DESCRIPCION,ARTICULO,FRACCION,CODEJERCICIO" OnPageIndexChanging="gridViewExenciones_PageIndexChanging"
                            AllowSorting="True" OnSorting="gridViewExenciones_Sorting" PageSize="5" 
                            onrowdatabound="gridViewExenciones_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ARTICULO" HeaderText="Artículo" SortExpression="ARTICULO">
                                    <ItemStyle Width="30px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION">
                                    <ItemStyle Width="100%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FRACCION" HeaderText="Fracción" SortExpression="FRACCION">
                                    <ItemStyle Width="30px" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                                            CommandName="Select" Text="Sel" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            </fieldset>
        </td>
    </tr>
    <tr class="TablaCeldaCaja">
        <td>
          <asp:UpdatePanel ID="updatePanelBotones" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
            <div style="display: inline; float: right">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" CausesValidation="false" />
                &nbsp;&nbsp;
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>
            </ContentTemplate></asp:UpdatePanel>
        </td>
    </tr>    
    
</table>
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
