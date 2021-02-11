<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="BandejaEntradaSF.aspx.cs" Inherits="BandejaEntradaSF" Title="ISAI - Bandeja de entrada (SF)" %>

<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc4" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc5" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<%@ Register Src="UserControls/ModalBuscarNotarios.ascx" TagName="ModalBuscarNotarios"
    TagPrefix="uc2" %>
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

        function rellenar(quien, que, cuanto) {
            cadcero = '';
            for (i = 0; i < (cuanto - que.length); i++) {
                cadcero += '0';
            }
            quien.value = cadcero + que;
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderDImagen" runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <uc4:Navegacion ID="Navegacion1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderDContenido" runat="Server">
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
    <asp:UpdatePanel ID="UpdatePanelBusqueda" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="DivPaddingBotton">
                <fieldset class="formulario">
                    <legend class="formulario">Filtro declaraciones</legend>
                    <asp:Panel ID="PanelFiltroJornadaNotarial" runat="server">
                        <table class="TextLeftMiddle" style="width: 100%;">
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:ValidationSummary ID="vsFiltroSF" runat="server" ValidationGroup="FiltroSF" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 35%;">
                                    <asp:RadioButton ID="rbFechas" runat="server" GroupName="rbBusquedaGroup" Text="Rango de fechas"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" Checked="True" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" MaxLength="10">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaIni" PopupButtonID="btnFechaIni">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaIni" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" /><asp:RequiredFieldValidator
                                            ID="rfvFechaInicio" runat="server" ErrorMessage="Requerida una fecha" ValidationGroup="FiltroSF"
                                            ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                                                ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea" ValidationGroup="FiltroSF"
                                                ControlToValidate="txtFechaIni" ForeColor="Blue" Operator="DataTypeCheck" Type="Date"
                                                Display="Dynamic">!</asp:CompareValidator>
                                    &nbsp;-&nbsp;<asp:TextBox ID="txtFechaFin" runat="server" SkinID="TextBoxObligatorio"
                                        MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaFin" PopupButtonID="btnFechaFin">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaFin" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" /><asp:RequiredFieldValidator
                                            ID="rfvFechaFin" runat="server" ErrorMessage="Requerida una fecha" ValidationGroup="FiltroSF"
                                            ControlToValidate="txtFechaFin" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvFechaFin" runat="server" ErrorMessage="Fecha errónea"
                                        ValidationGroup="FiltroSF" ControlToValidate="txtFechaFin" ForeColor="Blue" Operator="DataTypeCheck"
                                        Type="Date" Display="Dynamic">!</asp:CompareValidator>
                                    <asp:CompareValidator ID="cvRangoFechas" runat="server" ErrorMessage="Rango entre fechas erróneo"
                                        ValidationGroup="FiltroSF" ControlToCompare="txtFechaFin" ControlToValidate="txtFechaIni"
                                        ForeColor="Blue" Operator="LessThan" Type="Date" Display="Dynamic">!</asp:CompareValidator>
                                </td>
                                <td class="TextRigthBottom" rowspan="5" style="width: 50px">
                                    <asp:UpdatePanel ID="UpdatePanelBuscar" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc3:Progreso ID="ProgresoBuscar" runat="server" AssociatedUpdatePanelID="UpdatePanelBuscar"
                                                DisplayAfter="0" DynamicLayout="false" />
                                            <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Images/search.gif" OnClick="btnBuscar_Click"
                                                ValidationGroup="FiltroSF" />&nbsp;
                                            <asp:ImageButton ID="btnEliminarBusqueda" runat="server" ImageUrl="~/Images/trash.gif"
                                                CausesValidation="False" AlternateText="Eliminar filtro busqueda" Enabled="true"
                                                ToolTip="Eliminar filtro busqueda" OnClick="btnEliminarBusqueda_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnEliminarBusqueda" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbCuenta" runat="server" GroupName="rbBusquedaGroup" Text="Cuenta catastral"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td style="width: 65%;">
                                    <asp:TextBox ID="txtRegion" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ControlToValidate="txtRegion"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerida una región" ValidationGroup="FiltroSF"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtManzana" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvManzana" runat="server" ControlToValidate="txtManzana"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerida una manzana" ValidationGroup="FiltroSF"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLote" runat="server" Enabled="false" MaxLength="2" Width="20px"
                                        onkeypress="return validaAlfanumerico(event)" onblur="javascript:if(this.value!=''){rellenar(this,this.value,2);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLote" runat="server" ControlToValidate="txtLote"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un lote" ValidationGroup="FiltroSF"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtUnidadPrivativa" runat="server" Enabled="false" MaxLength="3"
                                        onkeypress="return validaAlfanumerico(event)" Width="30px" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUnidadPrivativa" runat="server" ControlToValidate="txtUnidadPrivativa"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un condominio"
                                        ValidationGroup="FiltroSF" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtCuenta" runat="server" Enabled="False" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 37%">
                                    <asp:RadioButton ID="rbNotario" runat="server" GroupName="rbBusquedaGroup" Text="Notario "
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumNot" runat="server" Enabled="False" MaxLength="4" ReadOnly="true" Width="30"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="txtNomNot" runat="server" Enabled="False"  ReadOnly="true" Width="234"></asp:TextBox>
                                    <asp:TextBox ID="txtIdPerNot" runat="server" Enabled="False" MaxLength="10" ReadOnly="true"  Visible="false"></asp:TextBox>
                                   <asp:ImageButton ID="btnNotario" runat="server" AlternateText="Asignar notario"
                                         ImageUrl="~/Images/user_p.gif" Enabled="false" OnClick="btnNotario_Click" 
                                         CausesValidation="False" Height="16px" />
                                   <cc1:ModalPopupExtender ID="btnNotario_ModalPopupExtender" runat="server" DynamicServicePath=""
                                         Enabled="True" TargetControlID="btnNotarioHidden" PopupControlID="PnlModalBuscarNotario"
                                         BackgroundCssClass="PanelModalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                     <asp:HiddenField runat="server" ID="btnNotarioHidden" 
                                         onvaluechanged="btnNotarioHidden_ValueChanged" /> 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbNumPresentacion" runat="server" GroupName="rbBusquedaGroup"
                                        Text="Número de presentación" AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumPresentacion" runat="server" Enabled="False" MaxLength="15"
                                        SkinID="TextBoxObligatorio"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNumPresentacion" runat="server" ControlToValidate="txtNumPresentacion"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un Número de presentación"
                                        ValidationGroup="FiltroSF" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revtxtNumPresentacion" runat="server" ErrorMessage="Formato número presentación erroneo. El número de presentación debe tener el formato I-aaaa-zzzzzz (Ej: I-2010-1154)"
                                        ControlToValidate="txtNumPresentacion" Display="Dynamic" SetFocusOnError="True"
                                        ValidationExpression="^I-[0-9]{4}-[0-9]{1,9}(\s)*$" ValidationGroup="FiltroSF"
                                        ForeColor="Blue">!</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEstadoDeclaraciones" SkinID="Titulo2" class="TextLeftTop" runat="server"
                                        Text="Estado Declaración"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEstadoDeclaracion" runat="server" CssClass="TextBoxNormal">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="PnlModalBuscarNotario" Style="width: 700px; display: none"
                        SkinID="Modal">
                        <uc2:ModalBuscarNotarios ID="ModalBuscarNotarios" runat="server" OnConfirmClick="buscarNotario_ConfirmClick" />
                    </asp:Panel>
                    <cc1:AlwaysVisibleControlExtender ID="PnlModalBuscarNotarioAlwaysVisibleControlExtender"
                        runat="server" TargetControlID="PnlModalBuscarNotario" Enabled="false">
                    </cc1:AlwaysVisibleControlExtender>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <uc3:Progreso ID="ProgresoGrid" runat="server" AssociatedUpdatePanelID="UpdatePanelGrid"
                DisplayAfter="0" DynamicLayout="false" />
            <div class="DivPaddingBotton TextLeftTop">
                <asp:Label ID="lblTituloListadoDeclaraciones" SkinID="Titulo2" class="TextLeftTop"
                    runat="server" Text="Listado de declaraciones"></asp:Label>
                <br />
                <asp:HiddenField runat="server" ID="HiddenIdPersonaToken" />
                <asp:HiddenField runat="server" ID="HiddenTokenModal" />
                <asp:GridView ID="gridViewDeclaraciones" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" PagerSettings-Mode="Numeric" GridLines="Horizontal" DataKeyNames="IDDECLARACION,CODESTADODECLARACION"
                    EmptyDataText="No hay declaraciones para el filtro seleccionado" OnPageIndexChanging="gridViewDeclaraciones_PageIndexChanging"
                    OnSorting="gridViewDeclaraciones_Sorting" OnSelectedIndexChanged="gridViewDeclaraciones_SelectedIndexChanged"
                    OnRowDataBound="gridViewDeclaraciones_RowDataBound" DataSourceID="odsDeclaracionFechaSF">
                    <Columns>
                        <asp:BoundField HeaderText="Nº" DataField="NUMPRESENTACION" SortExpression="NUMPRESENTACION">
                            <ItemStyle Width="80" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TIPO" HeaderText="Tipo" SortExpression="TIPO">
                            <ItemStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Estado" DataField="ESTADO" SortExpression="ESTADO">
                            <ItemStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Notario" DataField="IDPERSONADECLARANTE" SortExpression="IDPERSONADECLARANTE">
                            <ItemStyle Width="45px" HorizontalAlign="Right"/>
                        </asp:BoundField> 
                        <asp:BoundField DataField="PORCACTOTOTAL" HeaderText="%" SortExpression="PORCACTOTOTAL" DataFormatString="{0:p}" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle  HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="F. Pres." DataField="FECHAPRESENTACION" DataFormatString="{0:dd-MM-yy}"
                            SortExpression="FECHAPRESENTACION" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="F.LC." DataField="FECHALINEACAPTURA" DataFormatString="{0:dd-MM-yy}"
                            SortExpression="FECHALINEACAPTURA" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="50px" HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="FECHAPAGO" DataFormatString="{0:dd-MM-yy}" HeaderText="F. Pago"
                            SortExpression="FECHAPAGO"  HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="50px" HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="LC" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxLC" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle  HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pag." HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxPAG" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Siscor"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="checkboxSISCOR" Enabled="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:Button ID="ButtonSelect" runat="server" CausesValidation="False" CommandName="Select"
                                    Text="Ver" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SUJETO" HeaderText="NomNotario" SortExpression="SUJETO" visible="false">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsDeclaracionFechaSF" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="ObtenerDeclaracionesPorFechaSF" TypeName="DseDeclaracionIsaiPagger"
                    EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="NumTotalFilasFechaSF"
                    SortParameterName="SortExpression" StartRowIndexParameterName="indice">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenFechaIni" Name="fechaInicio" PropertyName="Value"
                            Type="DateTime" />
                        <asp:ControlParameter ControlID="HiddenFechaFin" Name="fechaFin" PropertyName="Value"
                            Type="DateTime" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" DefaultValue="" Name="idNotario"
                            PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenCodEstado" Name="codEstado" PropertyName="Value"
                            Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div style="text-align: right">
                <asp:Label ID="lblCount" runat="server"></asp:Label>
            </div>
            <div>
                <asp:HiddenField ID="HiddenIdDeclaracion" runat="server" />
                <asp:HiddenField ID="HiddenFechaIni" runat="server" />
                <asp:HiddenField ID="HiddenFechaFin" runat="server" />
                <asp:HiddenField ID="HiddenCodEstado" runat="server" />
                <asp:HiddenField ID="HiddenCuentaCat" runat="server" />
                <asp:HiddenField ID="HiddenRegion" runat="server" />
                <asp:HiddenField ID="HiddenManzana" runat="server" />
                <asp:HiddenField ID="HiddenLote" runat="server" />
                <asp:HiddenField ID="HiddenUPrivativa" runat="server" />
                <asp:HiddenField ID="HiddenSujeto" runat="server" />
                <asp:HiddenField ID="HiddenIdSujeto" runat="server" />
                <asp:HiddenField ID="HiddenSujeto0" runat="server" />
                <asp:HiddenField ID="HiddenNumPres" runat="server" />
                <asp:ObjectDataSource ID="odsDeclaracionCuentaSF" runat="server" EnablePaging="True"
                    MaximumRowsParameterName="pageSize" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="NumTotalObtenerDeclaracionesPorCuentaSF" SelectMethod="ObtenerDeclaracionesPorCuentaSF"
                    SortParameterName="SortExpression" StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenRegion" Name="region" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenManzana" Name="manzana" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenLote" Name="lote" PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="HiddenUPrivativa" Name="unidad" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenCodEstado" Name="codEstado" PropertyName="Value"
                            Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="odsDeclaracionSujetoSF" runat="server" EnablePaging="True"
                    MaximumRowsParameterName="pageSize" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="NumTotalObtenerDeclaracionesPorSujetoSF" SelectMethod="ObtenerDeclaracionesPorSujetoSF"
                    SortParameterName="SortExpression" StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenIdSujeto" Name="idPerNot" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenCodEstado" Name="codEstado" PropertyName="Value"
                            Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="odsDeclaracionNumPresSF" runat="server" EnablePaging="True"
                    MaximumRowsParameterName="pageSize" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="NumTotalObtenerDeclaracionesPorNumPresSF" SelectMethod="ObtenerDeclaracionesPorNumPresSF"
                    SortParameterName="SortExpression" StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenNumPres" Name="numPres" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenCodEstado" Name="codEstado" PropertyName="Value"
                            Type="Int32" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <input type="hidden" id="hidBusquedaActual" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal" runat="Server">
    <uc1:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
