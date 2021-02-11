<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalBuscarReducciones.ascx.cs" Inherits="UserControls_ModalBuscarReducciones" EnableTheming="true"%>
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
        <td>
            <div style="float: left">
                <asp:Label ID="lblTituloModalBuscardorReducciones" runat="server" SkinID="None" Text="Buscador de reducciones" />
            </div>
            <div style="float: right">
                <asp:ImageButton ID="btnCerrarBuscadorReducciones" runat="server" SkinID="BotonBarraCerrar"
                    ImageUrl="~/Images/x.gif" OnClick="btnCerrarBuscadorReducciones_Click" />
            </div>
        </td>
    </tr>
    <tr class="TablaCeldaCaja">
        <td>
            <%--<asp:Label ID="lblTituloFiltro" runat="server" Text="Filtro de reducciones" SkinID="Titulo2"></asp:Label>--%>
             <fieldset class="formulario">
            <legend class="formulario">Filtro de reducciones</legend>
            <asp:Panel ID="pnlBucarReduccionesModalFiltros" runat="server">
                <asp:UpdatePanel ID="updatePanelBuscadorReducciones" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:Progreso ID="ProgresoupdatePanelBuscadorReducciones" runat="server" AssociatedUpdatePanelID="updatePanelBuscadorReducciones"
                            DisplayAfter="0" DynamicLayout="false" Mensaje="Buscando reducciones..." />
                        <table style="width: 100%">
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:ValidationSummary ID="vsFiltroArticulo" runat="server" ValidationGroup="FiltroArticulo" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblBuscarReduccionesModalArticulo" runat="server" Text="Artículo" SkinID="Titulo2"></asp:Label>
                                </td>
                                <td style="width: 70%">
                                    <asp:TextBox Style="width: 95%" ID="txtBuscarReduccionesModalArticulo" runat="server">
                                    </asp:TextBox><asp:CompareValidator
                                        ID="cvArticulo" runat="server" ErrorMessage="Artículo erróneo" 
                                    ForeColor="Blue" Type="Integer" ValidationGroup="FiltroArticulo" Operator="DataTypeCheck"
                                        ControlToValidate="txtBuscarReduccionesModalArticulo" Display="Dynamic">!</asp:CompareValidator>
                                </td>
                                <td class="TextRigthBottom" rowspan="6" style="width: 10%">
                                    <asp:ImageButton ID="btnBuscarReduccionesModalFiltrar" runat="server" ImageAlign="Right"
                                        ImageUrl="~/Images/search.gif" OnClick="btnBuscarReduccionesModalFiltrar_Click" ValidationGroup="FiltroArticulo" CausesValidation="true"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBuscarReduccionesModalDescripcion" runat="server" Text="Descripción"
                                        SkinID="Titulo2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox Style="width: 95%" ID="txtBuscarReduccionesModalDescripción" runat="server">
                                    </asp:TextBox>
                                </td>
                            </tr>    
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            </fieldset>
         <%--   <asp:Label ID="lblBuscarReduccionesModalFiltroResultado" runat="server" SkinID="Titulo2"
                class="TextLeftTop" Text="Resultado"></asp:Label>--%>
                  <fieldset class="formulario">
            <legend class="formulario">Resultado</legend>
            <asp:Panel ID="Panel1" runat="server">
                <asp:UpdatePanel ID="updatePanelGridReducciones" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:Progreso ID="ProgresoUpadatePanelReducciones" runat="server" AssociatedUpdatePanelID="updatePanelGridReducciones"
                            DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
                        <asp:ObjectDataSource ID="odsReduccionesPorArticuloDescripcion" runat="server" 
                            EnablePaging="True" MaximumRowsParameterName="pageSize" 
                            SelectCountMethod="NumTotalFilasBuscarReduccionesPorArticuloDescripcion" 
                            SelectMethod="ObtenerBusquedaReduccionesPorArticuloDescripcion" 
                            SortParameterName="SortExpression" StartRowIndexParameterName="indice" 
                            TypeName="DseReduccionesPagger" 
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="HiddenAnio" Name="anio" PropertyName="Value" 
                                    Type="Int32" />
                                <asp:ControlParameter ControlID="txtBuscarReduccionesModalArticulo" 
                                    Name="articulo" PropertyName="Text" Type="String" />
                                <asp:ControlParameter ControlID="txtBuscarReduccionesModalDescripción" 
                                    Name="descripcion" PropertyName="Text" Type="String" />
                                <asp:Parameter Name="pageSize" Type="Int32" />
                                <asp:Parameter Name="indice" Type="Int32" />
                                <asp:Parameter Name="SortExpression" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>   
                        <asp:HiddenField ID="HiddenAnio" runat="server" />
                        <asp:GridView ID="gridViewReducciones" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="false" PageSize="5" PagerSettings-Mode="Numeric" 
                            GridLines="Horizontal" OnSelectedIndexChanged="gridViewReducciones_SelectedIndexChanged"
                            DataKeyNames="IDREDUCCION,DESCRIPCION,DESCUENTO,ARTICULO,APARTADO,CODEJERCICIO"
                            OnPageIndexChanging="gridViewReducciones_PageIndexChanging" AllowSorting="True"
                            OnSorting="gridViewReducciones_Sorting" OnRowDataBound="gridViewReducciones_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ARTICULO" HeaderText="Artículo" SortExpression="ARTICULO">
                                    <ItemStyle Width="30px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" SortExpression="DESCRIPCION">
                                    <ItemStyle Width="100%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BENEFICIOS" HeaderText="Beneficio" SortExpression="DESCUENTO">
                                    <ItemStyle Width="30px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="APARTADO" HeaderText="Apartado" SortExpression="APARTADO">
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
                    <uc1:Progreso ID="Progreso" runat="server" AssociatedUpdatePanelID="updatePanelBotones" DisplayAfter="0" DynamicLayout="false" Mensaje="Cargando datos..." />
            <div style="display: inline; float: right">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" CausesValidation="false" />
                &nbsp;&nbsp;
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false"/>
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
