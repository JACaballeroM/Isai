<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="BandejaJornada.aspx.cs" Inherits="BandejaJornada" Title="ISAI - Bandeja de Jornada Notarial" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc4" %>
<%@ Register Src="UserControlsCommon/ModalConfirmarcion.ascx" TagName="ModalConfirmacion"
    TagPrefix="uc5" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc6" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<asp:Content ID="ContentDHead" ContentPlaceHolderID="ContentPlaceHolderDHead" runat="Server">
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
        function customValidateFecha(source, arguments) {
            arguments.IsValid = rangoFechasMaxAno('<%= txtFechaIni.ClientID %>', '<%= txtFechaFin.ClientID %>', '<%=System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern %>');
        }

        function rellenarHid() {
            var txtSujetoVal = document.getElementById("<%=txtSujeto.ClientID%>").value;
            var txtSujeto0Val = document.getElementById("<%=txtSujeto0.ClientID%>").value;
            var hiddenSuj = document.getElementById("<%=HiddenSuj.ClientID%>");
            hiddenSuj.value = txtSujetoVal + txtSujeto0Val;
        }
    </script>
</asp:Content>
<asp:Content ID="ContentImage" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="ContentRuta" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <uc4:Navegacion ID="NavegacionRuta" runat="server" />
</asp:Content>
<asp:Content ID="ContentContenido" ContentPlaceHolderID="ContentPlaceHolderDContenido"
    runat="Server">
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
    <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <div class="DivPaddingBotton TextLeftMiddle">
                <fieldset class="formulario">
                    <legend class="formulario">Filtro declaraciones Jornada Notarial</legend>
                    <asp:Panel ID="PanelFiltroJornadaNotarial" runat="server">
                        <table class="TextLeftMiddle">
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:ValidationSummary ID="vsFiltroJornada" runat="server" ValidationGroup="FiltroJornada" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbFechas" runat="server" GroupName="rbBusquedaGroup" Text="Rango de fechas"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" Checked="True" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaIni" PopupButtonID="btnFechaIni">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaIni" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                    <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ErrorMessage="Requerida una fecha"
                                        ValidationGroup="FiltroJornada" ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                                            ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea" ValidationGroup="FiltroJornada"
                                            ControlToValidate="txtFechaIni" ForeColor="Blue" Operator="DataTypeCheck" Type="Date"
                                            Display="Dynamic">!</asp:CompareValidator>
                                    <span class="TextoNormal">-</span>&nbsp;<asp:TextBox ID="txtFechaFin" runat="server"
                                        SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaFin" PopupButtonID="btnFechaFin">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaFin" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ErrorMessage="Requerida una fecha"
                                        ValidationGroup="FiltroJornada" ControlToValidate="txtFechaFin" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                                            ID="cvFechaFin" runat="server" ErrorMessage="Fecha errónea" ValidationGroup="FiltroJornada"
                                            ControlToValidate="txtFechaFin" ForeColor="Blue" Operator="DataTypeCheck" Type="Date"
                                            Display="Dynamic">!</asp:CompareValidator><asp:CompareValidator ID="cvRangoFechas"
                                                runat="server" ErrorMessage="Rango entre fechas erróneo" ValidationGroup="FiltroJornada"
                                                ControlToCompare="txtFechaFin" ControlToValidate="txtFechaIni" ForeColor="Blue"
                                                Operator="LessThan" Type="Date" Display="Dynamic">!</asp:CompareValidator>
                                </td>
                                <td class="TextRigthBottom" rowspan="3" style="width: 50px">
                                    <asp:UpdatePanel ID="UpdatePanelBuscar" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc3:Progreso ID="ProgresoBuscar" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBuscar"
                                                Mensaje="Buscando jornadas..." />
                                            <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Images/search.gif" OnClick="btnBuscar_Click"
                                                ValidationGroup="FiltroJornada" />&nbsp;
                                            <asp:ImageButton ID="btnEliminarBusqueda" runat="server" ImageUrl="~/Images/trash.gif"
                                                CausesValidation="False" AlternateText="Eliminar filtro busqueda" Enabled="true"
                                                ToolTip="Eliminar filtro busqueda" OnClick="btnEliminarBusqueda_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbCuenta" runat="server" GroupName="rbBusquedaGroup" Text="Cuenta catastral"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegion" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvRegion" runat="server" ControlToValidate="txtRegion" SetFocusOnError="True"
                                            Enabled="false" ErrorMessage="Requerida una región" ValidationGroup="FiltroJornada"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtManzana" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvManzana" runat="server" ControlToValidate="txtManzana" SetFocusOnError="True"
                                            Enabled="false" ErrorMessage="Requerida una manzana" ValidationGroup="FiltroJornada"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLote" runat="server" Enabled="false" MaxLength="2" Width="20px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,2);}"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvLote" runat="server" ControlToValidate="txtLote" SetFocusOnError="True"
                                            Enabled="false" ErrorMessage="Requerido un lote" ValidationGroup="FiltroJornada"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtUnidadPrivativa" runat="server" Enabled="false" MaxLength="3"
                                        onkeypress="return validaAlfanumerico(event)" Width="30px" SkinID="TextBoxObligatorio"
                                        onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvUnidadPrivativa" runat="server" ControlToValidate="txtUnidadPrivativa"
                                            SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un condominio"
                                            ValidationGroup="FiltroJornada" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtCuenta" runat="server" CssClass="TextBoxNormal" Enabled="False"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <div id="divsuj" class="DivPaddingBotton" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbSujeto" runat="server" GroupName="rbBusquedaGroup" Text="Sujeto (Nombre-Apellidos) "
                                            AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSujeto" runat="server" Enabled="False" MaxLength="100" onblur="javascript:rellenarHid()"></asp:TextBox>
                                        -
                                        <asp:RegularExpressionValidator ID="cvSujeto" runat="server" ControlToValidate="txtSujeto"
                                            CssClass="TextoNormalError" ValidationExpression=".{3,}" Display="Dynamic" SetFocusOnError="True"
                                            ToolTip="Formato incorrecto" ValidationGroup="FiltroJornada" ErrorMessage="El nombre debe tener como mínimo tres carácteres">*</asp:RegularExpressionValidator>
                                        &nbsp;<asp:TextBox ID="txtSujeto0" runat="server" Enabled="False" MaxLength="200"
                                            onblur="javascript:rellenarHid()"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="cvSujeto0" runat="server" ControlToValidate="txtSujeto0"
                                            CssClass="TextoNormalError" ValidationExpression=".{3,}" Display="Dynamic" SetFocusOnError="True"
                                            ToolTip="Formato incorrecto" ValidationGroup="FiltroJornada" ErrorMessage="El apellido debe tener como mínimo tres carácteres">*</asp:RegularExpressionValidator>
                                        <asp:TextBox ID="HiddenSuj" runat="server" Style="display: none;" AutoPostBack="true" />
                                        <asp:RequiredFieldValidator ID="rfvHiddenSuj" runat="server" ControlToValidate="HiddenSuj"
                                            SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido nombre y/o apellido"
                                            ValidationGroup="FiltroJornada" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </div>
                        </table>
                    </asp:Panel>
                </fieldset>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="DivPaddingBotton TextLeftTop">
        <asp:UpdatePanel ID="UpdatePanelBotones" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblTituloListadoDeclaraciones" SkinID="Titulo2" class="TextLeftTop"
                    runat="server" Text="Listado de declaraciones Jornada Notarial"></asp:Label>
                &nbsp;<br />
                <asp:UpdatePanel ID="UpdatePanelNueva" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:Progreso ID="ProgresoNueva" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelNueva"
                            Mensaje="Nueva declaración..." />
                        <asp:ImageButton ID="btnNuevo" runat="server" ImageUrl="~/Images/plus.gif" OnClick="btnNuevo_Click"
                            CausesValidation="False" ToolTip="Nueva declaración" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;
                <asp:UpdatePanel ID="UpdatePanelModificar" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:Progreso ID="ProgresoModificar" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelModificar"
                            Mensaje="Abriendo declaración seleccionada" />
                        <asp:ImageButton ID="btnModificar" runat="server" ImageUrl="~/Images/edit_p.gif"
                            OnClick="btnModificar_Click" CausesValidation="False" ToolTip="Editar declaración" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;
                <asp:UpdatePanel ID="UpdatePanelEliminar" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/trash_p.gif"
                            CausesValidation="False" ToolTip="Eliminar declaración" />
                        <cc1:ModalPopupExtender ID="extenderModalEliminar" runat="server" BackgroundCssClass="PanelModalBackground"
                            Enabled="True" PopupControlID="pnlModalEliminar" TargetControlID="btnEliminar">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlModalEliminar" runat="server" Style="width: 300px; display: none"
                            SkinID="Modal">
                            <uc5:ModalConfirmacion ID="modalConfirmacionEliminar" runat="server" OnConfirmClick="modalConfirmacionEliminar_ConfirmClick"
                                TextoConfirmacion="¿Esta seguro de borrar la declaración seleccionada?" TextoLinkConfirmacion="Borrar"
                                TextoTitulo="Confirmación de borrado" TextoProgreso="Borrando declaración..."
                                VisibleTitulo="true" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;&nbsp;
                <asp:UpdatePanel ID="UpdatePanelVer" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc3:Progreso ID="ProgresoVer" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelVer"
                            Mensaje="Abriendo declaración" />
                        <asp:ImageButton ID="btnVer" runat="server" ImageUrl="~/Images/zoom-in_p.gif" OnClick="btnVer_Click"
                            CausesValidation="False" ToolTip="Visualizar declaración" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                &nbsp;
                <asp:UpdatePanel ID="UpdatePanelJustificante" runat="server" RenderMode="Inline"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton ID="btnGenerarAcuse" runat="server" ImageUrl="~/Images/two-docs_p.gif"
                            CausesValidation="False" ToolTip="Acuse de la declaración" OnClick="btnGenerarAcuse_Click" />&nbsp;
                        <asp:ImageButton ID="btnGenerarJustificantes" runat="server" ImageUrl="~/Images/two-docs_p.gif"
                            CausesValidation="False" ToolTip="Justificantes de la declaración" OnClick="btnGenerarJustificantes_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <uc3:Progreso ID="ProgresoGrid" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelGrid" />
                <asp:HiddenField runat="server" ID="HiddenTokenModal" />
                <asp:GridView ID="gridViewDeclaraciones" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    PagerSettings-Mode="Numeric" GridLines="Horizontal" OnRowDataBound="gridViewDeclaraciones_RowDataBound"
                    DataKeyNames="IDDECLARACION,CODESTADODECLARACION" OnSelectedIndexChanged="gridViewDeclaraciones_SelectedIndexChanged"
                    EmptyDataText="No hay declaraciones para el filtro seleccionado" OnPageIndexChanging="gridViewDeclaraciones_PageIndexChanging"
                    AllowSorting="True" OnSorting="gridViewDeclaraciones_Sorting" PageSize="10" DataSourceID="odsPorFecha">
                    <Columns>
                        <asp:BoundField HeaderText="Nº" DataField="NUMPRESENTACION" SortExpression="NUMPRESENTACION">
                            <ItemStyle Width="80" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Estado" DataField="ESTADO" SortExpression="ESTADO">
                            <ItemStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SUJETO" HeaderText="Sujeto" SortExpression="SUJETO" Visible="false">
                            <ItemStyle Width="125px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PORCACTOTOTAL" HeaderText="%" SortExpression="PARTICIPACION" DataFormatString="{0:p}" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle  HorizontalAlign="Right" Width="52px"/>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="F. Presentación" DataField="FechaPresentacion" DataFormatString="{0:dd-MM-yy}"
                            SortExpression="FechaPresentacion" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle  HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaPago" DataFormatString="{0:dd-MM-yy}" HeaderText="F. Pago"
                            SortExpression="FechaPago" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle  HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="LC" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxLC" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pag." Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxPAG" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Siscor" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px" ItemStyle-Width="25px">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxSISCOR" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                    Text="Sel" Width="20px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="text-align: right">
                    <asp:Label ID="lblCount" runat="server"></asp:Label>
                </div>
                </div>
                <div>
                    <asp:HiddenField ID="HiddenFechaFin" runat="server" />
                    <asp:HiddenField ID="HiddenFechaIni" runat="server" />
                    <asp:HiddenField ID="HiddenCuentaCat" runat="server" />
                    <asp:HiddenField ID="HiddenSujeto" runat="server" />
                    <asp:HiddenField ID="HiddenSujeto0" runat="server" />
                    <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" Value="0" />
                    <asp:HiddenField runat="server" ID="HiddenIdPersonaToken" />
                    <asp:ObjectDataSource ID="odsPorFecha" runat="server" TypeName="DseDeclaracionIsaiPagger"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="ObtenerDecPorFechaJornada"
                        EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="NumtTotalObtenerDecPorFechaJornada"
                        StartRowIndexParameterName="indice" SortParameterName="SortExpression">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="HiddenFechaIni" Name="fechaInicio" PropertyName="Value"
                                Type="String" />
                            <asp:ControlParameter ControlID="HiddenFechaFin" Name="fechaFin" PropertyName="Value"
                                Type="String" />
                            <asp:Parameter Name="pageSize" Type="Int32" />
                            <asp:Parameter Name="indice" Type="Int32" />
                            <asp:Parameter Name="SortExpression" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="odsPorCC" runat="server" TypeName="DseDeclaracionIsaiPagger"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="ObtenerDecPorCCJornada"
                        EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="NumtTotalObtenerDecPorCCJornada"
                        StartRowIndexParameterName="indice" SortParameterName="SortExpression">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idnotario" PropertyName="Value"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="HiddenCuentaCat" Name="cuentaCatastral" PropertyName="Value"
                                Type="Object" />
                            <asp:Parameter Name="pageSize" Type="Int32" />
                            <asp:Parameter Name="indice" Type="Int32" />
                            <asp:Parameter Name="SortExpression" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="odsPorNom" runat="server" TypeName="DseDeclaracionIsaiPagger"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="ObtenerDecPorNomJornada"
                        EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="NumtTotalObtenerDecPorNomJornada"
                        StartRowIndexParameterName="indice" SortParameterName="SortExpression">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idnotario" PropertyName="Value"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="HiddenSujeto" Name="nombre" PropertyName="Value"
                                Type="String" />
                            <asp:ControlParameter ControlID="HiddenSujeto0" Name="apellidoPaterno" PropertyName="Value"
                                Type="String" />
                            <asp:Parameter Name="pageSize" Type="Int32" />
                            <asp:Parameter Name="indice" Type="Int32" />
                            <asp:Parameter Name="SortExpression" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <input type="hidden" id="hidBusquedaActual" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
