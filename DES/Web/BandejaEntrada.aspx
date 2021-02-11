<%@ Page Language="C#" MasterPageFile="~/MasterPageFE/MasterDetalleFE.master" AutoEventWireup="true"
    CodeFile="BandejaEntrada.aspx.cs" Inherits="BandejaEntrada" Title="ISAI - Bandeja de entrada" %>

<%@ Register Src="UserControls/Navegacion.ascx" TagName="Navegacion" TagPrefix="uc1" %>
<%@ Register Src="UserControls/MenuLocal.ascx" TagName="MenuLocal" TagPrefix="uc2" %>
<%@ Register Src="UserControlsCommon/Progreso.ascx" TagName="Progreso" TagPrefix="uc3" %>
<%@ Register Src="UserControls/ModalBuscarPeritos.ascx" TagName="ModalBuscarPeritos"
    TagPrefix="uc4" %>
<%@ Register Src="UserControlsCommon/ModalInfo.ascx" TagName="ModalInfo" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControlsCommon/ModalInfoExcepcion.ascx" TagName="ModalInfoExcepcion"
    TagPrefix="uc11" %>
<asp:Content ID="ContentDHead" ContentPlaceHolderID="ContentPlaceHolderDHead" runat="Server">

    <script src="<%= this.ResolveUrl("~/JS/documental.js") %>" type="text/javascript"></script>

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
<asp:Content ID="ContentImagen" ContentPlaceHolderID="ContentPlaceHolderDImagen"
    runat="Server">
    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/caracter_detalle.jpg" />
</asp:Content>
<asp:Content ID="ContentRuta" ContentPlaceHolderID="ContentPlaceHolderDRuta" runat="Server">
    <uc1:Navegacion ID="navegacion" runat="server" />
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
            <div class="DivPaddingBotton">
                <fieldset class="formulario">
                    <legend class="formulario">Filtro de avalúos</legend>
                    <asp:Panel ID="Panel1" runat="server">
                        <table class="TextLeftMiddle" style="width: 100%">
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:ValidationSummary ID="vsFiltroAvaluos" runat="server" ValidationGroup="FiltroAvaluos" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbFechas" runat="server" CssClass="Titulo2" GroupName="rbBusquedaGroup"
                                        Text="Rango de fechas" AutoPostBack="True" Checked="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaIni" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaIni" PopupButtonID="btnFechaIni">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaIni" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                    <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ErrorMessage="Requerida una fecha"
                                        ValidationGroup="FiltroAvaluos" ControlToValidate="txtFechaIni" Display="Dynamic">*</asp:RequiredFieldValidator><asp:CompareValidator
                                            ID="cvFechaInicio" runat="server" ErrorMessage="Fecha errónea" ForeColor="Blue"
                                            Type="Date" ValidationGroup="FiltroAvaluos" Operator="DataTypeCheck" ControlToValidate="txtFechaIni"
                                            Display="Dynamic">!</asp:CompareValidator>
                                    <span class="TextoNormal">- </span>
                                    <asp:TextBox ID="txtFechaFin" runat="server" SkinID="TextBoxObligatorio" MaxLength="10"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFechaFin" PopupButtonID="btnFechaFin">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton runat="server" ID="btnFechaFin" ImageUrl="~/images/calendario.png"
                                        CausesValidation="false" Height="16" Width="16" AlternateText="Seleccione una Fecha" />
                                    <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ErrorMessage="Requerida una fecha"
                                        ValidationGroup="FiltroAvaluos" ControlToValidate="txtFechaFin" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvFechaFin" runat="server" ErrorMessage="Fecha errónea"
                                        Operator="DataTypeCheck" Type="Date" ValidationGroup="FiltroAvaluos" ControlToValidate="txtFechaFin"
                                        Display="Dynamic">!</asp:CompareValidator>
                                    <asp:CompareValidator ID="cvRangoFechas" runat="server" ErrorMessage="Rango entre fechas erróneo"
                                        ForeColor="Blue" ControlToValidate="txtFechaIni" Type="Date" ValidationGroup="FiltroAvaluos"
                                        ControlToCompare="txtFechaFin" Operator="LessThan" Display="Dynamic">!</asp:CompareValidator>
                                 </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbCuenta" runat="server" CssClass="Titulo2" GroupName="rbBusquedaGroup"
                                        Text="Cuenta catastral" AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td>
                                   
                                    <asp:TextBox ID="txtRegion" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ControlToValidate="txtRegion"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerida una región" ValidationGroup="FiltroAvaluos"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtManzana" runat="server" Enabled="false" MaxLength="3" Width="30px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvManzana" runat="server" ControlToValidate="txtManzana"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerida una manzana" ValidationGroup="FiltroAvaluos"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLote" runat="server" Enabled="false" MaxLength="2" Width="20px"
                                        onkeypress="return validaAlfanumerico(event)" SkinID="TextBoxObligatorio" onblur="javascript:if(this.value!=''){rellenar(this,this.value,2);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLote" runat="server" ControlToValidate="txtLote"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un lote" ValidationGroup="FiltroAvaluos"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtUnidadPrivativa" runat="server" Enabled="false" MaxLength="3"
                                        onkeypress="return validaAlfanumerico(event)" Width="30px" SkinID="TextBoxObligatorio"
                                        onblur="javascript:if(this.value!=''){rellenar(this,this.value,3);}"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUnidadPrivativa" runat="server" ControlToValidate="txtUnidadPrivativa"
                                        SetFocusOnError="True" Enabled="false" ErrorMessage="Requerido un condominio"
                                        ValidationGroup="FiltroAvaluos" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtCuenta" runat="server" Enabled="False" Visible="false" SkinID="TextBoxObligatorio"></asp:TextBox>
                                      </td>
                            </tr>
                           
                            <tr>
                                <td style="width: 182px">
                                    <asp:RadioButton ID="rbNumeroAvaluo" runat="server" GroupName="rbBusquedaGroup" Text="Nº avalúo"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td style="width: 326px">
                                    <asp:TextBox ID="textNumeroAvaluo" runat="server" Enabled="False" SkinID="TextBoxObligatorio"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNumeroAvaluo" runat="server" ErrorMessage="Requiere un número de avalúo"
                                        ControlToValidate="textNumeroAvaluo" SetFocusOnError="True" ValidationGroup="FiltroAvaluos"
                                        Enabled="False" Display="Dynamic">*</asp:RequiredFieldValidator>
                                     <asp:Label ID="lblNumPerSoci" SkinID="Titulo2" class="TextLeftTop" runat="server"
                                        Text="Per. o Soc." Visible="true"></asp:Label>
                                    <asp:TextBox ID="txtPerito" runat="server" Enabled="False" Width="85px"></asp:TextBox>
                                    <asp:ImageButton ID="btnPeritos" runat="server" CausesValidation="False" ImageUrl="~/Images/user_p.gif"
                                        OnClick="btnPeritos_Click" ToolTip="Buscar Peritos" Visible="true" Enabled="False" />
                                    <cc1:ModalPopupExtender ID="btnPeritos_ModalPopupExtender" runat="server" DynamicServicePath=""
                                        Enabled="True" TargetControlID="btnPeritoHidden" PopupControlID="PnlModalBuscarPerito"
                                        BackgroundCssClass="PanelModalBackground" DropShadow="true">
                                    </cc1:ModalPopupExtender>
                                    <asp:HiddenField runat="server" ID="btnPeritoHidden" />
                                    <asp:Panel runat="server" ID="PnlModalBuscarPerito" Style="width: 700px; display: none"
                                        SkinID="Modal">
                                        <uc4:ModalBuscarPeritos ID="ModalBuscarPeritos1" runat="server" OnConfirmClick="buscarPerito_ConfirmClick" />
                                    </asp:Panel>
                                     <asp:RangeValidator ID="revNumeroPerito" runat="server" ErrorMessage="Rango del número de perito erróneo"
                                        ValidationGroup="FiltroAvaluos" ControlToValidate="txtPerito" ForeColor="Blue"
                                        MaximumValue="99999999999999999999" MinimumValue="0" SetFocusOnError="True" Enabled="False"
                                        Display="Dynamic">!</asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 182px; height: 24px;">
                                    <asp:RadioButton ID="rbIdAvaluo" runat="server" GroupName="rbBusquedaGroup" Text="Nº único avalúo"
                                        AutoPostBack="True" OnCheckedChanged="rbBusquedaGroup_CheckedChanged" />
                                </td>
                                <td style="height: 24px">
                                    <asp:TextBox ID="txtIdAvaluo" runat="server" SkinID="TextBoxObligatorio" Enabled="False"
                                        MaxLength="20" />
                                    <asp:RequiredFieldValidator ID="rfvIdAvaluo" runat="server" ControlToValidate="txtIdAvaluo"
                                        Display="Dynamic" Enabled="False" ErrorMessage="Requiere un identificador de avalúo"
                                        SetFocusOnError="True" ValidationGroup="FiltroAvaluos">*</asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator ID="revIdAvaluo" runat="server" ErrorMessage="Formato nº único erroneo. El nº único debe tener el formato A-xxx-aaaa-zzzzzz (Ej: A-COM-2010-1154)"
                                        ControlToValidate="txtIdAvaluo" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^A-(CAT|COM)-[0-9]{4}-[0-9]{1,9}(\s)*$"
                                        ValidationGroup="FiltroAvaluos" ForeColor="Blue">!</asp:RegularExpressionValidator>
                                 </td></tr>
                            <tr>
                                <td style="width: 182px">
                                    <asp:Label ID="lblVigente" runat="server" SkinID="Titulo2" Text="Vigencia" Visible="true"></asp:Label>
                                    <asp:CheckBox ID="checkVigente" runat="server" Checked="True" Text="Vigente" Visible="False" />
                                </td>
                                <td style="width: 326px">
                                    <div>
                                        <asp:RadioButton ID="RadioButtonTodos" runat="server" Text="Todos" GroupName="vigencia"
                                           />
                                        <asp:RadioButton ID="RadioButtonVigente" runat="server" Text="Vigentes" GroupName="vigencia"
                                            Checked="True" />
                                        <asp:RadioButton ID="RadioButtonNoVigente" runat="server" Text="No vigentes" GroupName="vigencia" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="TextRigthBottom" colspan="2" align="right">
                                    <asp:UpdatePanel ID="UpdatePanelBuscar" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                        <ContentTemplate>
                                            <uc3:Progreso ID="ProgresoBuscar" runat="server" AssociatedUpdatePanelID="UpdatePanelBuscar"
                                                DisplayAfter="0" />
                                            <asp:ImageButton ID="btnBuscar" runat="server" CssClass="ImageButton" OnClick="btnBuscar_Click"
                                                ImageUrl="~/Images/search.gif" ValidationGroup="FiltroAvaluos" />&nbsp;
                                            <asp:ImageButton ID="btnEliminarBusqueda" runat="server" ImageUrl="~/Images/trash.gif"
                                                CausesValidation="False" AlternateText="Eliminar filtro busqueda" Enabled="true"
                                                ToolTip="Eliminar filtro busqueda" OnClick="btnEliminarBusqueda_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="HiddenIdPersonaToken" />
            <asp:HiddenField runat="server" ID="HiddenTokenModal" />
            <asp:HiddenField ID="HiddenVigente" runat="server" Value="T" />
            <asp:HiddenField ID="HiddenFechaFin" runat="server" />
            <asp:HiddenField ID="HiddenFechaIni" runat="server" />
            <asp:HiddenField ID="HiddenRegistro" runat="server" />
            <asp:HiddenField ID="HiddenCuentaCat" runat="server" />
            <asp:HiddenField ID="HiddenIdAvaluo" runat="server" />
            <asp:HiddenField ID="HiddenNumAvaluo" runat="server" />
            <asp:HiddenField ID="HiddenNumUnicoAv" runat="server" />
            <asp:ObjectDataSource ID="odsPorFecha" runat="server" EnablePaging="True" MaximumRowsParameterName="pageSize"
                SelectCountMethod="NumTotalFilasFechaNotario" SelectMethod="ObtenerAvaluosPorFechaNotario"
                StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger" SortParameterName="SortExpression"
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenFechaIni" Name="fechaInicio" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenFechaFin" Name="fechaFin" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="HiddenRegistro" Name="registro" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenVigente" Name="vigente" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenNumAvaluo" Name="numValuo" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenNumUnicoAv" Name="idAvaluo" PropertyName="Value"
                        Type="string" />
                    <asp:Parameter Name="pageSize" Type="Int32" />
                    <asp:Parameter Name="indice" Type="Int32" />
                    <asp:Parameter Name="SortExpression" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
          
            <asp:ObjectDataSource ID="odsPorFechaSF" runat="server" EnablePaging="True" MaximumRowsParameterName="pageSize"
                OldValuesParameterFormatString="original_{0}" SelectCountMethod="NumTotalFilasFechaNotarioSF"
                SelectMethod="ObtenerAvaluosPorFechaNotarioSF" SortParameterName="SortExpression"
                StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenFechaIni" Name="fechaInicio" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenFechaFin" Name="fechaFin" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="HiddenRegistro" Name="registro" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenVigente" Name="vigente" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenNumAvaluo" Name="numValuo" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenNumUnicoAv" Name="idAvaluo" PropertyName="Value"
                        Type="String" />
                    <asp:Parameter Name="pageSize" Type="Int32" />
                    <asp:Parameter Name="indice" Type="Int32" />
                    <asp:Parameter Name="SortExpression" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsPorCuentaCatastral" runat="server" SelectMethod="ObtenerAvaluosPorCuentaCatastralNotario"
                TypeName="DseDeclaracionIsaiPagger" EnablePaging="True" MaximumRowsParameterName="pageSize"
                SelectCountMethod="NumTotalFilasCuentaCatastralNotario" StartRowIndexParameterName="indice"
                SortParameterName="SortExpression" OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="HiddenCuentaCat" Name="cuentaCatastral" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="HiddenRegistro" Name="registro" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="HiddenVigente" Name="vigente" PropertyName="Value"
                        Type="String" />
                    <asp:Parameter Name="pageSize" Type="Int32" />
                    <asp:Parameter Name="indice" Type="Int32" />
                    <asp:Parameter Name="SortExpression" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <div class="DivPaddingBotton TextLeftTop">
            
                <asp:ObjectDataSource ID="odsPorCuentaCatastralSF" runat="server" EnablePaging="True"
                    MaximumRowsParameterName="pageSize" OldValuesParameterFormatString="original_{0}"
                    SelectCountMethod="NumTotalFilasCuentaCatastralNotarioSF" SelectMethod="ObtenerAvaluosPorCuentaCatastralNotarioSF"
                    SortParameterName="SortExpression" StartRowIndexParameterName="indice" TypeName="DseDeclaracionIsaiPagger">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="HiddenCuentaCat" Name="cuentaCatastral" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenIdPersonaToken" Name="idNotario" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="HiddenRegistro" Name="registro" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="HiddenVigente" Name="vigente" PropertyName="Value"
                            Type="String" />
                        <asp:Parameter Name="pageSize" Type="Int32" />
                        <asp:Parameter Name="indice" Type="Int32" />
                        <asp:Parameter Name="SortExpression" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblTituloListado" SkinID="Titulo2" class="TextLeftTop" runat="server"
                                Text="Listado de avalúos"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gridViewAvaluos" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" GridLines="Horizontal" EmptyDataText="No hay avaluos para el filtro seleccionado"
                                DataKeyNames="IDAVALUO"  OnSelectedIndexChanged="gridViewAvaluos_SelectedIndexChanged"
                                OnSorting="gridViewAvaluos_Sorting" OnRowDataBound="gridViewAvaluos_RowDataBound" DataSourceID="odsPorFecha">
                                <Columns>
                                    <asp:BoundField HeaderText="IDAVALUO" DataField="IDAVALUO" Visible="False" SortExpression="IDAVALUO">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nº Único" DataField="NUMEROUNICO" SortExpression="NUMEROUNICO">
                                        <ItemStyle Width="95px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Cuenta cat." DataField="CUENTACATASTRAL" SortExpression="CUENTACATASTRAL">
                                       <ItemStyle Width="80px" />  
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Fecha" DataField="FECHA_DOCDIGITAL" DataFormatString="{0:dd-MM-yy}"
                                        SortExpression="FECHA_DOCDIGITAL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />  
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHAVALORREFERIDO" HeaderText="FECHAVALORREFERIDO" Visible="False"
                                        SortExpression="FECHAVALORREFERIDO" />
                                    <asp:BoundField DataField="CODESTADOAVALUO" HeaderText="CODESTADOAVALUO" Visible="False"
                                        SortExpression="CODESTADOAVALUO" />
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" SortExpression="ESTADO" />
                                    <asp:BoundField DataField="REGISTRO_PERITO" HeaderText="Perito" SortExpression="REGISTRO_PERITO"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    </asp:BoundField>
                                      <asp:BoundField DataField="REGISTRO_SOCIEDAD" HeaderText="Sociedad" SortExpression="REGISTRO_SOCIEDAD"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
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
                                    <asp:TemplateField ShowHeader="True">
                                        <ItemTemplate>
                                            <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Select"
                                                Text="Declaraciones" Width="78" />
                                        </ItemTemplate>
                                        <ItemStyle Width="78"/>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <uc3:Progreso ID="ProgresoGrid" runat="server" AssociatedUpdatePanelID="UpdatePanelGrid"
                DisplayAfter="0" />
            <input type="hidden" id="hidBusquedaActual" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="ContentMenu" ContentPlaceHolderID="ContentPlaceHolderDMenuLocal"
    runat="Server">
    <uc2:MenuLocal ID="MenuLocal1" runat="server" />
</asp:Content>
